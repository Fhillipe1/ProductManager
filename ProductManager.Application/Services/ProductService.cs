using ProductManager.Application.DTOs;
using ProductManager.Application.Interfaces;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Enums;
using ProductManager.Domain.Interfaces;

namespace ProductManager.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductResponseDTO?> GetByIdAsync(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return null;

            return MapToResponseDTO(product);
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetAllAsync(
            string? category,
            decimal? minPrice,
            decimal? maxPrice,
            ProductStatus? status)
        {
            var products = await _repository.GetAllAsync(category, minPrice, maxPrice, status);

            return products.Select(p => MapToResponseDTO(p));
        }

        public async Task<ProductResponseDTO> CreateAsync(CreateProductDTO dto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Category = dto.Category,
                Status = ProductStatus.Active,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdProduct = await _repository.AddAsync(product);

            return MapToResponseDTO(createdProduct);
        }

        public async Task<ProductResponseDTO?> UpdateAsync(Guid id, UpdateProductDTO dto)
        {
            var existingProduct = await _repository.GetByIdAsync(id);

            if (existingProduct == null)
                return null;

            existingProduct.Name = dto.Name;
            existingProduct.Description = dto.Description;
            existingProduct.Price = dto.Price;
            existingProduct.Category = dto.Category;
            existingProduct.Status = dto.Status;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            var updatedProduct = await _repository.UpdateAsync(existingProduct);

            return MapToResponseDTO(updatedProduct);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingProduct = await _repository.GetByIdAsync(id);

            if (existingProduct == null)
                return false;

            await _repository.DeleteAsync(id);

            return true;
        }

        public async Task<ProductResponseDTO?> UpdateImageAsync(Guid id, string imageUrl)
        {
            var existingProduct = await _repository.GetByIdAsync(id);

            if (existingProduct == null)
                return null;

            existingProduct.ImageUrl = imageUrl;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            var updatedProduct = await _repository.UpdateAsync(existingProduct);

            return MapToResponseDTO(updatedProduct);
        }

        private static ProductResponseDTO MapToResponseDTO(Product product)
        {
            return new ProductResponseDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
                Status = product.Status,
                ImageUrl = product.ImageUrl,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }
    }
}
