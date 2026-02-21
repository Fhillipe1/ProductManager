using ProductManager.Application.Interfaces;

namespace ProductManager.Application.Services
{
    public class LocalStorageService : IStorageService
    {
        private readonly string _storagePath;

        public LocalStorageService()
        {
            _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            if (!Directory.Exists(_storagePath))
                Directory.CreateDirectory(_storagePath);
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(_storagePath, uniqueFileName);

            using var file = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(file);

            return $"/Uploads/{uniqueFileName}";
        }
    }
}
