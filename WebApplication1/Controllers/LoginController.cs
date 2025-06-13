        
using BudgetMobApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BudgetMobApp.Controllers
{

    public class LoginController : Controller
    {
        private readonly WebApplication1.Data.AppDbContext _context;
        public LoginController(WebApplication1.Data.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(BudgetMobApp.Models.LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                return View(model);

            // Find user by username
            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);

            if (user != null && user.Password == model.Password) // TODO: Use hashed passwords in production
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username ?? "")
                };

                var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Add Remember Me functionality
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe
                };

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);

                TempData["SuccessMessage"] = $"👋 Welcome to BudgetApp\n🙎 Username: {user.Username}";
                return RedirectToAction("Dashboard", "Home");
            }

            TempData["ErrorMessage"] = "Invalid Credentials.";
            return View(model);
        }

        public IActionResult Register()
        {
            ViewBag.Groups = _context.Groups.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Register(BudgetMobApp.Models.User model)
        {
            ViewBag.Groups = _context.Groups.ToList();
            var form = Request.Form;
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                TempData["ErrorMessage"] = "Username and Password are required.";
                return View(model);
            }

            if (_context.Users.Any(u => u.Username == model.Username))
            {
                TempData["ErrorMessage"] = "Username already exists.";
                return View(model);
            }

            if (model.AccountType == AccountType.Group)
            {
                var groupAction = form["GroupAction"].ToString();
                if (groupAction == "existing")
                {
                    // User selected an existing group
                    if (model.GroupId == null)
                    {
                        TempData["ErrorMessage"] = "Please select a group.";
                        return View(model);
                    }
                    var groupCodeInput = form["GroupCodeInput"].ToString();
                    var group = _context.Groups.FirstOrDefault(g => g.Id == model.GroupId);
                    if (group == null || group.GroupCode != groupCodeInput)
                    {
                        TempData["ErrorMessage"] = "Invalid group code for the selected group.";
                        return View(model);
                    }
                }
                else if (groupAction == "new")
                {
                    // User wants to create a new group
                    var newGroupName = form["NewGroupName"].ToString();
                    var newGroupCode = form["NewGroupCode"].ToString();
                    if (string.IsNullOrWhiteSpace(newGroupName) || string.IsNullOrWhiteSpace(newGroupCode))
                    {
                        TempData["ErrorMessage"] = "Group name and code are required to create a new group.";
                        return View(model);
                    }
                    // Check if group name or code already exists
                    if (_context.Groups.Any(g => g.GroupName == newGroupName || g.GroupCode == newGroupCode))
                    {
                        TempData["ErrorMessage"] = "Group name or code already exists.";
                        return View(model);
                    }
                    var newGroup = new Group { GroupName = newGroupName, GroupCode = newGroupCode };
                    _context.Groups.Add(newGroup);
                    _context.SaveChanges();
                    model.GroupId = newGroup.Id;
                }
                else
                {
                    TempData["ErrorMessage"] = "Please select group action (existing or new).";
                    return View(model);
                }
            }

            // TODO: Hash password before saving in production
            _context.Users.Add(model);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Registration successful. Please log in.";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login", "Login");
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["ErrorMessage"] = "Please enter your email.";
                return View();
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null && !string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                // Generate a 6-digit OTP
                var otp = new Random().Next(100000, 999999).ToString();
                // Store OTP in TempData for demo (use DB or cache in production)
                TempData["ResetOtp"] = otp;
                TempData["ResetEmail"] = email;

                // Send OTP via Twilio
                var smsService = new BudgetMobApp.Services.SmsService(this.HttpContext.RequestServices.GetService(typeof(Microsoft.Extensions.Configuration.IConfiguration)) as Microsoft.Extensions.Configuration.IConfiguration);
                smsService.SendSms(new List<string> { user.PhoneNumber }, $"Your BudgetApp password reset OTP is: {otp}");

                TempData["SuccessMessage"] = "An OTP has been sent to your registered mobile number.";
                return RedirectToAction("ChangePasswordWithOtp");
            }
            else
            {
                // Always show success message for security
                TempData["SuccessMessage"] = "If an account with that email exists, an OTP has been sent to the registered mobile number.";
                return RedirectToAction("ChangePasswordWithOtp");
            }
        }

        [HttpGet]
        public IActionResult ChangePasswordWithOtp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePasswordWithOtp(string otp, string newPassword, string confirmPassword)
        {
            var expectedOtp = TempData["ResetOtp"] as string;
            var email = TempData["ResetEmail"] as string;

            if (string.IsNullOrWhiteSpace(otp) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                TempData["ErrorMessage"] = "All fields are required.";
                return View();
            }
            if (newPassword != confirmPassword)
            {
                TempData["ErrorMessage"] = "Passwords do not match.";
                return View();
            }
            if (expectedOtp == null || email == null || otp != expectedOtp)
            {
                TempData["ErrorMessage"] = "Invalid or expired OTP.";
                return View();
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return View();
            }
            user.Password = newPassword; // In production, hash the password!
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Password changed successfully. Please log in.";
            return RedirectToAction("Login");
        }



        public IActionResult AccessDenied() => View();
    }
}
