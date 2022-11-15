using ShoppingCart.Models;

namespace ShoppingCart.Data.Products
{
    public interface IProductsService
    {
        Task <IEnumerable<Product>> GetAllAsync();
        Task <Product> GetProductByIdAsync(int ProductID);

        Task AddAsync(Product product);

        Task <Product> Update(int ProductID, Product newProduct);

        Task DeleteAsync(int ProductID);

    }
}
