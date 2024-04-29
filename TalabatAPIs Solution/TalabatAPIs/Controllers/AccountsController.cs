using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TalabatAPIs.DTO;
using TalabatAPIs.Error;
using TalabatAPIs.Extensions;
using TalabatCore.Entities.Identity;
using TalabatCore.Services;
using TalabatServices;

namespace TalabatAPIs.Controllers
{
    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenServices _Tokenservices;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager,
                                  SignInManager<AppUser> signInManager,
                                  ITokenServices Tokenservices,
                                  IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _Tokenservices = Tokenservices;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RigisterDto model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiRespponse(400, "This email is already exists"));
                //return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "This email is already exists" } });

            var User = new AppUser
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber
            };

            var Result = await _userManager.CreateAsync(User, model.Password);

            if (!Result.Succeeded) return BadRequest((new ApiRespponse(400)));

            var RequestUser = new UserDto()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                Token = await _Tokenservices.CreateTokenAsync(User, _userManager)
            };

            return Ok(RequestUser);
        }



        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User == null) return Unauthorized(new ApiRespponse(401));

            var Result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, false);
            if (!Result.Succeeded) return Unauthorized(new ApiRespponse(401));

            return Ok(new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _Tokenservices.CreateTokenAsync(User, _userManager)
            });
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _Tokenservices.CreateTokenAsync(user, _userManager)
            });
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetCurrenAddress")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            //var email = User.FindFirstValue(ClaimTypes.Email);
            //var user = _userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == email);

            var user = await _userManager.FindUserWithAddressAsync(User); // FindUserWithAddressAsync : Extension Method I Developed it	// user with Address

            var MappedAddress = _mapper.Map<Address, AddressDto>(user.Address);

            return Ok(MappedAddress);
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("UpdateAddress")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updateAddress)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            if (user is null) return Unauthorized(new ApiRespponse(401));
            var address = _mapper.Map<AddressDto, Address>(updateAddress);

            address.Id = user.Address.Id;

            user.Address = address;
            var Result = await _userManager.UpdateAsync(user);
            if (!Result.Succeeded) return BadRequest(new ApiRespponse(400));

            return Ok(updateAddress);
        }



        [HttpGet("CheckEmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }


    }
}
