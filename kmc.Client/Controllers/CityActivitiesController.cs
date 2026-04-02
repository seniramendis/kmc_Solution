using kmc.Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace kmc.Client.Controllers
{
    public class CityActivitiesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CityActivitiesController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // GET: CityActivities
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];

            // This calls your API to get the data
            var response = await client.GetAsync($"{baseUrl}/api/CityActivities");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var activities = JsonSerializer.Deserialize<List<CityActivityViewModel>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(activities);
            }

            // If the API call fails, return an empty list so the page doesn't crash
            return View(new List<CityActivityViewModel>());
        }
    }
}