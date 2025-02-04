using AdminPanel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.Entities;

namespace AdminPanel.Controllers
{
    public class ProductBrandController : Controller
    {
        private readonly IUnitOfWork _repo;

        public ProductBrandController(IUnitOfWork repo)
        {
            _repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _repo.Repo<ProductBrand>().GetAllAsync());
        }



        [HttpPost]
        public async Task<IActionResult> Create(ProductBrandViewModel model)
        {
           

                
            try
            {


                await _repo.Repo<ProductBrand>().Add(new ProductBrand()
                {
                    Name = model.Name
                });

                await _repo.Complete();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Name", "Name must be Unique");
            }
            return View("Index", await _repo.Repo<ProductBrand>().GetAllAsync());



        }
       
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _repo.Repo<ProductBrand>().GetByIdAsync(id);
            if (brand == null) return NotFound();
            else
            {
             _repo.Repo<ProductBrand>().Delete(brand);
                await _repo.Complete();
                return RedirectToAction("Index", await _repo.Repo<ProductBrand>().GetAllAsync());
            }
           
        }
    }
}
