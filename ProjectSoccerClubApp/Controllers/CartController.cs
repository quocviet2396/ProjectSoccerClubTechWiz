using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectModels;
using ProjectSoccerClubApp.Database_Helper;
using ProjectSoccerClubApp.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectSoccerClubApp.Controllers
{
    public class CartController : Controller
    {
        List<CartItem> cart;

        private DatabaseContext db;
        private IProductService _pService;
        private IAuthenService aService;
        private ICategoryService bService;
        private IHttpContextAccessor _httpContext;

        public CartController(DatabaseContext db, IProductService pService, IAuthenService aService, ICategoryService bService, IHttpContextAccessor httpContext)
        {
            _pService = pService;
            _httpContext = httpContext;
            this.db = db;
            this.aService = aService;
            this.bService = bService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (aService.IsUserAdmin()) {
                if (aService.IsUserAdmin())
                {
                    return Json(new
                    {
                        error = "Admin can not access this feature."
                    });
                }
            }
            this.cart = null;
            if (_httpContext.HttpContext.Session.GetString("MiamiShopCart") != null)
            {
                var cartJson = _httpContext.HttpContext.Session.GetString("MiamiShopCart");
                this.cart = JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
                ViewData["CartSubTotal"] = this.cart.Sum(item => item.Qty * Convert.ToInt32(item.Product.SellingPrice));
            }

            return View(this.cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int pid, int qty)
        {
            if (aService.IsUserAdmin())
            {
                return Json(new
                {
                    error = "Admin can not use this feature."
                });
            }
            string sessionCart = _httpContext.HttpContext.Session.GetString("MiamiShopCart");

            this.cart = sessionCart != null
                ? JsonConvert.DeserializeObject<List<CartItem>>(sessionCart)
                : new List<CartItem>();

            var product = await _pService.GetProductById(pid);

            foreach (var item in this.cart)
            {
                if (item.Product.Id == pid)
                {
                    if (item.Qty >= product.Quantity)
                    {
                        return Json(new
                        {
                            error = "Quantity exceeds available stock."
                        });
                    }
                    item.Qty += qty;
                    _httpContext.HttpContext.Session.SetString("MiamiShopCart", JsonConvert.SerializeObject(this.cart));

                    return Json(new
                    {
                        cartQuantity = this.cart.Sum(item => item.Qty)
                    });
                }
            }

            CartItem cartItem = new CartItem(product, qty);
            this.cart.Add(cartItem);

            string cartJson = JsonConvert.SerializeObject(this.cart);
            _httpContext.HttpContext.Session.SetString("MiamiShopCart", cartJson);

            return Json(new
            {
                cartQuantity = this.cart.Sum(item => item.Qty)
            });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeQuantity(int pid, int qty)
        {
            var cartJson = _httpContext.HttpContext.Session.GetString("MiamiShopCart");
            var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            var existingCartItem = cart.FirstOrDefault(item => item.Product.Id == pid);

            if (existingCartItem != null)
            {
                if (qty > existingCartItem.Product.Quantity)
                {
                    return Json(new { 
                        error = "Quantity exceeds available stock."
                    });
                }
                existingCartItem.Qty = qty;
                _httpContext.HttpContext.Session.SetString("MiamiShopCart", JsonConvert.SerializeObject(cart));
            }

            return Json(new
            {
                cartItemSubTotal = Convert.ToInt32(existingCartItem.Product.SellingPrice) * existingCartItem.Qty,
                cartQuantity = cart.Sum(item => item.Qty),
                cartSubTotal = cart.Sum(item => item.Qty * Convert.ToInt32(item.Product.SellingPrice))
            });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int pid)
        {
            var cartJson = _httpContext.HttpContext.Session.GetString("MiamiShopCart");
            var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            var removedCartItem = cart.FirstOrDefault(item => item.Product.Id == pid);

            if (removedCartItem != null)
            {
                cart.Remove(removedCartItem);
                _httpContext.HttpContext.Session.SetString("MiamiShopCart", JsonConvert.SerializeObject(cart));
            }

            var cartQuantity = cart.Sum(item => item.Qty);
            var cartSubTotal = cart.Sum(item => item.Qty * Convert.ToInt32(item.Product.SellingPrice));

            return Json(new
            {
                cartQuantity = cartQuantity,
                cartSubTotal = cartSubTotal
            });
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            // Xóa session cart
            _httpContext.HttpContext.Session.Remove("MiamiShopCart");

            return Ok(); 
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            if (aService.IsUserAdmin())
            {
                return RedirectToAction("Index", "Backend");
            }
            var username = _httpContext.HttpContext.Session.GetString("accNo");
            var user = db.User.FirstOrDefault(u => u.Username.Equals(username));
            if (user == null)
            {
                _httpContext.HttpContext.Session.SetString("returnCheckout", "true");
                return RedirectToAction("Login", "Authen"); // Hoặc xử lý khi người dùng không tồn tại
            }
            ViewData["User"] = user;

            var cartJson = _httpContext.HttpContext.Session.GetString("MiamiShopCart");
            if (cartJson != null) {
                var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
                ViewData["Cart"] = cart;
                ViewData["CartSubTotal"] = cart.Sum(item => item.Qty * Convert.ToInt32(item.Product.SellingPrice));
            }
            
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            // Lấy thông tin người dùng từ Session và gán UserId
            var username = _httpContext.HttpContext.Session.GetString("accNo");
            var user = db.User.FirstOrDefault(u => u.Username.Equals(username));
            if (user == null)
            {
                return RedirectToAction("Login", "Authen"); // Hoặc xử lý khi người dùng không tồn tại
            }
            order.UserId = user.Id;
            order.PaymentMethod = "PayPal";

            Random random = new Random();

            // Tạo một dãy số ngẫu nhiên 10 chữ số
            int randomNumber = random.Next(100000000, 999999999);

            order.TrackingNo = "MIAM-APTECH" + randomNumber;

            db.Orders.Add(order);
            db.SaveChanges();

            // Lấy các CartItem từ session và thêm vào OrderDetails
            var cartJson = _httpContext.HttpContext.Session.GetString("MiamiShopCart");
            var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
            foreach (var cartItem in cart)
            {
                var orderDetail = new OrderDetails
                {
                    OrderId = order.Id,
                    ProductId = cartItem.Product.Id,
                    CategoryId = cartItem.Product.CategoryId,
                    Quantity = cartItem.Qty
                };
                var product = db.Product.FirstOrDefault(p => p.Id == cartItem.Product.Id);
                product.Quantity -= cartItem.Qty;
                if (product.Quantity == 0) {
                    product.status = false;
                }
                db.OrderDetails.Add(orderDetail);
            }

            // Thêm đơn hàng vào cơ sở dữ liệu
            db.SaveChanges();

            // Xóa giỏ hàng
            _httpContext.HttpContext.Session.Remove("MiamiShopCart");

            return View("Thankyou");
        }
    }
}
