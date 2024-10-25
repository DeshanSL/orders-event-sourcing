using Orders.Api.Orders.Aggregate.ValueObjects;
using Orders.Api.Products.Aggregate.ValueObjects;
using Orders.Api.SharedKernel;

namespace Orders.Api.Orders.Aggregate.Entities;

public sealed class LineItem : Entity<LineItemId>
{
    public ProductId ProductId { get; init; }

    public int Qty { get; private set; }

    public LineItem(LineItemId id, ProductId productId, int qty) : base(id)
    {
        ProductId = productId;
        Qty = qty;
    }
}
