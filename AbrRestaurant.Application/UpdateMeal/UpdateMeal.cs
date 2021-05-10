using AbrRestaurant.Application.CreateMeal;
using AbrRestaurant.Application.Generics;
using AbrRestaurant.Application.Mappers;
using AbrRestaurant.Domain.Entities;
using AbrRestaurant.Domain.Errors;
using AbrRestaurant.Infrastructure.Utils;
using AbrRestaurant.MenuApi.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AbrRestaurant.Application.UpdateMeal
{
    public class UpdateMealCommand : 
        IRequest<EitherResult<CreateMealCommandResponse, DomainError>>
    {
        public int Id { get; }
        public CreateMealCommand CreateMeal { get; }

        public UpdateMealCommand(
            int id,
            CreateMealCommand createMeal)
        {
            Id = id;
            CreateMeal = createMeal;
        }
    }

    public class UpdateMealCommandValidator : AbstractValidator<UpdateMealCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public UpdateMealCommandValidator(
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

            RuleFor(p => p.CreateMeal.Name)
                .NotEmpty()
                    .WithMessage("Необходимо указать название блюда")
                .MaximumLength(256)
                    .WithMessage("Название блюда не может быть длинее 256 символов");

            RuleFor(p => p.CreateMeal.Price)
                .Must(MealPriceMustBeInSpecifiedRange)
                    .WithMessage("Цена на блюдо должна быть указана в допустимых пределах");
        }

        // TODO: Refactor to seperate Rule class / Factory later
        private bool MealPriceMustBeInSpecifiedRange(decimal price)
        {
            decimal mealMinPrice = 1, mealMaxPrice = 100_000;
            return price >= mealMinPrice && price <= mealMaxPrice;
        }
    }

    public class UpdateMealCommandHandler :
        IRequestHandler<UpdateMealCommand, EitherResult<CreateMealCommandResponse, DomainError>>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public UpdateMealCommandHandler(
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<EitherResult<CreateMealCommandResponse, DomainError>> Handle(
            UpdateMealCommand request, CancellationToken cancellationToken)
        {
            var mealToUpdate = await _applicationDbContext.Meals
                .SingleOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted);

            if (mealToUpdate != null)
                return new DomainEntityNotFoundError(
                    $"Блюдо с идентификатором {request.Id} не найдено в меню!");

            var validator = new UpdateMealCommandValidator(_applicationDbContext);
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
                return validationResult.ToDomainError();

            Patch(mealToUpdate, request.CreateMeal);
            _applicationDbContext.Meals.Update(mealToUpdate);

            await _applicationDbContext.SaveChangesAsync();
            return mealToUpdate.CreateResponse();
        }


        private void Patch(Meal mealToUpdate, CreateMealCommand updatedModel)
        {
            mealToUpdate.Name = updatedModel.Name;
            mealToUpdate.Description = updatedModel.Name;
            mealToUpdate.PictureContent = updatedModel.PictureAsBase64?.ToByteArray() ?? null,
            mealToUpdate.Price = updatedModel.Price;
        }
    }
}
