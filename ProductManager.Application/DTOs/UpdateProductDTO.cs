using ProductManager.Domain.Enums;

namespace ProductManager.Application.DTOs
{
    public class UpdateProductDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public ProductStatus Status { get; set; }
    }
}
