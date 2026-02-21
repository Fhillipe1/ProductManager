using ProductManager.Domain.Entities;
using ProductManager.Domain.Enums;

namespace ProductManager.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllAsync(string? category, decimal? minPrice, decimal? maxPrice, ProductStatus? status);
        Task<Product> AddAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task DeleteAsync(Guid id);
    }
}
