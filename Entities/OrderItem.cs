namespace dagnys.api.Entities;

public class OrderItem
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal PricePerUnit { get; set; }
    public decimal TotalPrice => Quantity * PricePerUnit;
    public Order Order { get; set; }
    public Product Product { get; set; }
}
