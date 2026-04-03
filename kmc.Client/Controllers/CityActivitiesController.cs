using kmc.Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Net.Http.Json;

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

            var response = await client.GetAsync($"{baseUrl}/api/CityActivities");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var activities = JsonSerializer.Deserialize<List<CityActivityViewModel>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(activities);
            }

            return View(new List<CityActivityViewModel>());
        }

        // GET: CityActivities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CityActivities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CityActivityViewModel activity)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                var baseUrl = _configuration["ApiSettings:BaseUrl"];

                var response = await client.PostAsJsonAsync($"{baseUrl}/api/CityActivities", activity);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(activity);
        }

        // GET: CityActivities/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];

            var response = await client.GetAsync($"{baseUrl}/api/CityActivities/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var activity = JsonSerializer.Deserialize<CityActivityViewModel>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(activity);
            }
            return NotFound();
        }

        // POST: CityActivities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CityActivityViewModel activity)
        {
            if (id != activity.ActivityId) return BadRequest("ID mismatch");

            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                var baseUrl = _configuration["ApiSettings:BaseUrl"];

                var response = await client.PutAsJsonAsync($"{baseUrl}/api/CityActivities/{id}", activity);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Update failed. Make sure you are the original Organizer.");
                }
            }
            return View(activity);
        }

        // GET: CityActivities/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];

            var response = await client.GetAsync($"{baseUrl}/api/CityActivities/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var activity = JsonSerializer.Deserialize<CityActivityViewModel>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(activity);
            }
            return NotFound();
        }

        // POST: CityActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];

            // This sends the DELETE request to your API!
            var response = await client.DeleteAsync($"{baseUrl}/api/CityActivities/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return BadRequest("Delete failed.");
        }
    }
}