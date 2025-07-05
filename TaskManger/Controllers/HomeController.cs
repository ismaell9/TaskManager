using Microsoft.AspNetCore.Mvc;
using TaskManger.Services;

public class HomeController : Controller
{
    private readonly AuthService _authService;

    public HomeController(AuthService authService)
    {
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
            ViewBag.Error = "Invalid login credentials";
            return View();
        }

        ViewBag.Token = token;
        return View("LoginSuccess"); 
    }


    public IActionResult Logout()
    {
        return RedirectToAction("Login", "Auth");
    }

}
