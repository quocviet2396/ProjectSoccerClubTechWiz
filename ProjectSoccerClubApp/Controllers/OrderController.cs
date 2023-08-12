using Microsoft.AspNetCore.Mvc;
using ProjectSoccerClubApp.Database_Helper;
using ProjectSoccerClubApp.Repositories;

namespace ProjectSoccerClubApp.Controllers
{
    public class OrderController : Controller
    {
        private IOrderService _serv;
        private DatabaseContext _db;
        public OrderController(IOrderService serv, DatabaseContext db)
        {
            _serv = serv;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var models = await _serv.GetAllOrders();
            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id) {
            var model = await _serv.GetOneOrder(id);
            model.OrderDetails = _db.OrderDetails.Where(details => details.OrderId == id).ToList();
            foreach (var item in model.OrderDetails) {
                item.Product = _db.Product.FirstOrDefault(p => p.Id == item.ProductId); 
            }
            return View(model);
        }
    }
}
