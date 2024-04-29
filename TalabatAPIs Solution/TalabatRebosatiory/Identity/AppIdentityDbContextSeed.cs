using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities.Identity;

namespace TalabatRebosatiory.Identity
{
	public  static class AppIdentityDbContextSeed
	{
		public static async Task SeedUserAsync(UserManager<AppUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				var User = new AppUser()
				{
					DisplayName = "Ebrahim shosha",
					Email = "Ebrahimshosha2024@gmail.com",
					PhoneNumber = "01023547181",
					UserName = "Ebrahimshosha2024"
				};
				await userManager.CreateAsync(User,"P@ssw0rd");
			}
		}
	}
}
