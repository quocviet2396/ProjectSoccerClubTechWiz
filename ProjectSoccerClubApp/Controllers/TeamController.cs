using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using ProjectModels;
using ProjectSoccerClubApp.Database_Helper;
using ProjectSoccerClubApp.Repositories;

namespace ProjectSoccerClubApp.Controllers
{
    public class TeamController : Controller
    {

        private ITeamServices _service;
        private readonly DatabaseContext _context;
        private IAuthenService aService;
        public TeamController(DatabaseContext context, ITeamServices service, IAuthenService aService)
        {
            _service = service;
            _context = context;
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
            var model = await _service.GetAllTeams();
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
        public async Task<IActionResult> Create(Team newTeam, IFormFile file)
        {
            try
            {
                bool isUnique = await _service.IsTeamNameUnique(newTeam.TeamName);

                if (!isUnique)
                {
                    ViewBag.errormsg = "Team Name already exists!!!";
                    return View();
                }
                bool isCoachUnique = await _service.IsCoachNameUnique(newTeam.CoachName);

                if (!isCoachUnique)
                {
                    ViewBag.errormsg = "Coach Name already exists!!!";
                    return View();
                }
                if (file != null)
                {
                    string path = Path.Combine("wwwroot/images/Logo", file.FileName);
                    var stream = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(stream);
                    newTeam.Logo = file.FileName;
                }
                await _service.PostTeam(newTeam);
                TempData["msg"] = "Congratulation !!! Create a new team success";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.errormsg = "Create a new team fail";
                return View();
            }
           
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = await _service.DeleteTeam(id);
                TempData["msg"] = "Delete Team Success";
                return RedirectToAction("Index") ;
                
            }
            catch (Exception)
            {
                ViewBag.errormsg = "Delete Team Fail";
                return RedirectToAction("Index");
            }
          
            
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var model = await _service.GetOneTeam(id);
            if (!aService.IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authen");
            }

            if (!aService.IsUserAdmin())
            {
                return RedirectToAction("Login", "Authen");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Team editTeam, IFormFile file)
        {
          
            var oldTeam = await _service.GetOneTeam(editTeam.Id);
            if (oldTeam != null)
            {
                if (file != null)
                {
                    string path = Path.Combine("wwwroot/images/Logo", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    if (!string.IsNullOrEmpty(oldTeam.Logo))
                    {

                        string OldPath = Path.Combine("wwwroot/images/Logo", oldTeam.Logo);

                        if (System.IO.File.Exists(OldPath))
                        {
                            System.IO.File.Delete(OldPath);
                        }
                    }
                }
            }
            else
            {
                ViewBag.error = "Edit Team fail";
                return View();
            }
            editTeam.Logo = file != null ? file.FileName : oldTeam.Logo;

            oldTeam.CoachName = editTeam.CoachName;

            oldTeam.Logo = editTeam.Logo;
            await _service.PutTeam(editTeam);
            TempData["msg"] = "Congratulation !!! Edit Success";
            return RedirectToAction("Index");
        }
    }
}

