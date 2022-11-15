using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Models;

namespace ShoppingCart.Data.Cart
{
    public class CartLogic
    {
        public ApplicationDbContext _context { get; set; }
        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
        public CartLogic(ApplicationDbContext context)
        {
            _context = context;
        }

        public static CartLogic GetShoppingCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<ApplicationDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            return new CartLogic(context) { ShoppingCartId=cartId };
        }

        //Add to Cart. if null, becomes 1. else ++
        public void AddToCart(Product product)
        {
            var shoppingCartItem = _context.ShoppingCartItems
                .FirstOrDefault(x => x.Product.ProductID == product.ProductID && x.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    Product = product,
                    Quantity = 1
                };
                _context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Quantity++;
            }
            _context.SaveChanges();
        }

        //Remove from Cart.if null, remove. else --
        public void RemoveFromCart(Product product)
        {
            var shoppingCartItem = _context.ShoppingCartItems
                .FirstOrDefault(x => x.Product.ProductID == product.ProductID && x.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Quantity > 1)
                {
                    shoppingCartItem.Quantity--;
                }
                else
                {
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }
            _context.SaveChanges();
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ?? (ShoppingCartItems =
                _context.ShoppingCartItems.Where(x => x.ShoppingCartId == ShoppingCartId)
                .Include(x => x.Product).ToList());
        }

        public double GetCartTotal()
        //get list of item & calculate total w $ * qty
        {
            var total =
                _context.ShoppingCartItems.Where(x => x.ShoppingCartId == ShoppingCartId)
                .Select(x => x.Product.Price * x.Quantity).Sum();
            return total;
        }
    }
}
