using System;

namespace AbrRestaurant.MenuApi.Data.Trackable
{
    public abstract class TrackableEntity
    {
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set;  }
        public DateTime ? LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public byte [] RowVersion { get; set; }
    }
}
