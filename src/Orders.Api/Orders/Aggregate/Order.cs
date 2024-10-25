using Orders.Api.Orders.Aggregate.Entities;
using Orders.Api.Orders.Aggregate.Events;
using Orders.Api.Orders.Aggregate.ValueObjects;
using Orders.Api.Products.Aggregate.ValueObjects;
using Orders.Api.SharedKernel;
using Returns;

namespace Orders.Api.Orders.Aggregate;

public sealed class Order : AggregateRoot<OrderId>
{
    private readonly List<LineItem> lineItems;
    public Order(OrderId id, CustomerId customerId, List<LineItem> lineItems) : base(id)
    {
        State = OrderState.InProgress();
        CustomerId = customerId;
        this.lineItems = lineItems;
    }

    public OrderState State { get; private set; }
    public CustomerId CustomerId { get; private set; }

    public IReadOnlyList<LineItem> LineItems => lineItems.AsReadOnly();

    #region Functions 
    public void AddLineItem(ProductId productId, int qty)
    {
        var @event = new OrderEvents.LineItemAdded(Guid.NewGuid(), productId.Value, qty);
        AggregateHasChanged(@event);
    }

    public Return RemoveLineItem(LineItemId lineItemId)
    {
        if (lineItems.Count <= 1)
        {
            return Fault.Conflict("At least one line item should be available in a order.");
        }

        if (lineItems.All(a => a.Id != lineItemId))
        {
            return Fault.NotFound("Line item not available.");
        }

        var @event = new OrderEvents.LineItemRemoved(lineItemId.Id);
        AggregateHasChanged(@event);    
        return Return.Success();
    }

    #endregion

    #region Event publishers

    public void Apply(OrderEvents.LineItemAdded @event)
    {
        var lineItem = new LineItem(new LineItemId(@event.LineItemId), new ProductId(@event.ProductId), @event.Qty);
        lineItems.Add(lineItem);
    }

    public void Apply(OrderEvents.LineItemRemoved @event)
    {
        var lineItem = lineItems.FirstOrDefault(a => a.Id == new LineItemId(@event.LineItemId));
        if (lineItem != null)
        {
            lineItems.Remove(lineItem);
        }
    }

    #endregion

    
}
