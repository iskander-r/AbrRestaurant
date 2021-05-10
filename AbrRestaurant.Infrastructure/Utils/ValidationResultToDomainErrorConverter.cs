using AbrRestaurant.Domain.Errors;
using FluentValidation.Results;

namespace AbrRestaurant.Infrastructure.Utils
{
    public static class ValidationResultToDomainErrorConverter
    {
        public static DomainError ToDomainError(this ValidationResult validationResult)
        {
            var errorObject = new DomainValidationFailedError();
            validationResult.Errors.ForEach(p => errorObject.AddErrorMessage(p.ErrorMessage));

            return errorObject;
        }
    }
}
