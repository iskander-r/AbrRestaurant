using System;

namespace AbrRestaurant.Domain.Entities
{
    public abstract class TrackableEntity
    {
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set;  }
        public DateTime ? LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
