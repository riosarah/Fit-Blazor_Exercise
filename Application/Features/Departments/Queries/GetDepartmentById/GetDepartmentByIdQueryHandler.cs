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

namespace Application.Features.Departments.Queries.GetDepartmentById
{
    public sealed class GetDepartmentByIdQueryHandler(IUnitOfWork uow) :
        IRequestHandler<GetDepartmentByIdQuery, Result<GetDepartmentDto>>
    {
        public async Task<Result<GetDepartmentDto>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var department = await uow.Departments.GetByIdAsync(request.DepartmentId, cancellationToken);
            if(department is null)
            {
                return Result<GetDepartmentDto>.NotFound($"Department with id {request.DepartmentId} not found.");
            }
            return Result<GetDepartmentDto>.Success(department.Adapt<GetDepartmentDto>());
        }
    }
}
