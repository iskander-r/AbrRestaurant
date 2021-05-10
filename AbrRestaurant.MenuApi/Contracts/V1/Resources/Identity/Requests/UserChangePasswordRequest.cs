namespace AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity.Requests
{
    public class UserChangePasswordRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
