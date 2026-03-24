using Application.Contracts;
using MediatR;
using Shared.Dtos;
using Shared.Results;

namespace Application.Features.Companies.Queries.GetAllCompanies
{
    public partial class GetAllCompaniesPagedQueryHandler(IUnitOfWork uow) :
        IRequestHandler<GetAllCompaniesPagedQuery, PagedResult<GetCompanyDto>>
    {
        public async Task<PagedResult<GetCompanyDto>> Handle(GetAllCompaniesPagedQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page < 0 ? 0 : request.Page;
            var pageSize = request.PageSize < 1 ? 10 : request.PageSize;
            var skip = page * pageSize;

            var total = await uow.Companies.CountByDepartmentIdAsync(request.DepartmentId, cancellationToken);
            var com =  await uow.Companies.GetPagedByDepartmentIdAsync(request.DepartmentId,skip, pageSize, cancellationToken);

            return PagedResult<GetCompanyDto>.Success(com.ToList(), total, page, pageSize);
        
        }
    }
}
