using System.Collections.Generic;

namespace AbrRestaurant.Domain.Errors
{
    public abstract class DomainError
    {
        private ICollection<string> _descriptions;
        public IEnumerable<string> Errors => _descriptions;
        public void AddErrorMessage(string message) => _descriptions.Add(message);

        public DomainError()
        {
            _descriptions = new List<string>();
        }
    }
}
