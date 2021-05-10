using System.Collections.Generic;

namespace AbrRestaurant.Infrastructure.Models
{
    public class ChangePasswordResult
    {
        public bool IsPasswordChanged { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public static ChangePasswordResult GetSuccededPasswordChange()
        {
            return new ChangePasswordResult() { IsPasswordChanged = true };
        }

        public static ChangePasswordResult GetFailedPasswordChange(params string[] errors)
        {
            return new ChangePasswordResult() { Errors = errors, IsPasswordChanged = false };
        }
    }
}
