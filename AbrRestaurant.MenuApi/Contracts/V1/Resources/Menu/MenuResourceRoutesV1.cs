namespace AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu
{
    public static class MenuResourceRoutesV1
    {
        private const string API_VERSION = "1";
        private const string PATH_ROOT = "api";
        public static class MenuResource
        {
            private const string CONTROLLER_PATH = "menu";
            public const string GetAll = PATH_ROOT + "/v" + API_VERSION + "/" + CONTROLLER_PATH; 
            public const string Get = PATH_ROOT + "/v" + API_VERSION + "/" + CONTROLLER_PATH + "/" + "{id}";
            public const string Put = PATH_ROOT + "/v" + API_VERSION + "/" + CONTROLLER_PATH + "/" + "{id}";
            public const string Post = PATH_ROOT + "/v" + API_VERSION + "/" + CONTROLLER_PATH;
        }
    }
}
