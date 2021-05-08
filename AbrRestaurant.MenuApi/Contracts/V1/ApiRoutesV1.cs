namespace AbrRestaurant.MenuApi.Contracts.V1
{
    public static class ApiRoutesV1
    {
        private const string API_VERSION = "1";
        private const string PATH_ROOT = "api";
        public static class MenuItems
        {
            private const string CONTROLLER_PATH = "menu";
            public const string GetAll = PATH_ROOT + "/v" + API_VERSION + "/" + CONTROLLER_PATH; 
        }
    }
}
