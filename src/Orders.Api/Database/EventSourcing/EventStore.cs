
using Marten;
using Orders.Api.SharedKernel;

namespace Orders.Api.Database.EventSourcing;

public class EventStore : IAppEventStore
{
    private readonly IDocumentSession documentSession;

    public EventStore(IDocumentSession documentSession)
    {
        this.documentSession = documentSession;
    }
    public void StartStream<TAggregate>(Guid streamId, params Event[] events) where TAggregate : class, IAggregateRoot
    { 
        documentSession.Events.StartStream<TAggregate>(streamId, events);
    }

    public void StartStream<TId>(AggregateRoot<TId> aggregate, Func<AggregateRoot<TId>, Guid> streamId)
    {
        var streamGuid = streamId(aggregate);
        var events = aggregate.Events;

        documentSession.Events.StartStream(aggregate.GetType(),id:streamGuid, events);
    }

    public void Append(Guid streamId, params Event[] events)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync()
    {
        await documentSession.SaveChangesAsync();
    }
}

public interface IAppEventStore
{
    void StartStream<TAggregate>(Guid streamId, params Event[] events) where TAggregate : class, IAggregateRoot;
    void StartStream<TId>(AggregateRoot<TId> aggregate, Func<AggregateRoot<TId>, Guid> streamId);

    void Append(Guid streamId, params Event[] events);
    Task SaveChangesAsync();
}


