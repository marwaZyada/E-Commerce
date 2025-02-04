using AdminPanel.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.Dtos;
using Talabat.Core.Entities.Identity;

namespace AdminPanel.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager
            ,RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
            public async Task<IActionResult> Login(LoginDto model)
        {
            
                var user=await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                  
               {
                    ModelState.AddModelError("", "user not found");
                    return RedirectToAction(nameof(Login));
                }
                var result =await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if(!result.Succeeded || !await _userManager.IsInRoleAsync(user,"Admin"))
                {
                    ModelState.AddModelError("", "NotAuthorized");
                    return RedirectToAction(nameof(Login));
                }
                return RedirectToAction(nameof(Index),"User");
           
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
            public async Task<IActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser()
                {
                    Email = model.Email,
                    UserName=model.UserName,
                    PhoneNumber=model.TelephoneNo
                };
                await _userManager.CreateAsync(user,model.Password);
                return RedirectToAction(nameof(Login));
            }
                return View(model);
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
