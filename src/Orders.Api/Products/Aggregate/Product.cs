using Orders.Api.Products.Aggregate.ValueObjects;
using Orders.Api.SharedKernel;

namespace Orders.Api.Products.Aggregate;

public sealed class Product : AggregateRoot<ProductId>
{
    public Product(ProductId id, decimal price) : base(id)
    {
        Price = price;
    }

    public decimal Price { get; private set; }
}
