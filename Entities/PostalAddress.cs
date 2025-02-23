namespace dagnys.api.Entities;

public class PostalAddress
{
    public int Id { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public IList<Address> Addresses { get; set; }
}
