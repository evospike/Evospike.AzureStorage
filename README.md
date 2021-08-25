# Evospike.AzureStorage
This package is designed to use azure blob storage in a very simple way.

Allows you to configure AzureStorage in a very simple way

### `appsettings.json` configuration

The file path and other settings can be read from JSON configuration if desired.

In `appsettings.json` add a `"AzureStorageSetting"` properties:

```json
{
   "AzureStorageSetting": {
    "StorageConnectionString": "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;",
    "AccountUrl": "http://127.0.0.1:10000/devstoreaccount1"
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
}
```
