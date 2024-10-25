namespace Orders.Api.SharedKernel;

public class Entity<TId>
{
    public TId Id { get; init; }

    public Entity(TId id)
    {
        Id = id;
    }
}