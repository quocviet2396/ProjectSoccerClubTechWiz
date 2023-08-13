using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectModels;
using ProjectSoccerClubApp.Repositories;

namespace ProjectSoccerClubApp.Controllers
{
    public class NewsController : Controller
    {
        private INewsService service;
        public NewsController(INewsService service)
        {
            this.service = service;
        }
        public async Task<IActionResult> Index()
        {
            var news = await service.GetNewsList();
            return View(news);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(News newNews, IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    string path = Path.Combine("wwwroot/images/News", file.FileName);
                    var stream = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(stream);
                    newNews.Logo = file.FileName;
                }
                await service.addNews(newNews);
                TempData["msg"] = "Congratulation !!! Create a news success";
                return RedirectToAction("Index", "News");
            }
            catch (Exception ex)
            {
                ViewBag.errormsg = "Create a news fail";
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = await service.removeNews(id);
                TempData["msg"] = "Delete News Success";
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                ViewBag.errormsg = "Delete News Fail";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var model = await service.GetNewsById(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(News editNews, IFormFile file)
        {

            var oldNews = await service.GetNewsById(editNews.ID);
            if (oldNews != null)
            {
                if (file != null)
                {
                    string path = Path.Combine("wwwroot/images/News", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    if (!string.IsNullOrEmpty(oldNews.Logo))
                    {

                        string OldPath = Path.Combine("wwwroot/images/News", oldNews.Logo);

                        if (System.IO.File.Exists(OldPath))
                        {
                            System.IO.File.Delete(OldPath);
                        }
                    }
                }
            }
            else
            {
                ViewBag.error = "Edit fail";
                return View();
            }
            editNews.Logo = file != null ? file.FileName : oldNews.Logo;
            oldNews.Title = editNews.Title;
            oldNews.Content = editNews.Content;
            oldNews.Author = editNews.Author;
            oldNews.PublishDate = editNews.PublishDate;
            oldNews.Logo = editNews.Logo;
            await service.updateNews(editNews);
            TempData["msg"] = "Congratulation !!! Edit Success";
            return RedirectToAction("Index");
        }
    }
}

