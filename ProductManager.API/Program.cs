using Microsoft.EntityFrameworkCore;
using ProductManager.Application.Interfaces;
using ProductManager.Application.Services;
using ProductManager.Domain.Interfaces;
using ProductManager.Infrastructure.Data;
using ProductManager.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. CONEXÃO COM O BANCO DE DADOS

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. INJEÇÃO DE DEPENDÊNCIA

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStorageService, LocalStorageService>();

// 3. CONTROLLERS

builder.Services.AddControllers();

// 4. SWAGGER

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Product Manager API",
        Version = "v1",
        Description = "API REST para gerenciamento de produtos"
    });
});

var app = builder.Build();

// 5. MIDDLEWARE (o que roda ANTES dos controllers)

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Manager API v1");
    });
}

app.UseHttpsRedirection();


// 6. SERVIR ARQUIVOS ESTÁTICOS (imagens de upload)

var uploadsPath = Path.Combine(app.Environment.ContentRootPath, "Uploads");
if (!Directory.Exists(uploadsPath))
    Directory.CreateDirectory(uploadsPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(uploadsPath),
    RequestPath = "/Uploads"
});

app.UseAuthorization();
app.MapControllers();

app.Run();
