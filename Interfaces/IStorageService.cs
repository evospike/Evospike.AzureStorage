using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace Evospike.AzureStorage.Interfaces
{
    public interface IStorageService
    {
        Task<BlobContainerClient> CreateContainerAsync(string containerName);
        Task RemoveContainerAsync(string containerName);
        Task<long> GetContainerSize(string containerName);
        Task<string> SaveBlobAsync(string containerName, IFormFile file);
        Task<string> SaveBlobAsync(string containerName, string blobPath, Stream file);
        Task<bool> ExistBlobAsync(string containerName, string blobPath);
        Task RemoveBlobAsync(string containerName, string blobPath);
        string GetProtectedUrl(string containerName, string blobPath, DateTimeOffset expireDate);
        Task<Stream> DownloadBlobAsync(string containerName, string blobPath);
        Task<BlobProperties> GetBlobProperties(string containerName, string blobPath);
    }
}