using MediatR;
using Shared.Dtos;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Companies.Queries.GetCompanyById
{
    public readonly record struct GetCompanyByIdQuery(int id): IRequest<Result<GetCompanyDto>>;
}
