using Application.Features.Departments.Queries.GetAllDepartments;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace Blazor.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<GetDepartmentDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllDepartmentsQuery());
            if (!result.IsSuccess || result.Value == null)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Value);
    }
}
