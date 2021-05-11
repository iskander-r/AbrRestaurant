using System;
using System.Collections.Generic;

namespace AbrRestaurant.Domain.Errors
{
    public class BaseException : Exception
    {
        private readonly SortedSet<string> _errorDescriptions;
        public int AssociatedHttpStatusCode { get; protected set; } = 500;

        protected void AddErrorDescription(string description)
        {
            _errorDescriptions.Add(description);
        }
        public IEnumerable<string> ErrorDescriptions => _errorDescriptions;

        public BaseException()
        {
            _errorDescriptions = new SortedSet<string>();

            string defaultDescription = "Произошла непредвиденная ошибка во время обработки запроса. " +
                "Уведомление об ошибке уже отправлено команде разработчиков.";

            AddErrorDescription(defaultDescription);
        }
        public BaseException(string message)
        {
            _errorDescriptions = new SortedSet<string>();
            AddErrorDescription(message);
        }
    }
}
