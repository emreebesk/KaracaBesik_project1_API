using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KaracaBesik_project1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        private readonly ILogger<PingController> _logger;

        public PingController(ILogger<PingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Ping()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                // Bu kısımda herhangi bir işlem yapmanıza gerek yok.
                // Ping isteği, sunucunun çalışır durumda olduğunu doğrulamak için yeterlidir.

                stopwatch.Stop();

                var response = new
                {
                    Message = "PingPong",
                    Timestamp = DateTime.UtcNow,
                    Latency = stopwatch.ElapsedMilliseconds // Ping süresi milisaniye cinsinden
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Error occurred while processing ping request.");
                return StatusCode(500, new { Latency = stopwatch.ElapsedMilliseconds });
            }
        }
    }
}
