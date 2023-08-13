﻿using Microsoft.AspNetCore.Mvc;
using ProjectModels;
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
        private ICompetitionService cService;
        private IPlayerServices _playerServ;
        private INewsService _newsServ;


        public FrontendController(ILogger<FrontendController> logger, ITeamServices teamServ, IMatchService matchServ, ICompetitionService cService, IPlayerServices playerServ, INewsService newsServ)
        {
            _logger = logger;
            _teamServ = teamServ;
            _matchServ = matchServ;
            this.cService = cService;
            _playerServ = playerServ;
            _newsServ = newsServ;
        }

        public async Task<IActionResult> Index()
        {
            var news = await _newsServ.GetNewsList();

            var top10players = await _playerServ.GetTop10Players();
            var viewModel = new CombinedModel_HomePage
            {
                Top10Players = top10players,
                News = news.Take(3).ToList()
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

        public async Task<IActionResult> NewsDetail(int id)
        {
            var newsItem = await _newsServ.GetNewsById(id);
            if (newsItem == null)
            {
                return NotFound();
            }
            return View(newsItem);
        }



        public async Task<IActionResult> Match(int? competitionId)
        {
            var matches = await _matchServ.GetMatchList();
            var competitions = await cService.GetCompetition(); 

            foreach (var item in matches)
            {
                var competition = await cService.GetCompetitionById(item.CompetitionId);
                var homeTeam = await _teamServ.GetOneTeam(item.HomeTeamId);
                var awayTeam = await _teamServ.GetOneTeam(item.AwayTeamId);
                item.HomeTeam = homeTeam;
                item.AwayTeam = awayTeam;
                item.Competition = competition;
            }

            ViewBag.Competitions = competitions; 

            if (competitionId.HasValue)
            {
                matches = matches.Where(m => m.CompetitionId == competitionId).ToList();
            }

            return View(matches);
        }






        public async Task<IActionResult> News()
        {
            var news = await _newsServ.GetNewsList();
            return View(news);
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