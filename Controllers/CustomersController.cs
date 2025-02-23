using dagnys.api.Interfaces;
using dagnys.api.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;

namespace dagnys.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <summary>
    /// Adds a new customer.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/customers
    ///     {
    ///       "storeName": "Floura AB",
    ///       "phone": "010-411 11 22",
    ///       "email": "floura@info.com",
    ///       "addresses": [
    ///         {
    ///           "addressLine": "Kammakargatan 11",
    ///           "zipCode": "151 23",
    ///           "city": "Hjärup",
    ///           "addressType": 1
    ///         },
    ///         {
    ///           "addressLine": "Flormakargatan 11",
    ///           "zipCode": "151 24",
    ///           "city": "Hjärup",
    ///           "addressType": 2
    ///         }
    ///       ]
    ///     }
    /// </remarks>
    /// <returns>A json document/object representing a new customer.</returns>
    /// <response code = "201">If the customer was successfully added to the system.</response>
    /// <response code = "400">If the data was in the wrong format or if the customer already exists.</response>
    /// <response code = "500">If the server is not responding.</response>
    [HttpPost()]
    public async Task<IActionResult> AddCustomer(CustomerPostViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest("Information för att lägga till kund saknas.");

            if (await _unitOfWork.CustomerRepository.Add(model))
            {
                if (_unitOfWork.HasChanges())
                {
                    await _unitOfWork.Complete();
                }
                return StatusCode(201);
            }
            else
            {
                return BadRequest("Kunde inte lägga till beställningen.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// List all customers.
    /// </summary>
    /// <returns>A json document/object with all current customers.</returns>
    /// <response code="200"> A json object representing each customer.</response>
    /// <response code="404">If no customers are found.</response>
    /// <response code="400">If input is in wrong format.</response>
    /// <response code="500">We are experiencing server issues.</response>
    [ProducesResponseType(typeof(CustomersViewModel), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [HttpGet("list")]
    public async Task<ActionResult<List<CustomersViewModel>>> GetAllCustomers()
    {
        return Ok(await _unitOfWork.CustomerRepository.List());
    }

    /// <summary>
    /// Lists specific customer.
    /// </summary>
    /// <returns>A json document/object with a specific customer.</returns>
    /// <response code="200">A json document/object with specific customer.</response>
    /// <response code="404">If no customer was found.</response>
    [ProducesResponseType(typeof(CustomerViewModel), 200)]
    [ProducesResponseType(404)]
    [HttpGet("find/{id}")]
    public async Task<ActionResult<CustomerViewModel>> GetCustomer(int id)
    {
        var response = await _unitOfWork.CustomerRepository.Find(id);

        if (response == null)
        {
            return NotFound($"Kunde inte hitta kund med id {id}");
        }

        return Ok(response);
    }

    /// <summary>
    /// Updates customers contact.
    /// </summary>
    /// <returns>StatusCode whether update was successfull or not.</returns>
    /// <response code="204">If contact was succesfully updated.</response>
    /// <response code="400">If input is in wrong format.</response>
    /// <response code="500">We are experiencing server issues.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateContactPerson(int id, UpdateContactViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest("Information för att uppdatera kund saknas.");

        var result = await _unitOfWork.CustomerRepository.UpdateContact(id, model);

        if (result)
        {
            return NoContent();
        }
        else
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}
