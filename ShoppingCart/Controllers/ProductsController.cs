using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Data.Products;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService _service;

        public ProductsController(IProductsService service)
        {
            _service = service;
        }
          
        public async Task<ActionResult> Index()
        {
            var allProducts = await _service.GetAllAsync();
            return View(allProducts);
        }

        //GET
        public async Task<IActionResult> ProductInfo(int ProductID)
        {
            var ProductInfo = await _service.GetProductByIdAsync(ProductID);
            if (ProductInfo == null) return View("NIL");
            return View(ProductInfo);
        }
    }
}
