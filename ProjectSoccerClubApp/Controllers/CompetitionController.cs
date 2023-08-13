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
        private IMatchService mService;
        public CompetitionController(ICompetitionService service, IAuthenService aService, IMatchService mService)
        {
            _service = service;
            this.aService = aService;
            this.mService = mService;
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

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var competition = await _service.GetCompetitionById(id);
            var competMatches = await mService.GetMatchesByCompetition(id);
            if (competition == null || competMatches.Count > 0)
            {
                return Json(new
                {
                    error = "Can not delete this competition."
                });
            }
            bool isDeleted = await _service.deleteCompetition(competition);
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

