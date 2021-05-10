﻿using AbrRestaurant.Application.CreateMeal;
using AbrRestaurant.Domain.Entities;
using AbrRestaurant.Infrastructure.Utils;

namespace AbrRestaurant.Application.Mappers
{
    public static class DomainToApplicationLayerMappers
    {
        public static CreateMealCommandResponse CreateResponse(this Meal meal)
        {
            return new CreateMealCommandResponse(
                meal.Id.ToString(), meal.Name, meal.Description, string.Empty, meal.Price);
        }
    }


    public static class ApplicationToDomainLayerMappers
    {
        public static Meal MapToMeal(this CreateMealCommand request)
        {
            return new Meal()
            {
                Name = request.Name,
                Description = GetDescriptionOrDefault(request.Description),
                PictureContent = request.PictureAsBase64?.ToByteArray() ?? null,
                Price = request.Price
            };
        }

        private static string GetDescriptionOrDefault(string description) =>
            string.IsNullOrWhiteSpace(description) ? "Описание для блюда еще не добавлено" : description;
    }
}
