using AdminPanel.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using AdminPanel.Helpers;

namespace AdminPanel.Controllers
{
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _repo;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork Repo, IMapper mapper)
        {

            _repo = Repo;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _repo.Repo<Product>().GetAllAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewForm model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image != null)
                    model.PictureUrl = PictureSettings.UploadFile(model.Image, "products");
                else
                    model.PictureUrl = "images/products/hat-react2.png";

                await _repo.Repo<Product>().Add(_mapper.Map<Product>(model));
                await _repo.Complete();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _repo.Repo<Product>().GetByIdAsync(id);
            return View(_mapper.Map<ProductViewForm>( product));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id, ProductViewForm model)
        {
            if (id != model.Id) return NotFound();
            var product = await _repo.Repo<Product>().GetByIdAsync(id);
            var arr = model.PictureUrl.Split('/');
                var fileName = arr[arr.Length - 1];
               
        
            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    PictureSettings.DeleteFile("products", fileName);
                    model.PictureUrl = PictureSettings.UploadFile(model.Image, "products");
                  
                }
                else model.PictureUrl = product.PictureUrl;

                var updatedproduct = _mapper.Map(model,product);

                _repo.Repo<Product>().Update(updatedproduct);
                await _repo.Complete();
                return RedirectToAction("Index");
            }
            return View(model);
        }


      
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _repo.Repo<Product>().GetByIdAsync(id);
            if (product != null)
            {
                var arr = product.PictureUrl.Split('/');
                var fileName =arr[arr.Length - 1];
                if(!string.IsNullOrEmpty( product.PictureUrl))
                PictureSettings.DeleteFile("products", fileName);
                _repo.Repo<Product>().Delete(product);
                await _repo.Complete();
            }
            return RedirectToAction("Index");
        }
    }
}
