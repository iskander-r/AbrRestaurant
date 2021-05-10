using System.Collections.Generic;

namespace AbrRestaurant.Domain.Errors
{
    public abstract class DomainError
    {
        private ICollection<string> Descriptions { get; set; }
        public void AddErrorMessage(string message) => Descriptions.Add(message);
    }
}
