namespace AbrRestaurant.Domain.Errors
{
    public class ResourceNotFoundException : BaseException
    {
        public ResourceNotFoundException(string message) 
            : base(message)
        {
            AssociatedHttpStatusCode = 404;
        }

        public ResourceNotFoundException(params string [] messages) 
        {
            AssociatedHttpStatusCode = 404;
            foreach (var item in messages)
            {
                AddErrorDescription(item);
            }
        }

        public static ResourceNotFoundException GetFromTemplate(
            string recourseName, string resourceId)
        {
            var message = $"Ресурс типа '{recourseName}' с уникальным идентификатором '{resourceId}' не найден!" +
                " Пожалуйста, повторите запрос позднее или обратитесь в службу технической поддержки!";

            return new ResourceNotFoundException(message);
        }
    }
}
