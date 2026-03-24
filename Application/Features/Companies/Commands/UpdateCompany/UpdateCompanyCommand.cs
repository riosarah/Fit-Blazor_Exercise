using MediatR;
using Shared.Dtos;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Companies.Commands.UpdateCompany
{
    public record UpdateCompanyCommand(int Id,
    string CompanyName,
    string Zip,
    string City,
    int DepartmentId): IRequest<Result<GetCompanyDto>>;
}
