using Microsoft.AspNetCore.Mvc;
using TaskManger.Models;
using TaskManger.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace TaskManger.Controllers
{
    public class AuthController : Controller
    {
        private readonly TaskManagerContext _context;
        private readonly AuthService _authService;

        public AuthController(TaskManagerContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var token = await _authService.LoginAsync(email, password);
            if (token == null)
            {
                ViewBag.Error = "Invalid credentials";
                return View();
            }

            // Store JWT in a secure cookie
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return RedirectToAction("Index", "Task");
        }


        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ViewBag.Error = "Email already registered";
                return View();
            }

            var user = new User
            {
                Email = model.Email,
                Username = model.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Login");
        }

    }
}
