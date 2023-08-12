using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using ProjectModels;
using ProjectSoccerClubApp.Repositories;

namespace ProjectSoccerClubApp.Controllers
{
	public class PlayerController : Controller
    {
        private IPlayerServices repoPlayer;
		private ITeamServices repoTeam;

		private IAuthenService repoAuthen;

		public PlayerController(IPlayerServices _repoPlayer, ITeamServices _repoTeam, IAuthenService _repoAuthen)
        {
			repoPlayer = _repoPlayer;
			repoTeam = _repoTeam;

			repoAuthen = _repoAuthen;
        }
		//     public async Task<IActionResult> Index()
		//     {
		//var teams = await repoTeam.GetAllTeams();
		//ViewBag.TeamId = new SelectList(teams, "Id", "TeamName");

		//var model = await repoPlayer.GetPlayers();
		//return View(model);
		//     }
		public async Task<IActionResult> Index(int teamId)
		{
			if (!repoAuthen.IsUserLoggedIn())
			{
				return RedirectToAction("Login", "Authen");
			}

			if (!repoAuthen.IsUserAdmin())
			{
				return RedirectToAction("Login", "Authen");
			}
			//----
			var teams = await repoTeam.GetAllTeams();
			ViewBag.TeamId = new SelectList(teams, "Id", "TeamName");
			//----

			IEnumerable<Player> model = await repoPlayer.GetPlayers(teamId);
			return View(model);
		}
		[HttpGet]
        public async Task<IActionResult> Create()
		{
			if (!repoAuthen.IsUserLoggedIn())
			{
				return RedirectToAction("Login", "Authen");
			}

			if (!repoAuthen.IsUserAdmin())
			{
				return RedirectToAction("Login", "Authen");
			}
			var teams = await repoTeam.GetAllTeams();
            //ViewBag.TeamId = new SelectList(teams.Select(x => x.Id));
            ViewBag.TeamId = new SelectList(teams, "Id", "TeamName");

            return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(Player newPlayer, IFormFile file)
		{
			if (file != null)
			{
				string path = Path.Combine("wwwroot/images/PlayerImg", file.FileName);
				var stream = new FileStream(path, FileMode.Create);
				await file.CopyToAsync(stream);

				newPlayer.PlayerImg = file.FileName;
			}
			await repoPlayer.AddPlayer(newPlayer);

			return RedirectToAction("Index");
		}
		[HttpGet]
		// GET: Player/Details/5
		public async Task<IActionResult> Details(int id)
		{
			var player = await repoPlayer.GetPlayer(id);
			return View(player);
		}

		[HttpGet]
		public async Task<IActionResult>Edit(int id)
		{
			if (!repoAuthen.IsUserLoggedIn())
			{
				return RedirectToAction("Login", "Authen");
			}

			if (!repoAuthen.IsUserAdmin())
			{
				return RedirectToAction("Login", "Authen");
			}
			var teams = await repoTeam.GetAllTeams();
			ViewBag.TeamId = new SelectList(teams, "Id", "TeamName");

			var player = await repoPlayer.GetPlayer(id);
			return View(player);
		}
		[HttpPost]
        public async Task<IActionResult> Edit(Player OldPlayer, IFormFile file)
        {
            try
            {
                var model = await repoPlayer.GetPlayer(OldPlayer.Id);
                if (model != null)
                {
                    if (file != null)
                    {
                        string path = Path.Combine("wwwroot/images/PlayerImg", file.FileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        if (!string.IsNullOrEmpty(model.PlayerImg))
                        {

                            string OldPath = Path.Combine("wwwroot/images/PlayerImg", model.PlayerImg);

                            if (System.IO.File.Exists(OldPath))
                            {
                                System.IO.File.Delete(OldPath);
                            }
                        }
                    }
                    OldPlayer.PlayerImg = file != null ? file.FileName : model.PlayerImg;

                    await repoPlayer.EditPlayer(OldPlayer);

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Fail");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

            }
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> Delete(int[] arrList)
		{
			foreach (int item in arrList)
			{
				await repoPlayer.DeletePlayer(item);
			}
			return RedirectToAction("Index");
		}
	}
}
