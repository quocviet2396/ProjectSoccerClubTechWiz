using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectModels;
using ProjectSoccerClubApp.Repositories;

namespace ProjectSoccerClubApp.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _service;
        private IAuthenService aService;
        private ICategoryService bService;
        private IHttpContextAccessor _httpContext;

        public ShopController(IProductService service, IAuthenService aService, ICategoryService bService, IHttpContextAccessor httpContext)
        {
            _service = service;
            _httpContext = httpContext;
            this.aService = aService;
            this.bService = bService;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _service.GetProductWithCategory();
            model = model.Where(p => p.status == true).ToList();
            return View(model);
        }
    }
}
