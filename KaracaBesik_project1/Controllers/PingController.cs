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
        public Task<IActionResult> PingTest()
        {
           
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;

            string target = "www.google.com";
            PingReply reply = pingSender.Send(target, timeout, buffer, options);

            var response = new
            {
                Message = "Your Ping Approximately",
                Ping = reply.RoundtripTime
            };

            return Task.FromResult<IActionResult>(Ok(response));

        }
    }
}
