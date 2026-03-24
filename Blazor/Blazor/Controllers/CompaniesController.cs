using Application.Features.Companies.Commands.DeleteCompanyCommand;
using Application.Features.Companies.Commands.UpdateCompany;
using Application.Features.Companies.Queries.GetAllCompanies;
using Application.Features.Companies.Queries.GetCompanyById;
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
        var result = await mediator.Send(new GetAllCompaniesPagedQuery(departmentId, page, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetCompanyDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCompanyByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<GetCompanyDto>> Update(
        int id,
        [FromBody] UpdateCompanyRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateCompanyCommand(id, request.CompanyName, request.Zip, request.City, request.DepartmentId), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Result>> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteCompanyCommand(id), cancellationToken);
        return Ok(result);
    }
}

