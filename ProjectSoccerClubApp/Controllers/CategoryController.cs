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

        public CategoryController(ICategoryService service, IAuthenService aService)
        {
            _service = service;
            this.aService = aService;


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

    }
}

