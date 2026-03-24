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

namespace Application.Features.Companies.Commands.AddCompany
{
    public sealed class CreateCompanyCommandHandler(IUnitOfWork uow)
        : IRequestHandler<CreateCompanyCommand, Result<GetCompanyDto>>
    {
        public async Task<Result<GetCompanyDto>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = Domain.Entities.Company.Create(request.CompanyName, request.Zip, request.City, request.DepartmentId);
            await uow.Companies.AddAsync(company);
            await uow.SaveChangesAsync(cancellationToken);
            return Result<GetCompanyDto>.Created(company.Adapt<GetCompanyDto>());
        }
    }
}
