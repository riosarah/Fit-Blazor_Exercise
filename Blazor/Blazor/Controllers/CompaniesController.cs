using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Shared.Results;

namespace Blazor.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController(IMediator mediator) : ControllerBase
{
    [HttpGet("by-department/{departmentId}")]
    public async Task<ActionResult<object>> GetCompaniesByDepartment(
        [FromRoute] int departmentId,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Diese Methode muss noch implementiert werden.");
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetCompanyDto>> GetById(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Diese Methode muss noch implementiert werden.");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<GetCompanyDto>> Update(
        int id,
        [FromBody] UpdateCompanyRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Diese Methode muss noch implementiert werden.");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Diese Methode muss noch implementiert werden.");
    }
}

