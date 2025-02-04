using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Api.Dtos;
using Talabat.Api.Errors;
using Talabat.Api.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.Api.Controllers
{

    public class AccountsController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenservice;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager
            ,ITokenService tokenservice,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenservice = tokenservice;
            _mapper = mapper;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
           
                var user=await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
               
                    return Unauthorized(new ApiErrorResponse(401));
          var result=await _signInManager.CheckPasswordSignInAsync(user,model.Password,false);
            if (!result.Succeeded)
                return Unauthorized(new ApiErrorResponse(401));
            var userdto = new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token =await _tokenservice.CreateTokenAsync(user, _userManager)

            };
                return Ok(userdto);
        
        }


        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExist(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors=new string[]
                {
                    "Email is already exist"
                } });
            var user = new AppUser
            {
                Email=model.Email,
                DisplayName=model.DisplayName,
                PhoneNumber=model.TelephoneNo,
                UserName=model.UserName
            };
          var result=  await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(new ApiErrorResponse(400));
            var userdto = new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenservice.CreateTokenAsync(user, _userManager)

            };
            return Ok(userdto);

        }

        [Authorize]
        [HttpGet("currentuser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user=await _userManager.FindByEmailAsync(email);

            return Ok(new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenservice.CreateTokenAsync(user, _userManager)

            });

        }

        [Authorize]
        [HttpGet("useraddress")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.GetUserAddressAsync(User);
            var address = user.Address;
            return Ok(_mapper.Map<AddressDto>(address));
        }

        [Authorize]
        [HttpPut("updateuseraddress")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
           
            var user = await _userManager.GetUserAddressAsync(User);
            address.Id = user.Address.Id;
            var updatedaddress = _mapper.Map<Address>(address);
            user.Address = updatedaddress;
            var result= await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiErrorResponse(400));

            return Ok(address);
        }

        [HttpGet("checkemail")]
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
