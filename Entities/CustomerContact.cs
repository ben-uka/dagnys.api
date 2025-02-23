namespace dagnys.api.Entities;

public class CustomerContact
{
    public int CustomerId { get; set; }
    public int ContactId { get; set; }
    public Customer Customer { get; set; }
    public Contact Contact { get; set; }
}
