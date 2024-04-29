using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities.Identity;

namespace TalabatCore.Services
{
	public interface ITokenServices
	{
		Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> usermanager);
	}
}
