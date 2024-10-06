using Practica.Domain.Entities;

namespace Practica.Application.Interfaces
{
    internal interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);

        Task UpdateProductByIdAsync(int id, Product product);

        Task DeleteProductByIdAsync(int id);
    }
}
