using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MoviesAPI.Helpers
{
    public class LocalFileStorageService : IFileStorageService
    {
        private IWebHostEnvironment environment;
        private IHttpContextAccessor httpContextAcessor;

        public LocalFileStorageService(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            this.environment = environment;
            this.httpContextAcessor = httpContextAccessor;
        }

        public async Task<string> SaveFile(string containerName, IFormFile file)
        {
            string filename = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string folder = Path.Combine(this.environment.WebRootPath, containerName);
            string route = Path.Combine(folder, filename);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                var content = ms.ToArray();
                await File.WriteAllBytesAsync(route, content);
            }

            string url = $"{this.httpContextAcessor.HttpContext.Request.Scheme}://{this.httpContextAcessor.HttpContext.Request.Host}";
            string dbRoute = Path.Combine(url, containerName, filename).Replace("\\", "/");

            return dbRoute;
        }

        public Task DeleteFile(string fileRoute, string containerName)
        {
            if (string.IsNullOrEmpty(fileRoute))
            {
                return Task.CompletedTask;
            }

            string filename = Path.GetFileName(fileRoute);
            string fileDirectory = Path.Combine(this.environment.WebRootPath, containerName, filename);

            if (File.Exists(fileDirectory))
            {
                File.Delete(fileDirectory);
            }

            return Task.CompletedTask;
        }

        public async Task<string> EditFile(string containerName, IFormFile file, string fileRoute)
        {
            await this.DeleteFile(fileRoute, containerName);
            return await this.SaveFile(containerName, file);
        }
    }
}