using Moq;
using ProductManager.Application.DTOs;
using ProductManager.Application.Services;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Enums;
using ProductManager.Domain.Interfaces;

namespace ProductManager.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _service = new ProductService(_mockRepository.Object);
        }

        // TESTE 1: Criar produto com sucesso

        [Fact]
        public async Task CreateAsync_ValidDTO_ReturnsCreatedProduct()
        {
            // ARRANGE (Preparar)
            var dto = new CreateProductDTO
            {
                Name = "Mouse Gamer",
                Description = "Mouse com 7 botões",
                Price = 149.90m,
                Category = "Periféricos"
            };

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product p) => p);

            // ACT (Agir)
            var result = await _service.CreateAsync(dto);

            // ASSERT (Verificar)
            Assert.NotNull(result);
            Assert.Equal("Mouse Gamer", result.Name);
            Assert.Equal(149.90m, result.Price);
            Assert.Equal("Periféricos", result.Category);
            Assert.Equal(ProductStatus.Active, result.Status);
            Assert.NotEqual(Guid.Empty, result.Id);
        }

        // TESTE 2: Buscar produto por ID existente

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsProduct()
        {
            // ARRANGE
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Name = "Teclado Mecânico",
                Description = "Teclado RGB",
                Price = 299.90m,
                Category = "Periféricos",
                Status = ProductStatus.Active,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync(product);

            // ACT
            var result = await _service.GetByIdAsync(productId);

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal("Teclado Mecânico", result.Name);
            Assert.Equal(299.90m, result.Price);
        }

        // TESTE 3: Buscar produto por ID inexistente

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            // ARRANGE
            var fakeId = Guid.NewGuid();

            _mockRepository
                .Setup(r => r.GetByIdAsync(fakeId))
                .ReturnsAsync((Product?)null);

            // ACT
            var result = await _service.GetByIdAsync(fakeId);

            // ASSERT
            Assert.Null(result);
        }

        // TESTE 4: Atualizar produto existente

        [Fact]
        public async Task UpdateAsync_ExistingProduct_ReturnsUpdatedProduct()
        {
            // ARRANGE
            var productId = Guid.NewGuid();
            var existingProduct = new Product
            {
                Id = productId,
                Name = "Mouse Antigo",
                Description = "Descrição antiga",
                Price = 50.00m,
                Category = "Periféricos",
                Status = ProductStatus.Active,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var updateDto = new UpdateProductDTO
            {
                Name = "Mouse Novo",
                Description = "Descrição nova",
                Price = 99.90m,
                Category = "Periféricos",
                Status = ProductStatus.Active
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product p) => p);

            // ACT
            var result = await _service.UpdateAsync(productId, updateDto);

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal("Mouse Novo", result.Name);
            Assert.Equal("Descrição nova", result.Description);
            Assert.Equal(99.90m, result.Price);
        }

        // TESTE 5: Atualizar produto inexistente

        [Fact]
        public async Task UpdateAsync_NonExistingProduct_ReturnsNull()
        {
            // ARRANGE
            var fakeId = Guid.NewGuid();
            var updateDto = new UpdateProductDTO
            {
                Name = "Qualquer",
                Description = "Qualquer",
                Price = 10.00m,
                Category = "Qualquer",
                Status = ProductStatus.Active
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(fakeId))
                .ReturnsAsync((Product?)null);

            // ACT
            var result = await _service.UpdateAsync(fakeId, updateDto);

            // ASSERT
            Assert.Null(result);
        }

        // TESTE 6: Deletar produto existente

        [Fact]
        public async Task DeleteAsync_ExistingProduct_ReturnsTrue()
        {
            // ARRANGE
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Name = "Produto Para Deletar",
                Price = 10.00m,
                Category = "Teste",
                Status = ProductStatus.Active,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync(product);

            _mockRepository
                .Setup(r => r.DeleteAsync(productId))
                .Returns(Task.CompletedTask);

            // ACT
            var result = await _service.DeleteAsync(productId);

            // ASSERT
            Assert.True(result);
        }

        // TESTE 7: Deletar produto inexistente

        [Fact]
        public async Task DeleteAsync_NonExistingProduct_ReturnsFalse()
        {
            // ARRANGE
            var fakeId = Guid.NewGuid();

            _mockRepository
                .Setup(r => r.GetByIdAsync(fakeId))
                .ReturnsAsync((Product?)null);

            // ACT
            var result = await _service.DeleteAsync(fakeId);

            // ASSERT
            Assert.False(result);
        }

        // TESTE 8: Listar produtos retorna lista
        
        [Fact]
        public async Task GetAllAsync_ReturnsProductList()
        {
            // ARRANGE
            var products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Produto 1",
                    Price = 10.00m,
                    Category = "Cat A",
                    Status = ProductStatus.Active,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Produto 2",
                    Price = 20.00m,
                    Category = "Cat B",
                    Status = ProductStatus.Inactive,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            _mockRepository
                .Setup(r => r.GetAllAsync(null, null, null, null))
                .ReturnsAsync(products);

            // ACT
            var result = await _service.GetAllAsync(null, null, null, null);

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}
