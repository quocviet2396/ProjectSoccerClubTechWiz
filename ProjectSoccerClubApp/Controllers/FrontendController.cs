using Microsoft.AspNetCore.Mvc;
using ProjectSoccerClubApp.Models;
using ProjectSoccerClubApp.Repositories;
using System.Diagnostics;

namespace ProjectSoccerClubApp.Controllers
{
    public class FrontendController : Controller
    {
        private readonly ILogger<FrontendController> _logger;
        private ITeamServices _teamServ;
        private IMatchService _matchServ;
        private IPlayerServices _playerServ;

        public FrontendController(ILogger<FrontendController> logger, ITeamServices teamServ, IMatchService matchServ, IPlayerServices playerServ)
        {
            _logger = logger;
            _teamServ = teamServ;
            _playerServ = playerServ;
            _matchServ = matchServ;
        }

        public async Task<IActionResult> Index()
        {
            var top10players = await _playerServ.GetTop10Players();

            var viewModel = new CombinedModel_HomePage
            {
                Top10Players = top10players
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Team()
        {
            var teams = await _teamServ.GetAllTeams();
            return View(teams);
        }

        public async Task<IActionResult> Player(int teamId)
        {
            var player = await _playerServ.GetPlayers(teamId);
            return View(player);
        }

        public async Task<IActionResult> Match()
        {
            var matches = await _matchServ.GetMatchList();
            foreach (var item in matches)
            {
                var homeTeam = await _teamServ.GetOneTeam(item.HomeTeamId);
                var awayTeam = await _teamServ.GetOneTeam(item.AwayTeamId);
                item.HomeTeam = homeTeam;
                item.AwayTeam = awayTeam;
            }
            return View(matches);
        }

        public IActionResult News()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}