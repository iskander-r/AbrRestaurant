using AbrRestaurant.Application.Mappers;
using AbrRestaurant.Domain.Errors;
using AbrRestaurant.Infrastructure.Utils;
using AbrRestaurant.MenuApi.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AbrRestaurant.Application.CreateMeal
{
    public class CreateMealCommand : 
        IRequest<CreateMealCommandResponse>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureBase64 { get; set; }
        public decimal Price { get; set; }

        public CreateMealCommand(
            string name, 
            string description, 
            string pictureBase64, 
            decimal price)
        {
            Name = name;
            Description = description;
            PictureBase64 = pictureBase64;
            Price = price;
        }
    }


    public class CreateMealCommandResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureBase64 { get; set; }
        public decimal Price { get; set; }

        public CreateMealCommandResponse(
            string id, 
            string name,
            string description, 
            string pictureBase64,
            decimal price)
        {
            Id = id;
            Name = name;
            Description = description;
            PictureBase64 = pictureBase64;
            Price = price;
        }
    }

    public class CreateMealCommandValidator : AbstractValidator<CreateMealCommand>
    {
        private readonly AbrApplicationDbContext _applicationDbContext;
        public CreateMealCommandValidator(
            AbrApplicationDbContext applicationDbContext)
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

            RuleFor(p => p.PictureBase64)
                .Must(p => p.IsValidBase64String())
                    .WithMessage("Фотография блюда должна быть передана в виде валидной base64 строки");
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
        IRequestHandler<CreateMealCommand, CreateMealCommandResponse>
    {
        private readonly AbrApplicationDbContext _applicationDbContext;
        private readonly ILogger<CreateMealCommandHandler> _logger;

        public CreateMealCommandHandler(
            AbrApplicationDbContext applicationDbContext,
            ILogger<CreateMealCommandHandler> logger)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        public async Task<CreateMealCommandResponse> Handle(
            CreateMealCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateMealCommandValidator(_applicationDbContext);
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
                throw new BadRequestException(
                    validationResult.Errors
                        .Select(p => p.ErrorMessage).ToArray());

            var meal = request.MapToMeal();
            _applicationDbContext.Meals.Add(meal);

            await _applicationDbContext.SaveChangesAsync();

            string mealId = meal.Id.ToString();
            _logger.LogInformation("Создано новое блюдо в меню {mealId}", mealId);

            return meal.CreateResponse();
        }
    }
}
