using AbrRestaurant.MenuApi.Data.Trackable;

namespace AbrRestaurant.MenuApi.Data.Domain
{
    public class Meal : TrackableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureAsBase64 { get; set; }
        public decimal Price { get; set; }
    }
}
