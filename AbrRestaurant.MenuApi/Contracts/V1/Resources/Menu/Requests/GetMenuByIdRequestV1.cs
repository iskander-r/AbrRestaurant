using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Requests
{
    public class GetMenuByIdRequestV1
    {
        [Required]
        public string Id { get; set; }
    }
}
