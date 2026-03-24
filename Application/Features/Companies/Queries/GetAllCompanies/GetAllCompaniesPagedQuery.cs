using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Shared.Dtos;
using Shared.Results;

namespace Application.Features.Companies.Queries.GetAllCompanies
{
    public record GetAllCompaniesPagedQuery(int DepartmentId, int Page, int PageSize) : IRequest<PagedResult<GetCompanyDto>>;
}
