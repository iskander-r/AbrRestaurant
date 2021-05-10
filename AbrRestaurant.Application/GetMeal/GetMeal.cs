using AbrRestaurant.Application.CreateMeal;
using AbrRestaurant.Application.Generics;
using AbrRestaurant.Application.Mappers;
using AbrRestaurant.Domain.Errors;
using AbrRestaurant.MenuApi.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AbrRestaurant.Application.GetMeal
{
    public class GetAllMealsQuery : IRequest<IEnumerable<CreateMealCommandResponse>>
    {
        // TODO: Refactor to universal pagination mechanism later, aka IQueryableExtensions
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class GetMealByIdQuery : IRequest<EitherResult<CreateMealCommandResponse, DomainEntityNotFoundError>>
    {
        public int Id { get; set; }
    }

    public class GetMealRequestHandler : 
        IRequestHandler<GetAllMealsQuery, IEnumerable<CreateMealCommandResponse>>,
        IRequestHandler<GetMealByIdQuery, EitherResult<CreateMealCommandResponse, DomainEntityNotFoundError>>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public GetMealRequestHandler(
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<CreateMealCommandResponse>> Handle(
            GetAllMealsQuery request, CancellationToken cancellationToken)
        {
            var query = _applicationDbContext.Meals.Where(p => !p.IsDeleted);

            var paginatedQuery = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return paginatedQuery.Select(p => p.CreateResponse());
        }

        public async Task<EitherResult<CreateMealCommandResponse, DomainEntityNotFoundError>> Handle(
            GetMealByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _applicationDbContext.Meals
                .SingleOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted);

            if (item != null)
                return item.CreateResponse();

            return new DomainEntityNotFoundError(
                    $"Блюдо с идентификатором {request.Id} не найдено в меню!");
        }
    }
}
