namespace AbrRestaurant.Application.Generics
{
    public class EitherResult<TResponse, TError> 
        where TError : class 
        where TResponse : class
    {
        public TResponse Response { get; private set; }
        public TError Error { get; private set; }

        public bool CompletedWithError => Error != null;
        public bool CompletedSuccessfully => Response != null; 

        public EitherResult(TResponse response)
        {
            Response = response;
            Error = null;
        }

        public EitherResult(TError error)
        {
            Error = error;
            Response = null;
        }

        public static implicit operator EitherResult<TResponse, TError>(TResponse response) 
            => new EitherResult<TResponse, TError>(response);

        public static implicit operator EitherResult<TResponse, TError>(TError error)
            => new EitherResult<TResponse, TError>(error);
    }
}
