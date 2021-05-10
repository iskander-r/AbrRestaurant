using Microsoft.AspNetCore.Http;
using System;

namespace AbrRestaurant.Infrastructure.Services
{
    public class CurrentUserIdentity
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public int IssuedMoment { get; set; }
        public bool IsAnonymousUser => Id == Guid.Empty;

        public static CurrentUserIdentity AnonymousUser => 
            new CurrentUserIdentity() { Id = Guid.Empty, Email = string.Empty };
    }

    public interface ICurrentApplicationUserProvider
    {
        CurrentUserIdentity GetCurrentUser();
    }

    public class CurrentApplicationUserProvider : ICurrentApplicationUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentApplicationUserProvider(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CurrentUserIdentity GetCurrentUser()
        {
            if (_httpContextAccessor.HttpContext == null)
                return CurrentUserIdentity.AnonymousUser;

            var canDetermineUser = 
                _httpContextAccessor.HttpContext.User.HasClaim(p => p.Type == "id") &&
                _httpContextAccessor.HttpContext.User.HasClaim(p => p.Type == "email");

            if (!canDetermineUser)
                return CurrentUserIdentity.AnonymousUser;

            return new CurrentUserIdentity()
            {
                Id = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst("id").Value),
                Email = _httpContextAccessor.HttpContext.User.FindFirst("email").Value,
                IssuedMoment = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("issued_moment").Value),
            };
        }
    }
}
