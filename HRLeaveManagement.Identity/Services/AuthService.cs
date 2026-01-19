using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Models.Identity;
using HRLeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRLeaveManagement.Identity.Services
{

    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthResponse> Login(AuthRequest request)
        {
            // See if user exists
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new NotFoundException($"User with {request.Email} not found.", request.Email);
            }

            // Can this user sign in with this password? If yes, true.
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded == false)
            {
                throw new BadRequestException($"Credentials for '{request.Email}' are not valid.");
            }

            // Successfully verified the user. Generate token for user.
            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

            // User authenticated
            var response = new AuthResponse
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName
            };

            return response;
        }


        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Employee");
                return new RegistrationResponse() { UserId = user.Id };
            }
            else
            {
                StringBuilder str = new StringBuilder();
                foreach (var err in result.Errors)
                {
                    str.AppendFormat("- {0}\n", err.Description);
                }

                throw new BadRequestException($"{str}");
            }
        }




        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            // Retrieve user claims from DB
            var userClaims = await _userManager.GetClaimsAsync(user);

            // Retrieve user role from DB
            var userRoles = await _userManager.GetRolesAsync(user);

            // Once we send the token, everything will be seen as a claim. Claims determine permissions in the app

            // Give me all the strings and select them into new objects of type Claim. The role value is stored in x
            var roleClaims = userRoles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            // Create an array of claims that will be condensed into JWT string
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName), // Sub identifies as user, could be email, etc.
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Generate new Guid every login
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id) // Custom field
            }
            .Union(userClaims)
            .Union(roleClaims);

            // Generate symmetric security key. Encode by UTF8
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            // Pass in key and hash as 256
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            // Create JWT security token
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes), // Changed from DateTime.Now
                signingCredentials: signingCredentials);

            return jwtSecurityToken;

        }
    }
}
