using Azure.Storage.Blobs;
using Evospike.AzureStorage.Interfaces;
using Evospike.AzureStorage.Services;
using Evospike.AzureStorage.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evospike.AzureStorage.Extensions
{
    public static class ServiceExtension
    {
        public static void AddAzureStorageClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(sp => configuration.GetSection(nameof(AzureStorageSetting)).Get<AzureStorageSetting>());
            services.AddScoped(sp =>
            {
                var azureStorageSetting = sp.GetRequiredService<AzureStorageSetting>();
                return new BlobServiceClient(azureStorageSetting.StorageConnectionString);
            });

            services.AddScoped<IStorageService, AzureBlobStorageService>();
        }
    }
}