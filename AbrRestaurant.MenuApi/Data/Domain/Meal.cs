using AbrRestaurant.MenuApi.Data.Trackable;
using System;

namespace AbrRestaurant.MenuApi.Data.Domain
{
    public class Meal : TrackableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte [] Picture { get; set; }
        public decimal Price { get; set; }
    }
}
