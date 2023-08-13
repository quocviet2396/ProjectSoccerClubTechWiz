using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectModels;
using ProjectSoccerClubApp.Database_Helper;
using ProjectSoccerClubApp.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectSoccerClubApp.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryService _service;
        private IAuthenService aService;
        private IProductService pService;
        private DatabaseContext _dbContext;

        public CategoryController(ICategoryService service, IAuthenService aService, IProductService pService, DatabaseContext dbContext)
        {
            _service = service;
            _dbContext = dbContext;
            this.aService = aService;
            this.pService = pService;

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

            var model = await _service.GetCategory();
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            if (!aService.IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authen");
            }

            if (!aService.IsUserAdmin())
            {
                return RedirectToAction("Login", "Authen");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Categories newCategory)
        {
            try
            {
                // Nếu trạng thái không được chọn (false), đặt lại thành Inactive
                if (!newCategory.status)
                {
                    newCategory.status = false; // Inactive
                }
                else
                {
                    newCategory.status = true;  // Active
                }

                await _service.addCategory(newCategory);

                return RedirectToAction("Index", "Category");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(newCategory);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var categeory = await _service.GetCategoryById(id);
            var products = _dbContext.Product.Where(p => p.CategoryId == id).ToList();
            if (categeory == null || products.Count > 0)
            {
                return Json(new
                {
                    error = "Can not delete this category."
                });
            }
            bool isDeleted = await _service.deleteCategory(categeory);
            if (isDeleted)
            {
                return Json(new
                {
                    success = "Deleted this competition."
                });
            }
            return View();
        }
    }
}

