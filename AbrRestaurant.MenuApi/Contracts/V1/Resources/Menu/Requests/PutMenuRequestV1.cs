using System.ComponentModel.DataAnnotations;

namespace AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Requests
{
    public class PutMenuRequestV1 : PostMenuRequestV1
    {
        [Required]
        public int Id { get; set; }
    }
}
