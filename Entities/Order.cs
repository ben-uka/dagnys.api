namespace dagnys.api.Entities;

public class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public IList<OrderItem> OrderItems { get; set; }
    public Customer Customer { get; set; }
}
