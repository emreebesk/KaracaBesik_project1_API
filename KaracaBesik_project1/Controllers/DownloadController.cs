//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace KaracaBesik_project1.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class DownloadController : ControllerBase
//    {
//        private readonly ILogger<DownloadController> _logger;
//        private readonly HttpClient _httpClient;
//        private const string TestFileUrl = "http://ipv4.download.thinkbroadband.com/50MB.zip";

//        public DownloadController(ILogger<DownloadController> logger, IHttpClientFactory httpClientFactory)
//        {
//            _logger = logger;
//            _httpClient = httpClientFactory.CreateClient();
//        }
//        [HttpGet]
//        public async Task<IActionResult> DownloadFile()
//        {
//            try
//            {
//                _logger.LogInformation("Download test started at {Time}", DateTime.UtcNow);
//                var stopwatch = new System.Diagnostics.Stopwatch();
//                stopwatch.Start();
//                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
//                var response = await _httpClient.GetAsync(TestFileUrl, HttpCompletionOption.ResponseContentRead);

//                if (!response.IsSuccessStatusCode)
//                {
//                    return StatusCode((int)response.StatusCode, "Failed to download the file.");
//                }

//                var fileSizeInBytes = response.Content.Headers.ContentLength ?? 0;

//                stopwatch.Stop();
//                var downloadTimeInSeconds = stopwatch.Elapsed.TotalSeconds;
//                var speedInMbps = (fileSizeInBytes / downloadTimeInSeconds) * 8 / 1_000_000;

//                _logger.LogInformation("Download test completed in {DownloadTimeInSeconds} seconds with a speed of {SpeedInMbps} Mbps", downloadTimeInSeconds, speedInMbps);

//                return Ok(new { DownloadSpeedInMbps = speedInMbps });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while downloading the test file.");
//                return StatusCode(500, "An internal error occurred");
//            }
//        }


//    }
//}

//using Azure.Storage.Blobs;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Diagnostics;
//using System.IO;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace KaracaBesik_project1.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class DownloadController : ControllerBase
//    {
//        private readonly ILogger<DownloadController> _logger;
//        private readonly HttpClient _httpClient;
//        private readonly BlobServiceClient _blobServiceClient;
//        private readonly string _containerName;
//        private const string TestFileUrl = "http://ipv4.download.thinkbroadband.com/50MB.zip";

//        public DownloadController(ILogger<DownloadController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
//        {
//            _logger = logger;
//            _httpClient = httpClientFactory.CreateClient();
//            _blobServiceClient = new BlobServiceClient(configuration["AzureBlobStorage:ConnectionString"]);
//            _containerName = configuration["AzureBlobStorage:ContainerName"];
//        }

//        [HttpGet]
//        public async Task<IActionResult> DownloadAndUploadFile()
//        {
//            try
//            {
//                // Dosya indirme işlemi
//                var downloadStopwatch = Stopwatch.StartNew();
//                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
//                var response = await _httpClient.GetAsync(TestFileUrl, HttpCompletionOption.ResponseHeadersRead);
//                downloadStopwatch.Stop();

//                if (!response.IsSuccessStatusCode)
//                {
//                    _logger.LogWarning("Failed to download file. Status code: {StatusCode}", response.StatusCode);
//                    return StatusCode((int)response.StatusCode, "Failed to download the file.");
//                }

//                var contentLength = response.Content.Headers.ContentLength ?? 0;
//                var downloadTimeInSeconds = downloadStopwatch.Elapsed.TotalSeconds;
//                var downloadSpeedInMbps = (contentLength * 8) / (downloadTimeInSeconds * 1_000_000);

//                //// Dosyayı Azure Blob Storage'a yükleme
//                //var uploadStopwatch = Stopwatch.StartNew();
//                //var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
//                //var blobClient = containerClient.GetBlobClient("downloaded_file.zip");

//                //await using (var stream = await response.Content.ReadAsStreamAsync())
//                //{
//                //    await blobClient.UploadAsync(stream, true);
//                //}
//                //uploadStopwatch.Stop();
//                //var uploadTimeInSeconds = uploadStopwatch.Elapsed.TotalSeconds;
//                //var uploadSpeedInMbps = (contentLength * 8) / (uploadTimeInSeconds * 1_000_000);

//                return Ok(new
//                {
//                    DownloadSpeed = downloadSpeedInMbps + " Mbps",
//                    //UploadSpeed = uploadSpeedInMbps + " Mbps"
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred during file download and upload.");
//                return StatusCode(500, "An internal error occurred");
//            }
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KaracaBesik_project1.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    //public class DownloadController : ControllerBase
    //{
    //    private readonly ILogger<DownloadController> _logger;
    //    private readonly HttpClient _httpClient;
    //    private const string TestFileUrl = "http://ipv4.download.thinkbroadband.com/50MB.zip";

    //    public DownloadController(ILogger<DownloadController> logger, IHttpClientFactory httpClientFactory)
    //    {
    //        _logger = logger;
    //        _httpClient = httpClientFactory.CreateClient();
    //    }

    //    [HttpGet]
    //    public async Task<IActionResult> DownloadFile()
    //    {
    //        try
    //        {
    //            _logger.LogInformation("Download test started at {Time}", DateTime.UtcNow);
    //            var stopwatch = new System.Diagnostics.Stopwatch();
    //            stopwatch.Start();

    //            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
    //            var response = await _httpClient.GetAsync(TestFileUrl, HttpCompletionOption.ResponseHeadersRead);

    //            _logger.LogInformation("Response status code: {StatusCode}", response.StatusCode);

    //            if (!response.IsSuccessStatusCode)
    //            {
    //                _logger.LogWarning("Non-success status code received.");
    //                return StatusCode((int)response.StatusCode, "Failed to download the file.");
    //            }

    //            var fileSizeInBytes = response.Content.Headers.ContentLength ?? 0;
    //            stopwatch.Stop();
    //            var downloadTimeInSeconds = stopwatch.Elapsed.TotalSeconds;
    //            var speedInMbps = (fileSizeInBytes / downloadTimeInSeconds) * 8 / 1_000_000; // Convert bytes per second to Mbps

    //            _logger.LogInformation("Download test completed in {DownloadTimeInSeconds} seconds with a speed of {SpeedInMbps} Mbps", downloadTimeInSeconds, speedInMbps);

    //            // Instead of returning the file, return the download speed
    //            return Ok(new { DownloadSpeedInMbps = speedInMbps });
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error occurred while downloading the test file.");
    //            return StatusCode(500, "An internal error occurred");
    //        }
    //    }
    //}


    [ApiController]
    [Route("[controller]")]
    public class DownloadController : ControllerBase
    {
        private const int FileSize = 50 * 1024 * 1024; // 50 MB

        [HttpGet("DownloadFile")]
        public IActionResult Download()
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://link.testfile.org/300MB");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Dosya indirilemedi.");
            }

            var content = await response.Content.ReadAsByteArrayAsync();
            return File(content, "application/octet-stream", "downloadedfile.zip");
        }
    }

}