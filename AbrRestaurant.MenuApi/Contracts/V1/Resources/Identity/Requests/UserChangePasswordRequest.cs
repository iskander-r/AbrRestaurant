using System.ComponentModel.DataAnnotations;

namespace AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity.Requests
{
    public class UserChangePasswordRequest
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
