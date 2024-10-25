using Orders.Api.SharedKernel;

namespace Orders.Api.Orders.Aggregate.Events;

public class OrderEvents
{
    public record Created(Guid OrderId, DateTime OrderCreatedUtc, Guid CustomerId) : Event;
    public record LineItemAdded(Guid LineItemId, Guid ProductId, int Qty) : Event;
    public record LineItemRemoved(Guid LineItemId) : Event;
    public record LineItemQtyUpdated(Guid LineItemId, int UpdatedQty) : Event;
}

