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

namespace Application.Features.Companies.Commands.UpdateCompany
{
    public sealed class UpdateCompanyCommandHandler(IUnitOfWork uow)
        : IRequestHandler<UpdateCompanyCommand, Result<GetCompanyDto>>
    {
        public async Task<Result<GetCompanyDto>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = uow.Companies.GetByIdAsync(request.Id, cancellationToken).Result;
           
            if (company != null)
            {
                company?.Update(request.CompanyName, request.Zip, request.City, request.DepartmentId);
                uow.Companies.Update(company);
                await uow.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return Result<GetCompanyDto>.NotFound($"Company with ID {request.Id} not found.");
            }
                var companyDto = company.Adapt<GetCompanyDto>();

            return Result<GetCompanyDto>.Success(companyDto, "Company updated successfully.");
        }
    }
}
