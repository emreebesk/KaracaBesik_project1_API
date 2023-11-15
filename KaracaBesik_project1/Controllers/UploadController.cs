//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.IO;
//using System.Threading.Tasks;

//namespace KaracaBesik_project1.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class UploadController : ControllerBase
//    {
//        private readonly ILogger<UploadController> _logger;

//        public UploadController(ILogger<UploadController> logger)
//        {
//            _logger = logger;
//        }

//        // POST: api/upload
//        [HttpPost]
//        public async Task<IActionResult> UploadFile(IFormFile file)
//        {
//            if (file == null || file.Length == 0)
//            {
//                return BadRequest("No file uploaded.");
//            }

//            try
//            {
//                _logger.LogInformation("Received file: {FileName}, Size: {FileSize} bytes", file.FileName, file.Length);

//                // You can process the file here as needed for your application
//                // For now, we'll just read and discard the stream to simulate the upload
//                using (var stream = file.OpenReadStream())
//                {
//                    await stream.CopyToAsync(Stream.Null);
//                }

//                return Ok(new { FileName = file.FileName, Size = file.Length });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred during file upload.");
//                return StatusCode(500, "An internal error occurred");
//            }
//        }
//    }
//}


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace KaracaBesik_project1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly ILogger<UploadController> _logger;

        public UploadController(ILogger<UploadController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                _logger.LogInformation("File upload started at {Time}", DateTime.UtcNow);
                var stopwatch = Stopwatch.StartNew();

                if (file != null && file.Length > 0)
                {
                    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                    Directory.CreateDirectory(uploadsDirectory); // Create the 'uploads' directory if it doesn't exist

                    var fileName = $"uploaded_{DateTime.Now.Ticks}.zip";
                    var filePath = Path.Combine(uploadsDirectory, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    stopwatch.Stop();
                    var uploadTimeInSeconds = stopwatch.Elapsed.TotalSeconds;
                    var fileSizeInBytes = file.Length;
                    var speedInMbps = (fileSizeInBytes * 8) / (uploadTimeInSeconds * 1_000_000);
                    
                    _logger.LogInformation("File uploaded successfully: {FileName}", fileName);
                    _logger.LogInformation("Upload time: {UploadTimeInSeconds} seconds", uploadTimeInSeconds);
                    _logger.LogInformation("Upload speed: {SpeedInMbps} Mbps", speedInMbps);

                    return Ok(new
                    {
                        Message = "File uploaded successfully.",
                        FileName = fileName,
                        UploadTimeInSeconds = uploadTimeInSeconds,
                        UploadSpeedMbps = speedInMbps
                    });
                }
                else
                {
                    _logger.LogError("No file was received in the request.");
                    return BadRequest("No file was received.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while uploading the file.");
                return StatusCode(500, "An internal error occurred");
            }
        }

    }
}
