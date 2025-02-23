namespace dagnys.api.Entities;

public class AddressType
{
    public int Id { get; set; }
    public string Value { get; set; }
    public virtual ICollection<Address> Addresses { get; set; }
}
