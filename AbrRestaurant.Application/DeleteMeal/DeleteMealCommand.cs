using AbrRestaurant.Application.Generics;
using AbrRestaurant.Domain.Errors;
using AbrRestaurant.MenuApi.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AbrRestaurant.Application.DeleteMeal
{
    public class DeleteMealCommand : 
        IRequest<EitherResult<EmptyResponse, DomainError>>
    {
        public int Id { get; }
        public DeleteMealCommand(int id)
        {
            Id = id;
        }
    }


    public class DeleteMealCommandHandler :
        IRequestHandler<DeleteMealCommand, EitherResult<EmptyResponse, DomainError>>
    {
        private readonly AbrApplicationDbContext _applicationDbContext;
        public DeleteMealCommandHandler(
            AbrApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<EitherResult<EmptyResponse, DomainError>> Handle(
            DeleteMealCommand request, CancellationToken cancellationToken)
        {
            var mealToDelete = await _applicationDbContext.Meals
                .SingleOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted);

            if (mealToDelete == null)
                return new DomainEntityNotFoundError(
                    $"Блюдо с идентификатором {request.Id} не найдено в меню!");

            mealToDelete.IsDeleted = true;
            await _applicationDbContext.SaveChangesAsync();

            return new EmptyResponse();
        }
    }
}
