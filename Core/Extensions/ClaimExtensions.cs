using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Core.Extensions
{
    public static class ClaimExtensions
    {
        public static void AddFirstName(this ICollection<Claim> claims, string firstName)
        {
            claims.Add(new Claim(ClaimTypes.Name, firstName));
        }
        public static void AddLastName(this ICollection<Claim> claims, string lastName)
        {
            claims.Add(new Claim(ClaimTypes.Surname, lastName));
        }
        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
        }
        public static void AddRole(this ICollection<Claim> claims, string[] roles)
        {
            roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
        }
    }
}