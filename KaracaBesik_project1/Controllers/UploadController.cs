using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly ILogger<UploadController> _logger;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public UploadController(ILogger<UploadController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _blobServiceClient = new BlobServiceClient(configuration["AzureBlobStorage:ConnectionString"]);
        _containerName = configuration["AzureBlobStorage:ContainerName"];
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        try
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(file.FileName);

            await using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            stopwatch.Stop();
            var uploadTimeInSeconds = stopwatch.Elapsed.TotalSeconds;
            var fileSizeInBytes = file.Length;
            var speedInMbps = (fileSizeInBytes * 8) / (uploadTimeInSeconds * 1_000_000);

            return Ok(new { message = "File uploaded successfully to Azure Blob Storage.", uploadSpeed = speedInMbps + " Mbps" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while uploading the file.");
            return StatusCode(500, "An internal error occurred");
        }
    }

}
