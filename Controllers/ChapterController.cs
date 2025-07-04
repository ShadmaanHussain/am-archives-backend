using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace am_archives_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        private readonly ILogger<ChapterController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public ChapterController(ILogger<ChapterController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetChapterDetailsById(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var url = $"https://api.mangadex.org/chapter/{id}?includes[]=scanlation_group&includes[]=manga&includes[]=user";
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("User-Agent", "am-archives-backend/1.0 (contact@example.com)");

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new { Message = "Failed to fetch chapter details from MangaDex", Details = errorContent });
            }

            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        [HttpGet("feed/{id}")]
        public async Task<IActionResult> GetChapterFeedById(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var url = $"https://api.mangadex.org/at-home/server/{id}?forcePort443=false";
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("User-Agent", "am-archives-backend/1.0 (contact@example.com)");

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new { Message = "Failed to fetch chapter feed from MangaDex", Details = errorContent });
            }

            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
    }
}
