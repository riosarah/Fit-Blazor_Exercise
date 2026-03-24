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
    public record struct GetAllDepartmentsQuery(): IRequest<Result<IReadOnlyCollection<GetDepartmentDto>>>;
}
