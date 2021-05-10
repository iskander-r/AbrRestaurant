using System.ComponentModel.DataAnnotations;

namespace AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity.Requests
{
    public class UserSignUpRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
