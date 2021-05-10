namespace AbrRestaurant.Domain.Entities
{
    public class Meal : TrackableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte [] PictureContent { get; set; }
        public decimal Price { get; set; }
    }
}
