using Application.Features.Authors.Queries.GetAllAuthors;
using Application.Features.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAuthorDto>>> GetAllAuthors()
    {
        var result = await mediator.Send(new GetAllAuthorsQuery());
        
        if (!result.IsSuccess || result.Value == null)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Value);
    }
}
