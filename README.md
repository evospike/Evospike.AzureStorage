# Evospike.AzureStorage
This package is designed to use azure blob storage in a very simple way.

Allows you to configure AzureStorage in a very simple way

### `appsettings.json` configuration

The file path and other settings can be read from JSON configuration if desired.

In `appsettings.json` add a `"AzureStorageSetting"` properties:

```json
{
   "AzureStorageSetting": {
    "StorageConnectionString": "{YourConnectionString}",
    "AccountName": "{YourAccountName}"
  }
}
```

And then pass the configuration section to the next methods:

```csharp
services.AddAzureStorageClient(Configuration);
```

Example of a controller using dependency injection services

```csharp
public class ItemsController : ControllerBase
{
    private readonly IStorageService _storageService;

    public ItemsController(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        var responseUrl = await _storageService.SaveBlobAsync("ContainerName", file);
        return Redirect(responseUrl);
    }

    public async Task<IActionResult> UploadFile(string blobPath, Stream file)
    {
        var responseUrl = await _storageService.SaveBlobAsync("ContainerName", blobPath, file);
        return Redirect(responseUrl);
    }
    
    public IActionResult ViewFile(string filePath)
    {
        var responseUrl = _storageService.GetProtectedUrl("ContainerName", filePath, DateTimeOffset.UtcNow.AddSeconds(10));
        return Redirect(responseUrl);
    }
    
    public async Task<IActionResult> DeleteFile(string filePath)
    {
        await _storageService.RemoveBlobAsync("ContainerName", filePath);
        return View();
    }
}
```

Additional methods

```csharp
_storageService.CreateContainerAsync("ContainerName");
_storageService.RemoveContainerAsync("ContainerName");
_storageService.GetContainerSize("ContainerName");
_storageService.DownloadBlobAsync("ContainerName", "blobPath");
_storageService.GetBlobProperties("ContainerName", "blobPath");
_storageService.ExistBlobAsync("ContainerName", "blobPath");
```