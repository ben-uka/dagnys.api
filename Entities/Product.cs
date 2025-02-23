namespace dagnys.api.Entities;

public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public double Weight { get; set; }
    public string Unit { get; set; }
    public DateTime BestBeforeDate { get; set; }
    public DateTime BakedOnDate { get; set; }
    public IList<OrderItem> OrderItems { get; set; }
}
