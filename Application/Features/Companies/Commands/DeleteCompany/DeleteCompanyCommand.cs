using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Companies.Commands.DeleteCompanyCommand
{
    public record DeleteCompanyCommand(int id): IRequest<Result>;
}
