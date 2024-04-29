using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TalabatCore.Entities.Identity;
using TalabatCore.Services;
using TalabatRebosatiory.Identity;
using TalabatServices;

namespace TalabatAPIs.Extensions
{
	public static class IdentityServicesExtentions
	{
		public static IServiceCollection AddIdentityServices(this IServiceCollection Services, IConfiguration configuration)
		{
			Services.AddIdentity<AppUser, IdentityRole>(options =>
			{
				//options.Password.RequireLowercase = true;
			})
				.AddEntityFrameworkStores<AppIdentityDbContext>();

			Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
					.AddJwtBearer(options =>
					{
						options.TokenValidationParameters = new TokenValidationParameters()
						{
							ValidateIssuer = true,
							ValidIssuer = configuration["JWT:ValidIssure"],
							ValidateAudience = true,
							ValidAudience = configuration["JWT:ValidAudience"],
							ValidateLifetime = true,
							ValidateIssuerSigningKey = true,
							IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
						};
					});

			Services.AddScoped<ITokenServices, TokenService>();

			return Services;
		}
	}
}
