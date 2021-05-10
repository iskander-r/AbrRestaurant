using Newtonsoft.Json;

namespace AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Requests
{
    public class CreateMenuRequestV1
    {
        [JsonRequired]
        public string Name { get; set; }

        [JsonRequired]
        public string Description { get; set; }

        [JsonProperty("picture_base64")]
        public string PictureAsBase64 { get; set; }

        [JsonProperty]
        public decimal Price { get; set; }
    }
}
