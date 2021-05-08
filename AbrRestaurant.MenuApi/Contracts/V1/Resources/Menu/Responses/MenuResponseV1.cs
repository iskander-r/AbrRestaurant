using Newtonsoft.Json;

namespace AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Responses
{
    public class MenuResponseV1
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonProperty("picture_uri")]
        public string PictureUri { get; set; }
        public decimal Price { get; set; }
    }
}
