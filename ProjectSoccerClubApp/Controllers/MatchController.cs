using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectModels;
using ProjectSoccerClubApp.Repositories;

namespace ProjectSoccerClubApp.Controllers
{
    public class MatchController : Controller
    {
        private IMatchService service;
        private ITeamServices tService;
        public MatchController(IMatchService service, ITeamServices tService)
        {
            this.service = service;
            this.tService = tService;
        }

        public async Task<IActionResult> Index()
        {
            var models = await service.GetMatchList();
            foreach (var item in models)
            {
                var homeTeam = await tService.GetOneTeam(item.HomeTeamId);
                var awayTeam = await tService.GetOneTeam(item.AwayTeamId);
                item.HomeTeam = homeTeam;
                item.AwayTeam = awayTeam;
            }
            return View(models);
        }
        public IActionResult Details(Match match)
        {
            var models = service.GetMatchById(match.Id);
            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["TeamID"] = new SelectList(await tService.GetAllTeams(), "Id", "TeamName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Match newMatch)
        {
            try
            {
                ViewData["TeamID"] = new SelectList(await tService.GetAllTeams(), "Id", "TeamName", newMatch.Id);
                await service.addMatch(newMatch);
                return RedirectToAction("Index", "Match");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var match = service.GetMatchById(id);
            if (match != null)
            {
                service.removeMatch(id);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["TeamID"] = new SelectList(await tService.GetAllTeams(), "Id", "TeamName");
            var match = await service.GetMatchById(id);
            return View(match);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Match editMatch)
        {
            try
            {
                ViewData["TeamID"] = new SelectList(await tService.GetAllTeams(), "Id", "TeamName", editMatch.Id);
                await service.updateMatch(editMatch);
                return RedirectToAction("Index", "Match");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return View();
        }
    }
}
