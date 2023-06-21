using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Enums;
using VolunteerWorkApi.Helpers;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Models;
using VolunteerWorkApi.Services.Jwts;

namespace VolunteerWorkApi.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly IJwtService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<long>> _roleManager;

        public UsersService(
            IJwtService jwtService,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<long>> roleManager)
        {
            _jwtService = jwtService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDefaultUser()
        {
            var user = await _userManager.FindByNameAsync("admin");

            if (user == null)
            {
                var defaultUser = new ManagementEmployee
                {
                    UserName = "admin",
                    FirstName = "Admin",
                    LastName = "Admin",
                };

                await CreateManagement(
                    new CreateAccount(defaultUser, "123qwe"));
            }
        }

        public async Task<AuthenticationResponse> Authenticate(
            AuthenticateRequest authRequest, AccountType accountType)
        {

            var user = await FindUser(authRequest.UserNameOrEmail);

            if (user != null)
            {
                var isPasswordCorrect = await _userManager
                    .CheckPasswordAsync(user, authRequest.Password);

                if (isPasswordCorrect)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    if (!userRoles.IsNullOrEmpty()
                        && IsAccountTypeMatchRole(accountType, userRoles.First()))
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(authRequest.FCMToken))
                            {
                                user.FCMToken = authRequest.FCMToken;

                                var identityResult = await _userManager.UpdateAsync(user);

                                if (!identityResult.Succeeded)
                                {
                                    throw new Exception();
                                }
                            }

                            var token = _jwtService.CreateToken(
                            user,
                            userRoles.First());

                            return new AuthenticationResponse(user, token);
                        }
                        catch
                        {
                            throw new ApiResponseException(
                              HttpStatusCode.Unauthorized,
                              ErrorMessages.AuthError,
                              ErrorMessages.ErrorWhileTryingToAuth);
                        }
                    }
                    else
                    {
                        throw new ApiResponseException(
                            HttpStatusCode.Unauthorized,
                            ErrorMessages.AuthError,
                            ErrorMessages.NoPermissionsForAccount);
                    }
                }
            }

            throw new ApiResponseException(
                HttpStatusCode.Unauthorized,
                ErrorMessages.AuthError,
                ErrorMessages.ErrorUserNameOrPassword);
        }

        public async Task<CreatedUser> CreateManagement(CreateAccount user)
        {
            return await CreateUserAccount(user, UsersRoles.Management);
        }

        public async Task<CreatedUser> CreateOrganization(CreateAccount user)
        {
            return await CreateUserAccount(user, UsersRoles.Organization);
        }

        public async Task<CreatedUser> CreateStudent(CreateAccount user)
        {
            return await CreateUserAccount(user, UsersRoles.Student);
        }

        public async Task<AuthToken> ChangeUserPassword(
            string currentUserId,
            string currentPassword,
            string newPassword)
        {
            if (currentPassword == newPassword)
            {
                throw new ApiResponseException(
                    HttpStatusCode.Unauthorized,
                      ErrorMessages.AuthError,
                      ErrorMessages.OldAndCurrentPasswordsMatch);
            }

            var user = await _userManager.FindByIdAsync(currentUserId);

            if (user != null)
            {
                try
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    if (userRoles.IsNullOrEmpty())
                    {
                        throw new Exception();
                    }

                    var identityResult = await _userManager
                        .ChangePasswordAsync(user, currentPassword, newPassword);

                    if (!identityResult.Succeeded)
                    {
                        throw new Exception();
                    }

                    return _jwtService.CreateToken(
                    user,
                    userRoles.First());
                }
                catch
                {
                    throw new ApiResponseException(
                        HttpStatusCode.Unauthorized,
                      ErrorMessages.AuthError,
                      ErrorMessages.ErrorTryingChangePassword);
                }
            }

            throw new ApiResponseException(
                HttpStatusCode.Unauthorized,
                ErrorMessages.AuthError,
                ErrorMessages.ErrorUserNotFound);
        }

        public async Task<AuthToken> ChangeUserName(
           string currentUserId,
           string newUserName)
        {
            var currentUser = await _userManager.FindByIdAsync(currentUserId);

            if (currentUser != null)
            {
                var otherUser = await _userManager.FindByNameAsync(newUserName);

                if (otherUser != null)
                {
                    throw new ApiResponseException(
                        HttpStatusCode.Unauthorized,
                          ErrorMessages.AuthError,
                          ErrorMessages.ThisUserNameAlreadyTaken);
                }

                try
                {
                    var userRoles = await _userManager.GetRolesAsync(currentUser);

                    if (userRoles.IsNullOrEmpty())
                    {
                        throw new Exception();
                    }

                    currentUser.UserName = newUserName;

                    var identityResult = await _userManager
                        .UpdateAsync(currentUser);

                    if (!identityResult.Succeeded)
                    {
                        throw new Exception();
                    }

                    return _jwtService.CreateToken(
                    currentUser,
                    userRoles.First());
                }
                catch
                {
                    throw new ApiResponseException(
                        HttpStatusCode.Unauthorized,
                      ErrorMessages.AuthError,
                      ErrorMessages.ErrorTryingChangeUserName);
                }
            }

            throw new ApiResponseException(
                HttpStatusCode.Unauthorized,
                ErrorMessages.AuthError,
                ErrorMessages.ErrorUserNotFound);
        }

        public async Task<string> GenerateChangeEmailToken(
            ApplicationUser user,
            string newEmail)
        {
            return await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
        }

        public async Task<IdentityResult> ChangeUserEmail(
            ApplicationUser user,
            string newEmail,
            string token)
        {
            return await _userManager.ChangeEmailAsync(user, newEmail, token);
        }

        public async Task<string> GenerateChangePhoneNumberToken(
            ApplicationUser user,
            string phoneNumber)
        {
            return await _userManager.
                GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
        }

        public async Task<IdentityResult> ChangeUserPhoneNumber(
           ApplicationUser user,
            string phoneNumber,
            string token)
        {
            return await _userManager.ChangePhoneNumberAsync(user, phoneNumber, token);
        }

        private async Task<ApplicationUser?> FindUser(string userNameOrEmail)
        {
            ApplicationUser? user;

            if (Validation.IsValidEmailAddress(userNameOrEmail))
            {
                user = await _userManager.FindByEmailAsync(userNameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(userNameOrEmail);
            }

            return user;
        }

        private async Task<CreatedUser> CreateUserAccount(CreateAccount createAccount, string role)
        {
            var createUserResult = await _userManager.CreateAsync(
                createAccount.ApplicationUser,
                createAccount.Password);

            if (!createUserResult.Succeeded)
            {
                throw ConvertErrors.
                    ConvertIdentityResultErrors(createUserResult.Errors);
            }

            try
            {
                await HandleRole(createAccount.ApplicationUser, role);

                var authToken = _jwtService.CreateToken(
                     createAccount.ApplicationUser,
                    role);

                return new CreatedUser(
                    createAccount.ApplicationUser, authToken);
            }
            catch
            {
                throw new ApiResponseException(
                  HttpStatusCode.Unauthorized,
                  ErrorMessages.AuthError,
                  ErrorMessages.ErrorWhileTryingToCreateAccount);
            }
        }

        private async Task HandleRole(ApplicationUser applicationUser, string role)
        {

            var roleExists = await _roleManager.RoleExistsAsync(role);

            if (!roleExists)
            {
                var createRoleResult = await _roleManager.CreateAsync(new IdentityRole<long>(role));

                if (!createRoleResult.Succeeded)
                {
                    throw ConvertErrors.
                        ConvertIdentityResultErrors(createRoleResult.Errors);
                }
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(applicationUser, role);

            if (!addToRoleResult.Succeeded)
            {
                throw ConvertErrors.
                    ConvertIdentityResultErrors(addToRoleResult.Errors);
            }
        }

        private static bool IsAccountTypeMatchRole(AccountType accountType, string userRole)
        {
            if (accountType is AccountType.Management
                && userRole == UsersRoles.Management)
            {
                return true;
            }

            if (accountType is AccountType.Organization
                && userRole == UsersRoles.Organization)
            {
                return true;
            }

            if (accountType is AccountType.Student
                && userRole == UsersRoles.Student)
            {
                return true;
            }

            return false;
        }
    }
}
