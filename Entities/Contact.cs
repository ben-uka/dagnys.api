using System.ComponentModel.DataAnnotations;

namespace dagnys.api.Entities;

public class Contact
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    [Required]
    public string Email { get; set; }
    public IList<CustomerContact> CustomerContacts { get; set; }
}
