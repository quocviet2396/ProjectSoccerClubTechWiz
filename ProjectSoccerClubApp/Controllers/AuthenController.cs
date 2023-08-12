using Microsoft.AspNetCore.Mvc;
using ProjectModels;
using ProjectSoccerClubApp.Database_Helper;
using ProjectSoccerClubApp.Repositories;
using System.Collections.Generic;

namespace ProjectSoccerClubApp.Controllers
{
    public class AuthenController : Controller
    {
        private readonly DatabaseContext _context;
        private IAuthenService aService;
        private IHttpContextAccessor _httpContext;

        public AuthenController(DatabaseContext context, IAuthenService aService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
            this.aService = aService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (aService.IsUserLoggedIn() && aService.IsUserAdmin())
            {
                return RedirectToAction("Index", "Backend");
            }

            if (aService.IsUserLoggedIn() && !aService.IsUserAdmin())
            {
                if (_httpContext.HttpContext.Session.GetString("returnCheckout") == "true")
                {
                    return RedirectToAction("Checkout", "Cart");
                }
                return RedirectToAction("Index", "Frontend");
            }
            return View();
        }

        private bool IsLoginValid(string username, string password)
        {
            // Kiểm tra tính hợp lệ của tên đăng nhập và mật khẩu trong cơ sở dữ liệu
            var account = _context.User.FirstOrDefault(a => a.Username == username && a.Password == password);

            // Trả về true nếu tìm thấy tài khoản hợp lệ, ngược lại trả về false
            return account != null;
        }

        [HttpPost]
        public IActionResult Login(User model)
        {
            if (!ModelState.IsValid)
            {
                if (IsLoginValid(model.Username, model.Password))
                {
                    HttpContext.Session.SetString("accNo", model.Username);

                    // Kiểm tra role của người dùng và chuyển hướng tới trang tương ứng
                    if (aService.IsUserAdmin())
                    {
                        return RedirectToAction("Index", "Backend");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Frontend");
                    }
                }
                else
                {
                    ViewBag.errormsg = "Username or Password was wrong!! Please try again or create new one";
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("accNo") == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                HttpContext.Session.Remove("accNo");
                return RedirectToAction("Index", "Frontend");
            }
        }
    }
}