using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Requests
{
    public class GetAllMenuRequestV1
    {
        [Required]
        [FromQuery]
        [JsonProperty("page_index")]
        public int PageIndex { get; set; } = PaginationConstants.DEFAULT_PAGE_INDEX;

        [Required]
        [FromQuery]
        [JsonProperty("page_size")]
        public int PageSize { get; set; } = PaginationConstants.DEFAULT_PAGE_SIZE;
    }
}
