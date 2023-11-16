using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Text;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingTestController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PingTestController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("PingTest")]
        public async Task<IActionResult> PingTest()
        {
            string target = "https://www.example.com";
            var client = _httpClientFactory.CreateClient();
            var stopwatch = new Stopwatch();

            try
            {
                stopwatch.Start();
                var response = await client.GetAsync(target);
                stopwatch.Stop();

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest($"Unable to reach {target}.");
                }

                var responseTime = stopwatch.ElapsedMilliseconds;
                return Ok(new { Target = target, ResponseTimeMs = responseTime });
            }
            catch (HttpRequestException)
            {
                stopwatch.Stop();
                return StatusCode(500, $"Error occurred while trying to reach {target}.");
            }
        }
    }
}
