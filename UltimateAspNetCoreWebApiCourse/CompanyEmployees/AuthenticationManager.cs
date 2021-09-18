using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<User> userManager;

        private readonly IConfiguration configuration;

        private User user;

        public AuthenticationManager(UserManager<User> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuth)
        {
            this.user = await this.userManager.FindByNameAsync(userForAuth.UserName);

            return (this.user != null && await this.userManager.CheckPasswordAsync(this.user, userForAuth.Password));
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = this.GetSigningCredentials();
            var claims = await this.GetClaims();
            var tokenOptions = this.GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigningCredentials()
        {
            byte[] key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, this.user.UserName)
            };

            var roles = await this.userManager.GetRolesAsync(this.user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = this.configuration.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings.GetValue<string>("validIssuer"),
                audience: jwtSettings.GetValue<string>("validAudience"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtSettings.GetValue<double>("expires")),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }
    }
}
