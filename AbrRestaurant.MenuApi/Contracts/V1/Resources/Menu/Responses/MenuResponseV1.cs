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

        public MenuResponseV1(
            string id,
            string name, 
            string description,
            string pictureUri, 
            decimal price)
        {
            Id = id;
            Name = name;
            Description = description;
            PictureUri = pictureUri;
            Price = price;
        }
    }
}
