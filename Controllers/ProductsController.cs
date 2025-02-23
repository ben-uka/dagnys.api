using dagnys.api.Interfaces;
using dagnys.api.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace dagnys.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <summary>
    /// Lägg till en ny produkt.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /product
    ///     {
    ///       "productId": 3,
    ///       "productName": "Saltskorpa",
    ///       "price": 85,
    ///       "weight": 1,
    ///       "unit": "kg"
    ///       "bestBeforeDate": "2025-02-27T",
    ///		  "bakedOnDate": "2025-02-20"
    ///     }
    /// </remarks>
    /// <param name="model">A json document/object representing a new product</param>
    /// <returns></returns>
    /// <response code = "201">If the product was successfully added to the system.</response>
    /// <response code = "400">If the data was in the wrong format or if the supplier already exists.</response>
    /// <response code = "500">If the server is not responding.</response>
    [HttpPost()]
    public async Task<IActionResult> AddProduct(ProductPostViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest("Information för att lägga till produkt saknas.");

            if (await _unitOfWork.ProductRepository.AddProduct(model))
            {
                if (_unitOfWork.HasChanges())
                {
                    await _unitOfWork.Complete();
                }
                return StatusCode(201);
            }
            else
            {
                return BadRequest("Kunde inte lägga till produkt");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// Listar alla produkter.
    /// </summary>
    /// <returns>A list of all available products.</returns>
    /// <response code="200">If the list was successfully retrieved.</response>
    /// <response code="500">If a server error occurs.</response>
    [HttpGet()]
    public async Task<ActionResult<List<ProductViewModel>>> GetAllProducts()
    {
        return Ok(await _unitOfWork.ProductRepository.ListAllProducts());
    }

    /// <summary>
    /// Hämtar produkt på ID.
    /// </summary>
    /// <param name="id">The ID of the product.</param>
    /// <returns>The product matching the given ID.</returns>
    /// <response code="200">If the product was found.</response>
    /// <response code="404">If no product with the specified ID exists.</response>
    /// <response code="500">If a server error occurs.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductViewModel>> GetProduct(int id)
    {
        try
        {
            var response = await _unitOfWork.ProductRepository.FindById(id);

            if (response == null)
            {
                return NotFound($"Kunde inte hitta produkt med id {id}");
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    /// Uppdatera pris på en specifik produkt.
    /// </summary>
    /// <param name="id">The ID of the product.</param>
    /// <param name="model">A JSON object containing the new price information.</param>
    /// <returns>Returns 204 if the update was successful, 400 if invalid data, or 404 if the product was not found.</returns>
    /// <response code="204">If the price was successfully updated.</response>
    /// <response code="400">If the input data is invalid.</response>
    /// <response code="404">If no product with the specified ID exists.</response>
    /// <response code="500">If a server error occurs.</response>
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdatePrice(int id, PricePatchProductViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest("Information för att uppdatera produkt saknas.");

        var result = await _unitOfWork.ProductRepository.UpdatePrice(id, model);

        if (result)
        {
            return NoContent();
        }
        else
        {
            return BadRequest("Kunde inte uppdatera priset på produkt med id {id}");
        }
    }
}
