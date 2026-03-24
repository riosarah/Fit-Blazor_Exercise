using MediatR;
using Shared.Dtos;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Companies.Commands.AddCompany
{
    public record CreateCompanyCommand(
        string CompanyName,
        string Zip,
        string City,
        int DepartmentId) : IRequest<Result<GetCompanyDto>>;
}
