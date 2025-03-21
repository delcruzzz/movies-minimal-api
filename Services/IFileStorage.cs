namespace MoviesMinimalAPI.Services
{
    public interface IFileStorage
    {
        Task DeleteFileAsync(string path, string folder);
        Task<string> StorageFileAsync(string folder, IFormFile file);
        async Task<string> EditFileAsync(string path, string folder, IFormFile file)
        {
            await DeleteFileAsync(path, folder);
            return await StorageFileAsync(folder, file);
        }
    }
}
