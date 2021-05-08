using Newtonsoft.Json;

namespace AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Requests
{
    public class CreateMenuItemRequest
    {
        [JsonIgnore]
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonProperty("picture_base64")]
        public string PictureAsBase64 { get; set; }
        public decimal Price { get; set; }
    }
}
