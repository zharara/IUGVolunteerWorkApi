using Microsoft.AspNetCore.Identity;
using VolunteerWorkApi.Enums;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Services.Users
{
    public interface IUsersService
    {
        Task SeedDefaultUser();

        Task<AuthenticationResponse> Authenticate(
            AuthenticateRequest authRequest, AccountType accountType);

        Task<CreatedUser> CreateManagement(CreateAccount user);

        Task<CreatedUser> CreateOrganization(CreateAccount user);

        Task<CreatedUser> CreateStudent(CreateAccount user);

        Task<AuthToken> ChangeUserPassword(
            string currentUserId,
            string currentPassword,
            string newPassword);
    }
}
