using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectModels;
using ProjectSoccerClubApp.Repositories;

namespace ProjectSoccerClubApp.Controllers
{
    public class CartController : Controller
    {
        List<CartItem> cart;

        private IProductService _pService;
        private IAuthenService aService;
        private ICategoryService bService;
        private IHttpContextAccessor _httpContext;

        public CartController(IProductService pService, IAuthenService aService, ICategoryService bService, IHttpContextAccessor httpContext)
        {
            _pService = pService;
            _httpContext = httpContext;
            this.aService = aService;
            this.bService = bService;
        }

        public IActionResult Index()
        {
            this.cart = null;
            if (_httpContext.HttpContext.Session.GetString("MiamiShopCart") != null)
            {
                var cartJson = _httpContext.HttpContext.Session.GetString("MiamiShopCart");
                this.cart = JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
                ViewData["CartSubTotal"] = this.cart.Sum(item => item.Qty * Convert.ToInt32(item.Product.OriginalPrice));
            }
            
            return View(this.cart);
        }

        public async Task<IActionResult> AddToCart(int pid, int qty)
        {
            string sessionCart = _httpContext.HttpContext.Session.GetString("MiamiShopCart");

            this.cart = sessionCart != null
                ? JsonConvert.DeserializeObject<List<CartItem>>(sessionCart)
                : new List<CartItem>();

            // Kiểm tra nếu sản phẩm đã tồn tại trong giỏ hàng thì cộng qty vào qty hiện tại
            bool productExistsInCart = false;
            foreach (var item in this.cart)
            {
                if (item.Product.Id == pid)
                {
                    item.Qty += qty;
                    productExistsInCart = true;
                    break;
                }
            }

            if (!productExistsInCart)
            {
                CartItem cartItem = new CartItem(await _pService.GetProductById(pid), qty);
                this.cart.Add(cartItem);
            }

            string cartJson = JsonConvert.SerializeObject(this.cart);
            _httpContext.HttpContext.Session.SetString("MiamiShopCart", cartJson);

            return Json(new
            {
                cartQuantity = cart.Sum(item => item.Qty)
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
                existingCartItem.Qty = qty;
                _httpContext.HttpContext.Session.SetString("MiamiShopCart", JsonConvert.SerializeObject(cart));
            }

            return Json(new {
                cartItemSubTotal = Convert.ToInt32(existingCartItem.Product.OriginalPrice) * existingCartItem.Qty,
                cartQuantity = cart.Sum(item => item.Qty),
                cartSubTotal = cart.Sum(item => item.Qty * Convert.ToInt32(item.Product.OriginalPrice))
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
            var cartSubTotal = cart.Sum(item => item.Qty * Convert.ToInt32(item.Product.OriginalPrice));

            return Json(new
            {
                cartQuantity = cartQuantity,
                cartSubTotal = cartSubTotal
            });
        }
    }
}
