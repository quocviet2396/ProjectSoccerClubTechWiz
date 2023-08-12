using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ProjectModels;
using ProjectSoccerClubApp.Database_Helper;
using System.Linq;
using ProjectSoccerClubApp.Repositories;

namespace ProjectSoccerClubApp.Controllers
{
    public class BackendController : Controller
    {
        private readonly DatabaseContext _context;
        private IAuthenService aService;

        public BackendController(DatabaseContext context, IAuthenService aService)
        {
            _context = context;
            this.aService = aService;
        }

        public IActionResult Index()
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

    }
}