using AbrRestaurant.Application.CreateMeal;
using AbrRestaurant.Application.DeleteMeal;
using AbrRestaurant.Application.GetMeal;
using AbrRestaurant.Application.UpdateMeal;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Requests;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Responses;

namespace AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Mappers
{
    // TODO: Replace with Automapper or Mapster
    public static class MenuResourceMappers
    {
        public static CreateMealCommand ToApplicationCommand(this PostMenuRequestV1 input) =>
            new CreateMealCommand(
                input.Name, input.Description, input.PictureAsBase64, input.Price);

        public static GetMealByIdQuery ToApplicationCommand(this GetMenuByIdRequestV1 input) =>
            new GetMealByIdQuery() { Id = input.Id };

        public static GetAllMealsQuery ToApplicationCommand(this GetAllMenuRequestV1 input) =>
            new GetAllMealsQuery() { PageIndex = input.PageIndex, PageSize = input.PageSize };

        public static DeleteMealCommand ToApplicationCommand(this DeleteMenuByIdRequestV1 input) =>
            new DeleteMealCommand(input.Id);

        public static UpdateMealCommand ToApplicationCommand(this PutMenuRequestV1 input, int id) =>
            new UpdateMealCommand(id, ToApplicationCommand((PostMenuRequestV1) input));

        public static MenuResponseV1 ToOuterContractModel(this CreateMealCommandResponse output) =>
            new MenuResponseV1(
                output.Id, output.Name, output.Description, output.PictureBase64, output.Price);
    }
}
