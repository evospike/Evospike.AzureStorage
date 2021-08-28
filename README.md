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
    "AccountUrl": "{YourAccountUrl}"
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
    
    public IActionResult ViewFile(string fileName)
    {
        var responseUrl = _storageService.GetProtectedUrl("ContainerName", fileName, DateTimeOffset.UtcNow.AddSeconds(10));
        return Redirect(responseUrl);
    }
    
    public async Task<IActionResult> DeleteFile(string fileName)
    {
        await _storageService.RemoveBlobAsync("ContainerName", fileName);
        return View();
    }
}
```
