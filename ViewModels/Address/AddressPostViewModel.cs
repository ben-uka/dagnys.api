using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace dagnys.api.ViewModels.Address;

public enum AddressTypeEnum
{
    Delivery = 1,
    Invoice = 2,
    Distribution = 3,
}

public class AddressPostViewModel
{
    [Required(ErrorMessage = "Adress måste anges.")]
    public string AddressLine { get; set; }

    [Required(ErrorMessage = "Postkod måste anges.")]
    [StringLength(8)]
    public string ZipCode { get; set; }

    [Required(ErrorMessage = "Ort måste anges.")]
    public string City { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AddressTypeEnum AddressType { get; set; }
}
