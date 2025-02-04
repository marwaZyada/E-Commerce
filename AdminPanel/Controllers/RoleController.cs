using AdminPanel.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.Core.Entities.Identity;

namespace AdminPanel.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {

            return View(await _roleManager.Roles.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleExist =await _roleManager.RoleExistsAsync(model.Name);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Name));
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", "Role is Exist");
                    return View("Index", await _roleManager.Roles.ToListAsync());
                }
            }
            return RedirectToAction(nameof(Index));
           
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var roleviewmodel=new RoleViewModel() { 
                Id = role.Id ,
                Name=role.Name
            };
                return View(roleviewmodel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit( RoleViewModel form)
        {
           
            if (ModelState.IsValid)
            {
                var roleexist = await _roleManager.RoleExistsAsync(form.Name);
                if (!roleexist)
                {
                  var role=  await _roleManager.FindByIdAsync(form.Id);
                    role.Name = form.Name;
                    await _roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", "Role is Exist");
                    return View(form);
                } 
               
                
            }
            return View(form);
        }
        public async Task<IActionResult> Delete(string id)
        {
            var role=await _roleManager.Roles.FirstOrDefaultAsync(r=>r.Id==id);
            if (role is not null)
                await _roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(Index));
        }
        }
}
