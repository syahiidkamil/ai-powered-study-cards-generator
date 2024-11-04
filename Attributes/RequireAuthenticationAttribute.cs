using Microsoft.AspNetCore.Authorization;

namespace StudyCardsGenerator.Attributes
{
    public class RequireAuthenticationAttribute : AuthorizeAttribute
    {
        public RequireAuthenticationAttribute()
        {
        }
    }
}