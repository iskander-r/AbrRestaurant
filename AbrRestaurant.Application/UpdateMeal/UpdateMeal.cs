using AbrRestaurant.Application.CreateMeal;
using AbrRestaurant.Application.Mappers;
using AbrRestaurant.Domain.Entities;
using AbrRestaurant.Domain.Errors;
using AbrRestaurant.Infrastructure.Utils;
using AbrRestaurant.MenuApi.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AbrRestaurant.Application.UpdateMeal
{
    public class UpdateMealCommand : 
        IRequest<CreateMealCommandResponse>
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
        public UpdateMealCommandValidator()
        {
            RuleFor(p => p.CreateMeal.Name)
                .NotEmpty()
                    .WithMessage("Необходимо указать название блюда")
                .MaximumLength(256)
                    .WithMessage("Название блюда не может быть длинее 256 символов");

            RuleFor(p => p.CreateMeal.Price)
                .Must(MealPriceMustBeInSpecifiedRange)
                    .WithMessage("Цена на блюдо должна быть указана в допустимых пределах");

            RuleFor(p => p.CreateMeal.PictureBase64)
                .Must(p => p.IsValidBase64String())
                    .WithMessage("Фотография блюда должна быть передана в виде валидной base64 строки");
        }

        // TODO: Refactor to seperate Rule class / Factory later
        private bool MealPriceMustBeInSpecifiedRange(decimal price)
        {
            decimal mealMinPrice = 1, mealMaxPrice = 100_000;
            return price >= mealMinPrice && price <= mealMaxPrice;
        }
    }

    public class UpdateMealCommandHandler :
        IRequestHandler<UpdateMealCommand, CreateMealCommandResponse>
    {
        private readonly AbrApplicationDbContext _applicationDbContext;
        private readonly ILogger<UpdateMealCommandHandler> _logger;

        public UpdateMealCommandHandler(
            AbrApplicationDbContext applicationDbContext,
            ILogger<UpdateMealCommandHandler> logger)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        public async Task<CreateMealCommandResponse> Handle(
            UpdateMealCommand request, CancellationToken cancellationToken)
        {
            var mealToUpdate = await _applicationDbContext.Meals
                .SingleOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted);

            if (mealToUpdate != null)
                throw new ResourceNotFoundException(
                    $"Блюдо с идентификатором {request.Id} не найдено в меню!");

            var validator = new UpdateMealCommandValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
                throw new BadRequestException(
                    validationResult.Errors
                        .Select(p => p.ErrorMessage).ToArray());

            Patch(mealToUpdate, request.CreateMeal);
            _applicationDbContext.Meals.Update(mealToUpdate);

            await _applicationDbContext.SaveChangesAsync();

            var mealId = mealToUpdate.Id.ToString();
            _logger.LogInformation("В блюдо {mealId} внесли изменения", mealId);

            return mealToUpdate.CreateResponse();
        }


        private void Patch(Meal mealToUpdate, CreateMealCommand updatedModel)
        {
            mealToUpdate.Name = updatedModel.Name;
            mealToUpdate.Description = updatedModel.Name;
            mealToUpdate.PictureContent = Convert.FromBase64String(updatedModel.PictureBase64);
            mealToUpdate.Price = updatedModel.Price;
        }
    }
}
