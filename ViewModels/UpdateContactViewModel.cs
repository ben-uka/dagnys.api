using System.ComponentModel.DataAnnotations;
using dagnys.api.Entities;

namespace dagnys.api.ViewModels.Customer;

public class UpdateContactViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Förnamn måste anges.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Efternamn måste anges.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Mejladress måste anges.")]
    public string Email { get; set; }
}
