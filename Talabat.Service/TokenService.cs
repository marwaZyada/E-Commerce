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
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser appUser, UserManager<AppUser> userManager)
        {
            //private claims
            var authclaims = new List<Claim>() 
            { 
            new Claim(ClaimTypes.GivenName,appUser.UserName),
            new Claim(ClaimTypes.Email,appUser.Email),
            
            };
            var userroles = await userManager.GetRolesAsync(appUser);
            foreach (var role in userroles)
            {
                authclaims.Add(new Claim(ClaimTypes.Role,role));
                
            }

            //security key
            var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            //register claims
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValideIssure"],
                audience: _configuration["JWT:ValideAudience"],
                expires:DateTime.Now.AddDays(double.Parse(_configuration["JWT:ExpirationDays"])),
                claims:authclaims,
                signingCredentials:new SigningCredentials( authkey,SecurityAlgorithms.HmacSha256Signature)

                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
