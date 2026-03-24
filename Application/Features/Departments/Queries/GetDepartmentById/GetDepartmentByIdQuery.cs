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
   public record struct GetDepartmentByIdQuery(int DepartmentId): IRequest<Result<GetDepartmentDto>>;
}
