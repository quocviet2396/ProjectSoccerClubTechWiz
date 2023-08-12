using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectModels;
using ProjectSoccerClubApp.Database_Helper;
using ProjectSoccerClubApp.Repositories;

namespace ProjectSoccerClubApp.Controllers
{
    public class CreateAccountController : Controller
    {


        private ICreateAcountService cService;
        private DatabaseContext db;

        public CreateAccountController(ICreateAcountService cService, DatabaseContext db)
        {
            this.cService = cService;
            this.db = db;

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        public bool CheckPass(string pass)
        {
            if (pass == null || pass.Length < 8)
            {
                return false;
            }
            return true;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User newUser)
        {
            try
            {
                bool userExists = await db.User.AnyAsync(u => u.Username == newUser.Username || u.Email == newUser.Email);

                if (userExists)
                {
                    ViewBag.errormsg = "User or email already exists, try again";
                    return View();
                }

                if (!CheckPass(newUser.Password))
                {
                    ViewBag.errormsg = "Password must be at least 8 characters";
                    return View();
                }
                HttpContext.Session.SetString("accNo", newUser.Username);
                bool added = await cService.addUser(newUser);

                return RedirectToAction("Index", "Frontend");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details()
        {
            string accNo = HttpContext.Session.GetString("accNo");
            var user = await cService.GetUserByAccNo(accNo);
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> EditByUser()
        {
            string accNo = HttpContext.Session.GetString("accNo");
            var user = await cService.GetUserByAccNo(accNo);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditByUser(User editUser)
        {
            try
            {

                await cService.edit(editUser);
                return RedirectToAction("Details", new { id = editUser.Id });
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditByAdmin(int id)
        {
            var user = await cService.GetUserById(id);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditByAdmin(User editUser)
        {
            try
            {

                await cService.edit(editUser);
                return RedirectToAction("Index", "CreateAccount");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var model = await cService.GetUser();
            return View(model);
        }

    }
}

