using System.ComponentModel.DataAnnotations;

namespace dagnys.api.Entities;

public class Customer
{
    public int Id { get; set; }

    [Required]
    public string StoreName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public IList<Order> Orders { get; set; }
    public IList<CustomerAddress> CustomerAddresses { get; set; }
    public IList<CustomerContact> CustomerContacts { get; set; }
}
