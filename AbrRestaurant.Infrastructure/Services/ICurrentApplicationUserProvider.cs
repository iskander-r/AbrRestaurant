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
        public string TraceId { get; set; }

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
            var currentUser = default(CurrentUserIdentity);

            if (_httpContextAccessor.HttpContext == null)
            {
                currentUser = CurrentUserIdentity.AnonymousUser;
                return currentUser;
            }
                
            var canDetermineUser = 
                _httpContextAccessor.HttpContext.User.HasClaim(p => p.Type == "id") &&
                _httpContextAccessor.HttpContext.User.HasClaim(p => p.Type == "email");

            if (!canDetermineUser)
            {
                currentUser = CurrentUserIdentity.AnonymousUser;
            }
            else
            {
                currentUser = new CurrentUserIdentity()
                {
                    Id = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst("id").Value),
                    Email = _httpContextAccessor.HttpContext.User.FindFirst("email").Value,
                    IssuedMoment = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("issued_moment").Value),
                };
            }

            currentUser.TraceId = GetTraceId(_httpContextAccessor);
            return currentUser;
        }

        private string GetTraceId(IHttpContextAccessor httpContextAccessor) => 
            httpContextAccessor?.HttpContext.TraceIdentifier ?? string.Empty;
    }
}
