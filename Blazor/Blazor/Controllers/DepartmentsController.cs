using MediatR;
using Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Blazor.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<GetDepartmentDto>>> GetAll(CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Diese Methode muss noch implementiert werden.");
    }
}
