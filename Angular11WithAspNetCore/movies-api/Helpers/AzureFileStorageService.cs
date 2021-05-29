using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MoviesAPI.Helpers
{
    public class AzureFileStorageService : IFileStorageService
    {
        private string connectionString;

        public AzureFileStorageService(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("AzureStorageConnection");
        }

        public async Task<string> SaveFile(string containerName, IFormFile file)
        {
            BlobContainerClient client = new BlobContainerClient(this.connectionString, containerName);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            string filename = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var blob = client.GetBlobClient(filename);
            await blob.UploadAsync(file.OpenReadStream());

            return blob.Uri.ToString();
        }

        public async Task DeleteFile(string fileRoute, string containerName)
        {
            if (string.IsNullOrEmpty(fileRoute))
            {
                return;
            }

            BlobContainerClient client = new BlobContainerClient(this.connectionString, containerName);
            await client.CreateIfNotExistsAsync();
            string filename = Path.GetFileName(fileRoute);
            var blob = client.GetBlobClient(filename);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> EditFile(string containerName, IFormFile file, string fileRoute)
        {
            await this.DeleteFile(fileRoute, containerName);
            return await this.SaveFile(containerName, file);
        }
    }
}