namespace AbrRestaurant.Domain.Errors
{
    public class AuthenticationRequiredException : BaseException
    {
        public AuthenticationRequiredException(string message)
            : base(message)
        {
            AssociatedHttpStatusCode = 401;
        }

        public AuthenticationRequiredException() : this(
            "Для выполнения этого запроса требутся аутентификация в системе! " +
            "Пожалуйста, попробуйте повторно пройти процедуру входа в систему.")
        {

        }
    }
}
