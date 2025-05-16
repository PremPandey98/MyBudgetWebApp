using BudgetMobApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BudgetMobApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login() => View();
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (model.Username == null || model.Password == null)
                return View(model);

            if ((model.Username == "prem123" && model.Password == "pass@4") ||
                (model.Username == "Test@123" && model.Password == "Test@123"))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                TempData["SuccessMessage"] = $"👋 Welcome to BudgetApp\n🙎 Username: {model.Username}";

                return RedirectToAction("Dashboard", "Home");
            }

            TempData["ErrorMessage"] = "Invalid Credentials.";
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login", "Login");
        }

        public IActionResult AccessDenied() => View();
    }
}
