using AbrRestaurant.Application.Generics;
using AbrRestaurant.Application.Mappers;
using AbrRestaurant.Domain.Errors;
using AbrRestaurant.Infrastructure.Utils;
using AbrRestaurant.MenuApi.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AbrRestaurant.Application.CreateMeal
{
    /// <summary>
    /// Command to create a meal in them menu.
    /// </summary>
    public class CreateMealCommand : 
        IRequest<EitherResult<CreateMealCommandResponse, DomainError>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureAsBase64 { get; set; }
        public decimal Price { get; set; }

        public CreateMealCommand(
            string name, 
            string description, 
            string pictureAsBase64, 
            decimal price)
        {
            Name = name;
            Description = description;
            PictureAsBase64 = pictureAsBase64;
            Price = price;
        }
    }


    public class CreateMealCommandResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUri { get; set; }
        public decimal Price { get; set; }

        public CreateMealCommandResponse(
            string id, 
            string name,
            string description, 
            string pictureUri,
            decimal price)
        {
            Id = id;
            Name = name;
            Description = description;
            PictureUri = pictureUri;
            Price = price;
        }
    }

    public class CreateMealCommandValidator : AbstractValidator<CreateMealCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CreateMealCommandValidator(
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

            RuleFor(p => p.Name)
                .NotEmpty()
                    .WithMessage("Необходимо указать название блюда")
                .MaximumLength(256)
                    .WithMessage("Название блюда не может быть длинее 256 символов")
                .MustAsync(MealMustHaveUniqueName)
                    .WithMessage("В меню уже имеется блюдо с таким же названием");

            RuleFor(p => p.Price)
                .Must(MealPriceMustBeInSpecifiedRange)
                    .WithMessage("Цена на блюдо должна быть указана в допустимых пределах");
        }

        private async Task<bool> MealMustHaveUniqueName(
            string name, CancellationToken ct)
        {
            // TODO: To find correct way to compare strings in case-insensitive manner in PostgreSQL provider.
            var allNamesUnique = await _applicationDbContext
                .Meals.AllAsync(p => p.Name != name);

            return allNamesUnique;
        }

        // TODO: Refactor to seperate Rule class / Factory later
        private bool MealPriceMustBeInSpecifiedRange(decimal price)
        {
            decimal mealMinPrice = 1, mealMaxPrice = 100_000;
            return price >= mealMinPrice && price <= mealMaxPrice;
        }  
    }


    public class CreateMealCommandHandler :
        IRequestHandler<CreateMealCommand, EitherResult<CreateMealCommandResponse, DomainError>>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CreateMealCommandHandler(
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<EitherResult<CreateMealCommandResponse, DomainError>> Handle(
            CreateMealCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateMealCommandValidator(_applicationDbContext);
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
                return validationResult.ToDomainError();

            var meal = request.MapToMeal();
            _applicationDbContext.Meals.Add(meal);

            await _applicationDbContext.SaveChangesAsync();
            return meal.CreateResponse();
        }
    }
}
