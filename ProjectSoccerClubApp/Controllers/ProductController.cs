using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectModels;
using ProjectSoccerClubApp.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectSoccerClubApp.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _service;
        private IAuthenService aService;
        private ICategoryService bService;

        public ProductController(IProductService service, IAuthenService aService, ICategoryService bService)
        {
            _service = service;
            this.aService = aService;
            this.bService = bService;


        }
        public async Task<IActionResult> Index()
        {
            if (!aService.IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authen");
            }

            if (!aService.IsUserAdmin())
            {
                return RedirectToAction("Login", "Authen");
            }

            var model = await _service.GetProductWithCategory();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!aService.IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authen");
            }

            if (!aService.IsUserAdmin())
            {
                return RedirectToAction("Login", "Authen");
            }

            var categories = await bService.GetCategory();
            var categorySelectList = categories.Select(category => new SelectListItem
            {
                Value = category.Id.ToString(),
                Text = category.Name
            }).ToList();

            ViewData["CategoryID"] = new SelectList(categorySelectList, "Value", "Text");
            ViewBag.Categories = categorySelectList; // Set ViewBag.Categories
            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            if (!aService.IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authen");
            }

            if (!aService.IsUserAdmin())
            {
                return RedirectToAction("Login", "Authen");
            }

            var categories = await bService.GetCategory();
            var categorySelectList = categories.Select(category => new SelectListItem
            {
                Value = category.Id.ToString(),
                Text = category.Name
            }).ToList();

            ViewBag.Categories = categorySelectList; // Set ViewBag.Categories
            var product = await _service.GetProductById(id);
            return View(product);
        }


//       [HttpPost]
//public async Task<IActionResult> Edit(int id, Products editedProduct, IFormFile file)
//{
//    if (!aService.IsUserLoggedIn())
//    {
//        return RedirectToAction("Login", "Authen");
//    }

//    if (!aService.IsUserAdmin())
//    {
//        return RedirectToAction("Login", "Authen");
//    }

//    if (id != editedProduct.Id)
//    {
//        return NotFound();
//    }

//    try
//    {
//        var existingProduct = await _service.GetProductById(id);

//        if (file != null)
//        {
//            string path = Path.Combine("wwwroot/images/ProductPhoto", file.FileName);
//            using (var stream = new FileStream(path, FileMode.Create))
//            {
//                await file.CopyToAsync(stream);
//            }
//            editedProduct.Photo = file.FileName;
//        }
//        else
//        {
//            // Keep the existing photo if no new photo is uploaded
//            editedProduct.Photo = existingProduct.Photo;
//        }

//        // Update other properties of the product
//        existingProduct.Name = editedProduct.Name;
//        existingProduct.Slug = editedProduct.Slug;
//        existingProduct.Descrption = editedProduct.Descrption;
//        existingProduct.OriginalPrice = editedProduct.OriginalPrice;
//        existingProduct.SellingPrice = editedProduct.SellingPrice;
//        existingProduct.Trending = editedProduct.Trending;
//        existingProduct.Featured = editedProduct.Featured;
//        existingProduct.status = editedProduct.status;
//        existingProduct.CategoryId = editedProduct.CategoryId;

//        await _service.EditProduct(existingProduct);

//        return RedirectToAction("Index", "Product");
//    }
//    catch (Exception ex)
//    {
//        ModelState.AddModelError(string.Empty, ex.Message);
//        return View(editedProduct);
//    }
//}

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!aService.IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authen");
            }

            if (!aService.IsUserAdmin())
            {
                return RedirectToAction("Login", "Authen");
            }

            var product = await _service.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            await _service.DeleteProduct(product);

            return RedirectToAction("Index", "Product");
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Products editProducts, IFormFile file)
        {
            var oldProduct = await _service.GetProductById(editProducts.Id);
            if (oldProduct != null)
            {
                if (file != null)
                {
                    string path = Path.Combine("wwwroot/images/ProductPhoto", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    if (!string.IsNullOrEmpty(oldProduct.Photo))
                    {

                        string OldPath = Path.Combine("wwwroot/images/ProductPhoto", oldProduct.Photo);

                        if (System.IO.File.Exists(OldPath))
                        {
                            System.IO.File.Delete(OldPath);
                        }
                    }
                }
            }
            editProducts.Photo = file != null ? file.FileName : oldProduct.Photo;

            
            oldProduct.Name = editProducts.Name;
            oldProduct.Slug = editProducts.Slug;
            oldProduct.Descrption = editProducts.Descrption;
            oldProduct.OriginalPrice = editProducts.OriginalPrice;
            oldProduct.SellingPrice = editProducts.SellingPrice;
            oldProduct.Trending = editProducts.Trending;
            oldProduct.Featured = editProducts.Featured;
            oldProduct.Quantity = editProducts.Quantity;
            oldProduct.status = editProducts.status;
            oldProduct.Photo = editProducts.Photo;
            oldProduct.CategoryId = editProducts.CategoryId;
            await _service.EditProduct(editProducts);
            ViewBag.msg = "Edit Product Success";
            return RedirectToAction("Index","Product");
        }
    
    [HttpPost]
        public async Task<IActionResult> Create(Products newProducts,IFormFile file )
        {
            try
            {
                if (file != null)
                {
                    string path = Path.Combine("wwwroot/images/ProductPhoto", file.FileName);
                    var stream = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(stream);

                    newProducts.Photo = file.FileName;
                }

                await _service.AddProduct(newProducts);

                return RedirectToAction("Index", "Product");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(newProducts);
            }
        }
    }
}

