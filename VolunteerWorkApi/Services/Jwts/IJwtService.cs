using Microsoft.AspNetCore.Identity;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Services.Jwts
{
    public interface IJwtService
    {
        public AuthToken CreateToken(IdentityUser<long> user, string role);
    }
}
