using kmc.Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace kmc.Client.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AuthController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // GET: Auth/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];

            // Calls the API to create the user
            var response = await client.PostAsJsonAsync($"{baseUrl}/api/auth/register", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Registration successful! You can now log in.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, "Registration failed. Email might already be taken or password isn't strong enough (Needs 1 uppercase, 1 number, 1 special character).");
            return View(model);
        }

        // GET: Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];

            // Calls the API to check the password
            var response = await client.PostAsJsonAsync($"{baseUrl}/api/auth/login", model);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonString);
                var token = doc.RootElement.GetProperty("token").GetString();

                // MAGIC HAPPENS HERE: We save the token in a secure browser cookie!
                Response.Cookies.Append("JWToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt. Check your email and password.");
            return View(model);
        }

        // GET: Auth/Logout
        public IActionResult Logout()
        {
            // Deletes the cookie to log them out
            Response.Cookies.Delete("JWToken");
            return RedirectToAction("Index", "Home");
        }
    }
}