using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace KaracaBesik_project1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DownloadController : ControllerBase
    {
        private readonly ILogger<DownloadController> _logger;

        public DownloadController(ILogger<DownloadController> logger)
        {
            _logger = logger;
        }

        [HttpGet("download")]
        public IActionResult TestDownloadSpeed()
        {
            try
            {
                // 50 MB boyutunda rastgele veri üretimi
                var dataLength = 50 * 1024 * 1024; // 50 MB
                var randomData = new byte[dataLength];
                new Random().NextBytes(randomData);

                // Verileri byte array olarak gönder
                return File(randomData, "application/octet-stream", "testfile.dat");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during generating test data.");
                return StatusCode(500, "An internal error occurred");
            }
        }
    }
}
