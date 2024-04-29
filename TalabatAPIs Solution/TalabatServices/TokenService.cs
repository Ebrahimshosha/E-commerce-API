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
using TalabatCore.Entities.Identity;
using TalabatCore.Services;

namespace TalabatServices
{
	public class TokenService : ITokenServices
	{
		private readonly IConfiguration _configuration;

		public TokenService(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> usermanager)
		{
			// PayLoad [Data] [Claims]→ [Register Claims,Private Claims]

			//1. Private Claims
			var AuthClaims = new List<Claim>()
			{
				new Claim(ClaimTypes.GivenName,user.DisplayName),
				new Claim(ClaimTypes.Email,user.Email)
			};
			var userRole = await usermanager.GetRolesAsync(user);
			foreach (var role in userRole)
				AuthClaims.Add(new Claim(ClaimTypes.Role, role));


			var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

			var Token = new JwtSecurityToken(
				issuer: _configuration["JWT:ValidIssure"],  // Register Claims
				audience: _configuration["JWT:ValidAudience"], // Register Claims
				expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"])), // Register Claims
				claims: AuthClaims, // Private Claims
				signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256) // Header[key+algorithmType]
				);
			return new JwtSecurityTokenHandler().WriteToken(Token);
		}
	}
}
