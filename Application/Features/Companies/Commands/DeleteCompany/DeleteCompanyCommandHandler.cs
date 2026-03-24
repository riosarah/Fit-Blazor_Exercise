using Application.Contracts;
using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Companies.Commands.DeleteCompanyCommand
{
    public sealed class DeleteCompanyCommandHandler(IUnitOfWork uow) :
        IRequestHandler<DeleteCompanyCommand, Result>
    {
        public Task<Result> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = uow.Companies.GetByIdAsync(request.id, cancellationToken).Result;
            if (company is null)
            {
                return Task.FromResult(Result.NotFound($"Company with id {request.id} not found."));
            }
            uow.Companies.Remove(company);
            uow.SaveChangesAsync(cancellationToken).Wait();
            return Task.FromResult(Result.Success($"Company with id {request.id} deleted successfully."));
        }
    }
}
