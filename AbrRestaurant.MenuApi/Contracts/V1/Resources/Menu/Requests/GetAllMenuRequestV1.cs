using System.ComponentModel.DataAnnotations;

namespace AbrRestaurant.MenuApi.Contracts.V1.Resources.Menu.Requests
{
    public class GetAllMenuRequestV1
    {
        [Required]
        public int PageIndex { get; set; } = PaginationConstants.DEFAULT_PAGE_INDEX;

        [Required]
        public int PageSize { get; set; } = PaginationConstants.DEFAULT_PAGE_SIZE;
    }
}
