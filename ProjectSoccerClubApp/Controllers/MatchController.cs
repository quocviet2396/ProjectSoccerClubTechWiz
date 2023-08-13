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
        private ICompetitionService cService;
        public MatchController(IMatchService service, ITeamServices tService, ICompetitionService cService)
        {
            this.service = service;
            this.tService = tService;
            this.cService = cService;
        }

        public async Task<IActionResult> Index()
        {
            var models = await service.GetMatchList();
            foreach (var item in models)
            {
                var homeTeam = await tService.GetOneTeam(item.HomeTeamId);
                var awayTeam = await tService.GetOneTeam(item.AwayTeamId);
                var competition = await cService.GetCompetitionById(item.CompetitionId);

                item.HomeTeam = homeTeam;
                item.AwayTeam = awayTeam;
                item.Competition = competition;
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
            ViewData["CompetitionId"] = new SelectList(await cService.GetCompetition(), "Id", "Name"); // Corrected method call
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Match newMatch)
        {
            try
            {
                ViewData["TeamID"] = new SelectList(await tService.GetAllTeams(), "Id", "TeamName", newMatch.Id);
                ViewData["CompetitionId"] = new SelectList(await cService.GetCompetition(), "Id", "Name", newMatch.CompetitionId);
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
            ViewData["CompetitionId"] = new SelectList(await cService.GetCompetition(), "Id", "Name");
            ViewData["TeamID"] = new SelectList(await tService.GetAllTeams(), "Id", "TeamName");
            var match = await service.GetMatchById(id);
            return View(match);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Match editMatch)
        {
            try
            {
                ViewData["CompetitionId"] = new SelectList(await cService.GetCompetition(), "Id", "Name", editMatch.CompetitionId);
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