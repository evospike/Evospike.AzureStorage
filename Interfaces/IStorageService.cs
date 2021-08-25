using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Evospike.AzureStorage.Interfaces
{
    public interface IStorageService
    {
        Task<string> SaveBlobAsync(string containerName, IFormFile file);
        Task RemoveBlobAsync(string containerName, string blobName);
        string GetProtectedUrl(string containerName, string blobName, DateTimeOffset expireDate);
    }
}