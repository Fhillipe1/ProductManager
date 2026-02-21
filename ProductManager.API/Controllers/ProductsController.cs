using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.DTOs;
using ProductManager.Application.Interfaces;
using ProductManager.Domain.Enums;

namespace ProductManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IStorageService _storageService;

        public ProductsController(IProductService productService, IStorageService storageService)
        {
            _productService = productService;
            _storageService = storageService;
        }

        // GET /api/products
        // Listar produtos com filtros opcionais

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetAll(
            [FromQuery] string? category,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] ProductStatus? status)
        {
            var products = await _productService.GetAllAsync(category, minPrice, maxPrice, status);
            return Ok(products);
        }

        // GET /api/products/{id}
        // Buscar produto por ID

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDTO>> GetById(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound(new { message = "Produto não encontrado" });

            return Ok(product);
        }

        // POST /api/products
        // Cadastrar novo produto

        [HttpPost]
        public async Task<ActionResult<ProductResponseDTO>> Create([FromBody] CreateProductDTO dto)
        {
            var product = await _productService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // PUT /api/products/{id}
        // Editar produto existente

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponseDTO>> Update(Guid id, [FromBody] UpdateProductDTO dto)
        {
            var product = await _productService.UpdateAsync(id, dto);

            if (product == null)
                return NotFound(new { message = "Produto não encontrado" });

            return Ok(product);
        }

        // DELETE /api/products/{id}
        // Excluir produto

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var deleted = await _productService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Produto não encontrado" });

            return NoContent();
        }

        // POST /api/products/{id}/image
        // Upload de imagem do produto

        [HttpPost("{id}/image")]
        public async Task<ActionResult<ProductResponseDTO>> UploadImage(Guid id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Nenhum arquivo enviado" });

            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound(new { message = "Produto não encontrado" });

            using var stream = file.OpenReadStream();
            var imageUrl = await _storageService.UploadFileAsync(stream, file.FileName);

            var updateDto = new UpdateProductDTO
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
                Status = product.Status
            };

            var updatedProduct = await _productService.UpdateImageAsync(id, imageUrl);

            if (updatedProduct == null)
                return NotFound(new { message = "Produto não encontrado" });

            return Ok(updatedProduct);
        }
    }
}
