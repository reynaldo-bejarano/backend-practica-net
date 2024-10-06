using Microsoft.EntityFrameworkCore;
using Practica.Domain.Entities;
using Practica.Application.Interfaces;
using Practica.Infrastructure.Context;

namespace Practica.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDBContext _context;

        public ProductService(AppDBContext context)
        {
            _context = context;
        }

        //Actions

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }


        public async Task<Product> GetProductByIdAsync(int id) 
        {
            var product = await _context.Products.FindAsync(id);
            return product == null ? throw new KeyNotFoundException($"Product with ID {id} not found.") : product;
        }

        public async Task UpdateProductByIdAsync(int id, Product product)
        {
            var existingProduct = await _context.Products.FindAsync(id);

            if (existingProduct == null) throw new KeyNotFoundException($"Product with ID {id} not found.");

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) throw new KeyNotFoundException($"Product with ID {id} not found.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
