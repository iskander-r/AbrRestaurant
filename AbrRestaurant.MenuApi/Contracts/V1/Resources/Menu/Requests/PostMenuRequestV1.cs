using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Requests
{
    public class PostMenuRequestV1
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [JsonProperty("picture_base_64")]
        public string PictureAsBase64 { get; set; }

        [Required]
        [Range(1, 100_000)]
        public decimal Price { get; set; }
    }
}
