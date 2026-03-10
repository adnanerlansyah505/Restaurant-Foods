using Microsoft.AspNetCore.Mvc;
using System.Net;
using RestaurantFoods.Utilities.Handlers;

namespace RestaurantFoods.Controllers;

[ApiController]
public class BaseApiController : ControllerBase
{
    protected IActionResult SuccessResponse<T>(
        T data,
        string message = "Success",
        object? meta = null
    )
    {
        return Ok(new ResponseHandlers<T>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = message,
            Data = data,
            Meta = meta
        });
    }

    protected IActionResult CreatedResponse<T>(
        T data,
        string message = "Created successfully"
    )
    {
        return StatusCode(StatusCodes.Status201Created,
            new ResponseHandlers<T>
            {
                Code = StatusCodes.Status201Created,
                Status = HttpStatusCode.Created.ToString(),
                Message = message,
                Data = data
            });
    }

    protected IActionResult NotFoundResponse(
        string message = "Resource not found"
    )
    {
        return NotFound(new ResponseHandlers<object?>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = message
        });
    }

    protected IActionResult BadRequestResponse(
        string message = "Resource not found"
    )
    {
        return BadRequest(new ResponseHandlers<object?>
        {
            Code = StatusCodes.Status400BadRequest,
            Status = HttpStatusCode.BadRequest.ToString(),
            Message = message
        });
    }

    protected IActionResult NoContentResponse(
        string message = ""
    )
    {
        return Ok(new ResponseHandlers<object?>
        {
            Code = StatusCodes.Status204NoContent,
            Status = HttpStatusCode.NoContent.ToString(),
            Message = message
        });
    }
}