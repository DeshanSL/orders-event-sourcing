using Returns;

namespace Orders.Api.Orders.Aggregate.ValueObjects;

public record OrderState
{

    public string Status { get; init; }

    private OrderState(string status)
    {
        Status = status;
    }

    public static OrderState InProgress()
    {
        return new OrderState("INPROG");
    } 
    public static OrderState ReadyToShip()
    {
        return new OrderState("REASHP");
    } 
    public static OrderState Shipped()
    {
        return new OrderState("SHIPED");
    } 
    public static OrderState Deleted()
    {
        return new OrderState("DELETD");
    } 
}
