using Application.Contracts;
using Mapster;
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
    public sealed class GetCompanyByIdQueryHandler(IUnitOfWork uow)
        : IRequestHandler<GetCompanyByIdQuery, Result<GetCompanyDto>>
    {
        public async Task<Result<GetCompanyDto>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            var company = await uow.Companies.GetByIdAsync(request.id, cancellationToken);
            if (company == null)
            {
                return Result<GetCompanyDto>.NotFound($"Company with id {request.id} not found.");
            }

             return Result<GetCompanyDto>.Success(company.Adapt<GetCompanyDto>());
        }
    }
}
