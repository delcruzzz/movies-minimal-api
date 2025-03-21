
namespace MoviesMinimalAPI.Services
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LocalFileStorage(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task DeleteFileAsync(string path, string folder)
        {
            if (string.IsNullOrEmpty(path))
            {
                return Task.CompletedTask;
            }

            string filePath = Path.Combine(webHostEnvironment.WebRootPath, folder, Path.GetFileName(path));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask; // Return a completed task
        }

        public async Task<string> StorageFileAsync(string folder, IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            string folderPath = Path.Combine(webHostEnvironment.WebRootPath, folder);

            // Create the folder if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, fileName); // Combine the folder path and the file name to get the full path
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                await File.WriteAllBytesAsync(filePath, content);
            }

            var currentUrl = $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var fileUrl = Path.Combine(currentUrl, folder, fileName).Replace("\\", "/");

            return fileUrl;
        }
    }
}
