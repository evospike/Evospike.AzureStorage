using System;
using System.IO;
using System.Threading.Tasks;
using Evospike.AzureStorage.Interfaces;
using Evospike.AzureStorage.Settings;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace Evospike.AzureStorage.Services
{
    public class AzureBlobStorageService : IStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly AzureStorageSetting _azureStorageSetting;

        public AzureBlobStorageService(BlobServiceClient blobServiceClient, AzureStorageSetting azureStorageSetting)
        {
            _blobServiceClient = blobServiceClient;
            _azureStorageSetting = azureStorageSetting;
        }

        public async Task<BlobContainerClient> CreateContainerAsync(string containerName) 
        {
            return await _blobServiceClient.CreateBlobContainerAsync(containerName.Replace(".", "-"));
        }

        public async Task RemoveContainerAsync(string containerName) 
        {
            await _blobServiceClient.DeleteBlobContainerAsync(containerName.Replace(".", "-"));
        }

        public async Task<long> GetContainerSize(string containerName) 
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName.Replace(".", "-"));
            long size = 0;

            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                size += blobItem.Properties.ContentLength ?? 0;
            }

            return size;
        }

        public string GetProtectedUrl(string containerName, string blobPath, DateTimeOffset expireDate)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName.Replace(".", "-"));
            var blob = container.GetBlobClient(blobPath);
            var sasToken = blob.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, expireDate);

            return sasToken.AbsoluteUri;
        }

        public async Task RemoveBlobAsync(string containerName, string blobPath)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName.Replace(".", "-"));
            var blob = container.GetBlobClient(blobPath);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> SaveBlobAsync(string containerName, IFormFile file)
        {
            if (file == null)
                return null;

            var fileName = file.FileName;
            var extension = Path.GetExtension(fileName);
            var newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}-{Guid.NewGuid()}{extension}";

            using var stream = file.OpenReadStream();
            containerName = containerName.Replace(".", "-");
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlobClient(newFileName);
            await blob.UploadAsync(stream);

            return $"{_azureStorageSetting.AccountName}/{containerName}/{newFileName}";
        }

        public async Task<string> SaveBlobAsync(string containerName, string blobPath, Stream file)
        {
            if (file == null)
                return null;

            containerName = containerName.Replace(".", "-");
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlobClient(blobPath);
            await blob.UploadAsync(file);

            return $"{_azureStorageSetting.AccountName}/{containerName}/{blobPath}";
        }
    }
}
