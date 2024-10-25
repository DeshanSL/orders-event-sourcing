using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.Api.Database.EventSourcing;
using Orders.Api.Orders.Aggregate.ValueObjects;
using Returns;
using Slices.Endpoints.Mapper;

namespace Orders.Api.Orders.Features.CreateOrder;

public class CreateOrder : IFeature
{
    public record Command(Guid CustomerId, List<Command.LineItem> LineItems) : IRequest<Return<OrderId>>
    {
        public record LineItem(Guid ProductId, int Qty);
    };

    internal sealed class Handler : IRequestHandler<Command, Return<OrderId>>
    {
        private readonly IAppEventStore _store;

        public Handler(IAppEventStore store)
        {
            _store = store;
        }
        public async Task<Return<OrderId>> Handle(Command request, CancellationToken cancellationToken)
        {
            return new OrderId(Guid.Empty);
        }
    }


    public class Endpoints
    {
        public record Request(Guid CustomerId, List<Request.LineItem> LineItems)
        {
            public record LineItem(Guid ProductId, int Qty, bool test );
        }
        public void Map(IEndpointRouteBuilder app)
        {
            app.MapPost("api/orders", async ([FromBody] Request request, [FromServices] IMediator mediator) =>
            {
                var result = await mediator.Send(new Command(request.CustomerId,
                    request.LineItems.Select(a => new Command.LineItem(a.ProductId, a.Qty)).ToList()));
                return result.Value;
            });
        }
    }
}