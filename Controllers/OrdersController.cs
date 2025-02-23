using dagnys.api.Entities;
using dagnys.api.Interfaces;
using dagnys.api.ViewModels;
using dagnys.api.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;

namespace dagnys.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <summary>
    /// Lägga till en ny order.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/orders/add
    ///     {
    ///         "orderNumber": "ORD123456",
    ///         "orderDate": "2024-02-22T10:00:00",
    ///         "customerId": 1,
    ///         "orderItems": [
    ///             {
    ///                 "productId": 1,
    ///                 "quantity": 3
    ///             },
    ///             {
    ///                 "productId": 2,
    ///                 "quantity": 1
    ///             }
    ///         ]
    ///     }
    /// </remarks>
    /// <param name="model">A json document/object representing a new order.</param>
    /// <returns>Returns 201 if successfully added, 400 if invalid data, or 500 if server error.</returns>
    /// <response code="201">If the order was successfully created.</response>
    /// <response code="400">If the data was in the wrong format or if the customer already exists.</response>
    /// <response code="500">If the server is not responding.</response>
    [HttpPost("add")]
    public async Task<IActionResult> AddOrder(OrderPostViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest("Information för att lägga till order saknas.");

        if (await _unitOfWork.OrderRepository.Add(model))
        {
            if (_unitOfWork.HasChanges())
            {
                await _unitOfWork.Complete();
            }
            return StatusCode(201);
        }
        else
        {
            return BadRequest();
        }
    }

    /// <summary>
    /// Hitta specifik order via orderdatum.
    /// </summary>
    /// <param name="orderDate">The date to search for orders.</param>
    /// <returns>Returns the order made on the specified date.</returns>
    /// <response code="200">If the order was found.</response>
    /// <response code="404">If no order was found on the given date.</response>
    /// <response code="500">If a server error occurs.</response>
    [HttpGet("getbydate")]
    public async Task<ActionResult<Order>> GetByDate([FromQuery] DateTime orderDate)
    {
        try
        {
            var response = await _unitOfWork.OrderRepository.FindByOrderDate(orderDate);

            if (response is null)
                return NotFound(
                    "Vi kunde inte hitta beställning med någon av de angivna parametrarna."
                );

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    /// Hitta specifik order via ordernr.
    /// </summary>
    /// <param name="orderNumber">The order number to search for.</param>
    /// <returns>Returns the matching order.</returns>
    /// <response code="200">If the order was found.</response>
    /// <response code="404">If no order matches the provided number.</response>
    /// <response code="500">If a server error occurs.</response>
    [HttpGet("getbyorder")]
    public async Task<ActionResult<Order>> GetByOrder(string orderNumber)
    {
        try
        {
            var response = await _unitOfWork.OrderRepository.FindByOrderNumber(orderNumber);

            if (response is null)
                return NotFound(
                    "Vi kunde inte hitta beställning med någon av de angivna parametrarna."
                );

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    /// Listar alla ordrar.
    /// </summary>
    /// <returns>A list of all orders in the system.</returns>
    /// <response code="200">If the list was successfully retrieved.</response>
    /// <response code="500">If a server error occurs.</response>
    [HttpGet()]
    public async Task<ActionResult<List<OrderViewModel>>> GetOrder()
    {
        return Ok(await _unitOfWork.OrderRepository.List());
    }
}
