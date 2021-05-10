using AbrRestaurant.Application.CreateMeal;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Requests;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Responses;

namespace AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Mappers
{
    public static class MenuResourceMappers
    {
        public static CreateMealCommand ToApplicationCommand(this PostMenuRequestV1 input)
        {
            return new CreateMealCommand(
                input.Name, input.Description, input.PictureAsBase64, input.Price);
        }

        public static MenuResponseV1 ToOuterContractModel(this CreateMealCommandResponse output)
        {
            return new MenuResponseV1(
                output.Id, output.Name, output.Description, output.PictureBase64, output.Price);
        }
    }
}
