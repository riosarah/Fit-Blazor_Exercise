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

namespace Application.Features.Departments.Queries.GetAllDepartments
{
    public partial class GetAllDepartmentsQueryHandler(IUnitOfWork uow) : 
        IRequestHandler<GetAllDepartmentsQuery, Result<IReadOnlyCollection<GetDepartmentDto>>>
    {
        public async Task<Result<IReadOnlyCollection<GetDepartmentDto>>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
        {
            var deps = await uow.Departments.GetAllAsync();
            var models = deps.Adapt<IReadOnlyCollection<GetDepartmentDto>>();
            return Result<IReadOnlyCollection<GetDepartmentDto>>.Success(models);
        }
    }
}
