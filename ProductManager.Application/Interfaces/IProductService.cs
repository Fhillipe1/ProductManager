using ProductManager.Application.DTOs;
using ProductManager.Domain.Enums;

namespace ProductManager.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponseDTO?> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductResponseDTO>> GetAllAsync(string? category, decimal? minPrice, decimal? maxPrice, ProductStatus? status);
        Task<ProductResponseDTO> CreateAsync(CreateProductDTO dto);
        Task<ProductResponseDTO?> UpdateAsync(Guid id, UpdateProductDTO dto);
        Task<bool> DeleteAsync(Guid id);
        Task<ProductResponseDTO?> UpdateImageAsync(Guid id, string imageUrl);
    }
}
