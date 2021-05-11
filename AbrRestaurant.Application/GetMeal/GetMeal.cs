using AbrRestaurant.Application.CreateMeal;
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
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class GetMealByIdQuery : IRequest<CreateMealCommandResponse>
    {
        public int Id { get; set; }
    }

    public class GetMealRequestHandler : 
        IRequestHandler<GetAllMealsQuery, IEnumerable<CreateMealCommandResponse>>,
        IRequestHandler<GetMealByIdQuery, CreateMealCommandResponse>
    {
        private readonly AbrApplicationDbContext _applicationDbContext;
        public GetMealRequestHandler(
            AbrApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<CreateMealCommandResponse>> Handle(
            GetAllMealsQuery request, CancellationToken cancellationToken)
        {
            var query = _applicationDbContext.Meals.Where(p => !p.IsDeleted);

            var paginatedQuery = await query
                .OrderBy(p => p.Name)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return paginatedQuery
                .Select(p => p.CreateResponse());
        }


        public async Task<CreateMealCommandResponse> Handle(
            GetMealByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _applicationDbContext.Meals
                .SingleOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted);

            if (item != null)
                return item.CreateResponse();

            throw new ResourceNotFoundException(
                    $"Блюдо с идентификатором {request.Id} не найдено в меню!");
        }
    }
}
