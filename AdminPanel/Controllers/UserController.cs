using AdminPanel.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities.Identity;

namespace AdminPanel.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
          _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            IEnumerable<UserViewModel> users;
            if (string.IsNullOrEmpty(SearchValue))
            {
                users = await _userManager.Users.Select(u => new UserViewModel()
                {
                    Id = u.Id,
                    DisplayName = u.DisplayName,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(u).Result.ToList(),

                }).ToListAsync();
            }
            else
            {
                users = _userManager.Users.Where(u => u.UserName.Contains(SearchValue.Trim())).Select(u => new UserViewModel()
                {
                    Id = u.Id,
                    DisplayName = u.DisplayName,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(u).Result.ToList(),

                });
            }
            return View(users);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var roles =await _roleManager.Roles.ToListAsync();
            var user =await _userManager.Users.FirstOrDefaultAsync(u=>u.Id==id);
            var usermodel = new UserFormViewModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(r => new RoleViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList(),
            };
            return View(usermodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserFormViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return BadRequest();
            var userroles =await _userManager.GetRolesAsync(user);
          await  _userManager.RemoveFromRolesAsync(user, userroles);
            foreach (var role in model.Roles)
            {
                if(role.IsSelected)
               await _userManager.AddToRoleAsync(user, role.Name);
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Search(string SearchValue)
        {
            var users =  _userManager.Users.Where(u=>u.UserName.Contains(SearchValue)).Select(u => new UserViewModel()
            {
                Id = u.Id,
                DisplayName = u.DisplayName,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Roles = _userManager.GetRolesAsync(u).Result.ToList(),

            });
            return PartialView("_Search",users);
        }
    }
}
