using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.Products;
using ShoppingCart.Models;

namespace ShoppingCart.Data.Services
{
    public class ProductsService : IProductsService
    {

        private readonly ApplicationDbContext _context;
        public ProductsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int ProductID)
        {
            var result = await _context.Products.FirstOrDefaultAsync(x => x.ProductID == ProductID);
            _context.Products.Remove(result);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var result = await _context.Products.ToListAsync();
            return result;
        }

        public async Task<Product> GetProductByIdAsync(int ProductID)
        {
            var result = await _context.Products.FirstOrDefaultAsync(x => x.ProductID == ProductID);
            return result;
        }

        public async Task<Product> Update(int ProductID, Product newProduct)
        {
             _context.Update(newProduct);
            await _context.SaveChangesAsync();
            return newProduct;
        }


    }
}
