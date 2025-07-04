using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace am_archives_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MangaController : ControllerBase
    {
        private static readonly HttpClient httpClient = new HttpClient();

        [HttpGet("popular-new-titles")]
        public async Task<IActionResult> GetPopularNewTitles()
        {
            var url = "https://api.mangadex.org/manga?includes[]=cover_art&includes[]=artist&includes[]=author&order[followedCount]=desc&contentRating[]=safe&contentRating[]=suggestive&hasAvailableChapters=true";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "am-archives-backend/1.0 (contact@example.com)");

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return Content(content, "application/json");
        }

        [HttpGet("latest-updates")]
        public async Task<IActionResult> GetLatestUpdates([FromQuery] int limit, [FromQuery] int? offset)
        {
            int actualOffset = offset ?? 0;

            var url = $"https://api.mangadex.org/chapter?limit={limit}&offset={actualOffset}&includes[]=user&includes[]=scanlation_group&includes[]=manga&contentRating[]=safe&contentRating[]=suggestive&order[readableAt]=desc";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "am-archives-backend/1.0 (contact@example.com)");

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return Content(content, "application/json");
        }

        [HttpGet("latest-manga-covers")]
        public async Task<IActionResult> GetLatestMangaCovers([FromQuery] int limit, [FromQuery] int? offset)
        {
            int actualOffset = offset ?? 0;

            var url = $"https://api.mangadex.org/chapter?limit={limit}&offset={offset}&includes[]=user&includes[]=scanlation_group&includes[]=manga&contentRating[]=safe&contentRating[]=suggestive&order[readableAt]=desc";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "am-archives-backend/1.0 (contact@example.com)");

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return Content(content, "application/json");
        }

        [HttpGet("latest-manga-cover-image-details")]
        public async Task<IActionResult> GetLatestMangaCoverImageDetails([FromQuery] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("id is required.");

            var url = $"https://api.mangadex.org/manga?{id}&limit=100&includes[]=author&contentRating[]=safe&contentRating[]=suggestive&contentRating[]=erotica&contentRating[]=pornographic&includes[]=cover_art";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "am-archives-backend/1.0 (contact@example.com)");

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return Content(content, "application/json");
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetMangaDetailsById([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("id is required.");

            var url = $"https://api.mangadex.org/manga/{id}?includes[]=artist&includes[]=author&includes[]=cover_art";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "am-archives-backend/1.0 (contact@example.com)");

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return Content(content, "application/json");
        }

        [HttpGet("chapters/{mangaId}")]
        public async Task<IActionResult> GetMangaChaptersList([FromRoute] string mangaId, [FromQuery] int? offset)
        {
            if (string.IsNullOrWhiteSpace(mangaId))
                return BadRequest("mangaId is required.");

            int actualOffset = offset ?? 0;

            var url = $"https://api.mangadex.org/manga/{mangaId}/feed?limit=96&includes[]=scanlation_group&includes[]=user&order[volume]=asc&order[chapter]=asc&offset={actualOffset}&contentRating[]=safe&contentRating[]=suggestive&contentRating[]=erotica";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "am-archives-backend/1.0 (contact@example.com)");

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return Content(content, "application/json");
        }

        [HttpGet("group/{mangaId}")]
        public async Task<IActionResult> GetMangaGroup(
            [FromRoute] string mangaId,
            [FromQuery] string translatedLanguage,
            [FromQuery] string groups)
        {
            if (string.IsNullOrWhiteSpace(mangaId))
                return BadRequest("mangaId is required.");
            if (string.IsNullOrWhiteSpace(translatedLanguage))
                return BadRequest("translatedLanguage is required.");
            if (string.IsNullOrWhiteSpace(groups))
                return BadRequest("groups is required.");

            var url = $"https://api.mangadex.org/manga/{mangaId}/aggregate?translatedLanguage[]={translatedLanguage}&groups[]={groups}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "am-archives-backend/1.0 (contact@example.com)");

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return Content(content, "application/json");
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetSearchMangaResults([FromQuery] string searchQuery, [FromQuery] int? offset)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
                return BadRequest("searchQuery is required.");

            int actualOffset = offset ?? 0;

            // URL encode the search query to ensure it is safe for use in a URL
            var encodedQuery = Uri.EscapeDataString(searchQuery);

            var url = $"https://api.mangadex.org/manga?limit=32&offset={actualOffset}&includes[]=cover_art&contentRating[]=safe&contentRating[]=suggestive&contentRating[]=erotica&title={encodedQuery}&includedTagsMode=AND&excludedTagsMode=OR";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "am-archives-backend/1.0 (contact@example.com)");

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return Content(content, "application/json");
        }

        [HttpGet("tags")]
        public async Task<IActionResult> GetMangaTag()
        {
            var url = "https://api.mangadex.org/manga/tag";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "am-archives-backend/1.0 (contact@example.com)");

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return Content(content, "application/json");
        }

        [HttpGet("by-tag")]
        public async Task<IActionResult> GetMangaDetailsByTag([FromQuery] string tagId, [FromQuery] int? offset)
        {
            if (string.IsNullOrWhiteSpace(tagId))
                return BadRequest("tagId is required.");

            int actualOffset = offset ?? 0;

            var url = $"https://api.mangadex.org/manga?limit=32&offset={actualOffset}&includes[]=cover_art&contentRating[]=safe&contentRating[]=suggestive&contentRating[]=erotica&includedTags[]={tagId}&order[followedCount]=desc";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "am-archives-backend/1.0 (contact@example.com)");

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return Content(content, "application/json");
        }
    }
}
