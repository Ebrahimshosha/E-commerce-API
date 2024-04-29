using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TalabatCore.Entities.Identity;

namespace TalabatAPIs.Extensions
{
	public static class UserManagerExtension
	{
		public static Task<AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var user = userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == email);
			return user;
		}
	}
}
 