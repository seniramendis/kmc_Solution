using kmc.Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Net.Http.Json;
using System.Net.Http.Headers;

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

        // HELPER METHOD: Attaches the VIP Wristband (Token) to API requests
        private HttpClient CreateAuthenticatedClient()
        {
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Request.Cookies["JWToken"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        // GET: CityActivities
        public async Task<IActionResult> Index()
        {
            var client = CreateAuthenticatedClient(); // Uses new helper!
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
        public IActionResult Create() { return View(); }

        // POST: CityActivities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CityActivityViewModel activity)
        {
            if (ModelState.IsValid)
            {
                var client = CreateAuthenticatedClient(); // Uses new helper!
                var baseUrl = _configuration["ApiSettings:BaseUrl"];

                var response = await client.PostAsJsonAsync($"{baseUrl}/api/CityActivities", activity);
                if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            }
            return View(activity);
        }

        // GET: CityActivities/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateAuthenticatedClient(); // Uses new helper!
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
                var client = CreateAuthenticatedClient(); // Uses new helper!
                var baseUrl = _configuration["ApiSettings:BaseUrl"];
                var response = await client.PutAsJsonAsync($"{baseUrl}/api/CityActivities/{id}", activity);

                if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                else ModelState.AddModelError(string.Empty, "Update failed. Make sure you are the original Organizer.");
            }
            return View(activity);
        }

        // GET: CityActivities/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var client = CreateAuthenticatedClient(); // Uses new helper!
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
            var client = CreateAuthenticatedClient(); // Uses new helper!
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var response = await client.DeleteAsync($"{baseUrl}/api/CityActivities/{id}");
            if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            return BadRequest("Delete failed.");
        }

        // GET: CityActivities/Book/5
        public IActionResult Book(int id)
        {
            var booking = new EventBookingViewModel { ActivityId = id };
            return View(booking);
        }

        // POST: CityActivities/Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(EventBookingViewModel booking)
        {
            if (ModelState.IsValid)
            {
                var client = CreateAuthenticatedClient(); // Uses new helper!
                var baseUrl = _configuration["ApiSettings:BaseUrl"];

                var response = await client.PostAsJsonAsync($"{baseUrl}/api/EventBookings", booking);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Your booking was successfully confirmed!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }
            return View(booking);
        }
    }
}