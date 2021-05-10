namespace AbrRestaurant.Domain.Errors
{
    public class DomainEntityNotFoundError : DomainError
    {
        public DomainEntityNotFoundError(string message)
        {
            AddErrorMessage(message);
        }
        public DomainEntityNotFoundError()
        {

        }
    }
}
