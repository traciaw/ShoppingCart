using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.Cart;
using ShoppingCart.Data.Products;
using ShoppingCart.Data.ViewModel;
using System.Security.AccessControl;


namespace ShoppingCart.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly CartLogic _cartLogic;

        public OrdersController(IProductsService productsService, CartLogic cartLogic)
        {
            _productsService = productsService;
            _cartLogic = cartLogic;
        }
        public IActionResult Index()
        {
            var item = _cartLogic.GetShoppingCartItems();
            _cartLogic.ShoppingCartItems= item;

            var response = new ShoppingCartVM()
            {
                CartLogic = _cartLogic,
                CartTotal = _cartLogic.GetCartTotal()
            };
            return View(response);
        }

        public async Task <RedirectToActionResult> AddToCart(int ProductID)
        {
            var item = await _productsService.GetProductByIdAsync(ProductID);
            if(item != null)
            {
                _cartLogic.AddToCart(item);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<RedirectToActionResult> RemoveFromCart(int ProductID)
        {
            var item = await _productsService.GetProductByIdAsync(ProductID);
            if (item != null)
            {
                _cartLogic.RemoveFromCart(item);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
