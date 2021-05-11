namespace AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity
{
    public class IdentityResourceRoutesV1
    {
        private const string API_VERSION = "1";
        private const string PATH_ROOT = "api";
        public static class IdentityResource
        {
            private const string CONTROLLER_PATH = "identity";
            public const string GetProfile = PATH_ROOT + "/v" + API_VERSION + "/" + CONTROLLER_PATH + "/" + "get_profile";
            public const string SignUp = PATH_ROOT + "/v" + API_VERSION + "/" + CONTROLLER_PATH + "/" + "sign_up";
            public const string SignIn = PATH_ROOT + "/v" + API_VERSION + "/" + CONTROLLER_PATH + "/" + "sign_in";
            public const string SignOut = PATH_ROOT + "/v" + API_VERSION + "/" + CONTROLLER_PATH + "/" + "sign_out";
            public const string ChangePassword = PATH_ROOT + "/v" + API_VERSION + "/" + CONTROLLER_PATH + "/" + "change_password";
        }
    }
}
