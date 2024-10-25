namespace Orders.Api.SharedKernel;

public class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
{
    private readonly List<Event> events;
    public IReadOnlyList<Event> Events => events.AsReadOnly();
    public AggregateRoot(TId id) : base(id)
    {
    }

    protected void AggregateHasChanged(Event @event)
    {
        Uncommitted(@event);
        PublishChange(@event);
    }

    private void Uncommitted(Event @event) => events.Add(@event);

    private void PublishChange(Event @event)
    {
        dynamic aggregate = this;
        aggregate.Apply((dynamic)@event);
    }

    public void PublishChanges(params Event[] events)
    {
        foreach (var @event in events)
        {
            PublishChange(@event);
        }
    }
}

public interface IAggregateRoot
{
    void PublishChanges(params Event[] events); 
}
