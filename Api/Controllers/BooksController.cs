using Application.Common.Results;
using Application.Features.Books.Commands.CreateBook;
using Application.Features.Books.Commands.DeleteBook;
using Application.Features.Books.Commands.UpdateBook;
using Application.Features.Books.Queries.GetBookById;
using Application.Features.Books.Queries.GetBooksByAuthorIdPaged;
using Application.Features.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(IMediator mediator) : ControllerBase
{
    [HttpGet("by-author/{authorId}")]
    public async Task<ActionResult<PagedResult<GetBookDto>>> GetBooksByAuthor(
        int authorId,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 3)
    {
        var result = await mediator.Send(new GetBooksByAuthorIdPagedQuery(authorId, page, pageSize));
        
        if (!result.IsSuccess || result.Value == null)
        {
            return BadRequest(result.Message);
        }

        return Ok(PagedResult<GetBookDto>.Success(result.Value.Items, result.Value.TotalCount, page, pageSize));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetBookDto>> GetBookById(int id)
    {
        var result = await mediator.Send(new GetBookByIdQuery(id));
        
        if (!result.IsSuccess || result.Value == null)
        {
            return NotFound(result.Message);
        }

        return Ok(result.Value);
    }

}
