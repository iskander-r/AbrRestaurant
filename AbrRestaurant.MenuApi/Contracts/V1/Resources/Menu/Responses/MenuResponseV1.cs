using Newtonsoft.Json;

namespace AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Responses
{
    public class MenuResponseV1
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonProperty("picture_base_64")]
        public string PictureBase64 { get; set; }
        public decimal Price { get; set; }

        public MenuResponseV1(
            string id,
            string name, 
            string description,
            string pictureBase64, 
            decimal price)
        {
            Id = id;
            Name = name;
            Description = description;
            PictureBase64 = pictureBase64;
            Price = price;
        }
    }
}
