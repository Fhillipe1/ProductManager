namespace ProductManager.Application.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName);
    }
}
