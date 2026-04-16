using Cortex.Mediator;
using HngClassify.Application.Features.Genderize.GetGenderByName;
using HngClassify.Application.Features.Genderize.Responses;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HngClassify.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClassifyController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Classify
    /// </summary>
    /// <remarks>
    /// This endpoint classifies a gender based on the provided name. It uses the Genderize API 
    /// to determine the gender associated with the name and returns a response indicating the 
    /// classification result. The response includes the name, predicted gender, and probability.
    /// </remarks>
    /// <param name="name">The name to classify.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// Returns an HTTP response that determines the status of the request.
    /// </returns>
    /// <response code="200">A <see cref="ClassifyResponse"/> representing the classification result.</response>
    /// <response code="400">when the request fails validation or processing.</response>
    /// <response code="422">name is not a string</response>
    /// <response code="500">Upstream or server failure</response>
    /// <response code="502">Upstream or server failure</response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ClassifyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ClassifyErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ClassifyErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ClassifyErrorResponse), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ClassifyErrorResponse), StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> GetGenderByName(
        [FromQuery]
        [Required]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Name must contain only letters.")]
        string name,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            var errorMessage = ModelState.Values
                .SelectMany(v => v.Errors)
                .FirstOrDefault()?.ErrorMessage ?? "Invalid input";

            return UnprocessableEntity(new ClassifyErrorResponse("error", errorMessage
            ));
        }

        var result = await mediator.SendQueryAsync(new GetGenderByNameQuery(name), cancellationToken);

        try
        {
            if (result.Succeeded)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Data);
        }
        catch (HttpRequestException)
        {
            return StatusCode(StatusCodes.Status502BadGateway, result.Data);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
        }
    }
}
