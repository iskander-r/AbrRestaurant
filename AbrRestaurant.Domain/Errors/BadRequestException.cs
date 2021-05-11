namespace AbrRestaurant.Domain.Errors
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message)
               : base(message)
        {
            AssociatedHttpStatusCode = 400;
        }

        public BadRequestException(params string[] messages)
        {
            AssociatedHttpStatusCode = 400;
            foreach (var item in messages)
            {
                AddErrorDescription(item);
            }
        }
    }
}
