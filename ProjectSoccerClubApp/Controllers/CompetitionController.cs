using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectModels;
using ProjectSoccerClubApp.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectSoccerClubApp.Controllers
{
    public class CompetitionController : Controller
    {
        private ICompetitionService _service;
        private IAuthenService aService;
        public CompetitionController(ICompetitionService service, IAuthenService aService)
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

            var model = await _service.GetCompetition();
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
        public async Task<IActionResult> Create(Competition newCompetition)
        {
            try
            {
              await _service.addCompetition(newCompetition);

                return RedirectToAction("Index", "Competition");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(newCompetition);
            }
        }
    }
}

