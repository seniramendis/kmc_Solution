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

        public async Task<IActionResult> Index()
        {
            var client = CreateAuthenticatedClient();
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

        public IActionResult Create() { return View(); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CityActivityViewModel activity)
        {
            if (ModelState.IsValid)
            {
                var client = CreateAuthenticatedClient();
                var baseUrl = _configuration["ApiSettings:BaseUrl"];

                var response = await client.PostAsJsonAsync($"{baseUrl}/api/CityActivities", activity);
                if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            }
            return View(activity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateAuthenticatedClient();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CityActivityViewModel activity)
        {
            if (id != activity.ActivityId) return BadRequest("ID mismatch");

            if (ModelState.IsValid)
            {
                var client = CreateAuthenticatedClient();
                var baseUrl = _configuration["ApiSettings:BaseUrl"];
                var response = await client.PutAsJsonAsync($"{baseUrl}/api/CityActivities/{id}", activity);

                if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                else ModelState.AddModelError(string.Empty, "Update failed. Make sure you are the original Organizer.");
            }
            return View(activity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = CreateAuthenticatedClient();
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = CreateAuthenticatedClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var response = await client.DeleteAsync($"{baseUrl}/api/CityActivities/{id}");
            if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            return BadRequest("Delete failed.");
        }

        public IActionResult Book(int id)
        {
            var booking = new EventBookingViewModel { ActivityId = id };
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(EventBookingViewModel booking)
        {
            if (ModelState.IsValid)
            {
                var client = CreateAuthenticatedClient();
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

        public async Task<IActionResult> MyBookings()
        {
            var client = CreateAuthenticatedClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];

            var response = await client.GetAsync($"{baseUrl}/api/EventBookings/MyBookings");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var bookings = JsonSerializer.Deserialize<List<EventBookingViewModel>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(bookings);
            }

            return View(new List<EventBookingViewModel>());
        }

        public async Task<IActionResult> Attendees(int id)
        {
            var client = CreateAuthenticatedClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];

            var response = await client.GetAsync($"{baseUrl}/api/EventBookings/Activity/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var bookings = JsonSerializer.Deserialize<List<EventBookingViewModel>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(bookings);
            }

            return View(new List<EventBookingViewModel>());
        }

        public async Task<IActionResult> Dashboard()
        {
            var client = CreateAuthenticatedClient();
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

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var client = CreateAuthenticatedClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];

            var response = await client.DeleteAsync($"{baseUrl}/api/EventBookings/{bookingId}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Your ticket has been successfully cancelled.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to cancel the ticket. It may have already been removed.";
            }

            return RedirectToAction(nameof(MyBookings));
        }
    }
}