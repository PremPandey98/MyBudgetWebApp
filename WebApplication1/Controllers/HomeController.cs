using System.Diagnostics;
using BudgetMobApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly SmsService _smsService;

    public HomeController(ILogger<HomeController> logger, AppDbContext context, SmsService smsService)
    {
        _logger = logger;
        _context = context;
        _smsService = smsService;
    }
    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Budget()
    {
        var budget = _context.BudgetDeposits.ToList(); // Assuming you will use _context here for some logic
        return View(budget);
    }
    public IActionResult BudgetCreate()
    {
        return View();
    }
    [HttpPost]
    public IActionResult BudgetCreate(BudgetDeposit _BudgetDeposit)
    {
        if (ModelState.IsValid)
        {
            try
            {
                //_BudgetDeposit.DepositDate = DateTime.Now;

                _context.BudgetDeposits.Add(_BudgetDeposit);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Amount Deposited successfully."; // Success message
                return RedirectToAction("Budget");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error creating Deposit Amount: {ex.Message}"; // Error message
                return View(_BudgetDeposit);
            }
        }
        TempData["ErrorMessage"] = "Invalid model state, please check the input."; // Error message for invalid model
        return View(_BudgetDeposit);
    }

    public IActionResult BudgetDelete(int id)
    {
        var budget = _context.BudgetDeposits.Find(id);
        if (budget == null)
        {
            return NotFound();
        }
        return View(budget);
    }
   
    [HttpPost, ActionName("BudgetDelete")]
    public IActionResult BudgetDeleteConfirmed(int id)
    {
        var budget = _context.BudgetDeposits.Find(id);
        if (budget == null)
        {
            TempData["ErrorMessage"] = "Deposit Amount not found."; // Error message if not found
            return RedirectToAction("Budget");
        }

        try
        {
            _context.BudgetDeposits.Remove(budget);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Deposit Amount deleted successfully."; // Success message
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error deleting Deposit Amount: {ex.Message}"; // Error message in case of exception
        }

        return RedirectToAction("Budget");
    }




    public IActionResult BudgetUsage()
    {
        var budget = _context.BudgetUsages.ToList();
        return View(budget);
    }
    public IActionResult BudgetUsageCreate()
    {
        return View();
    }
    [HttpPost]
    public IActionResult BudgetUsageCreate(BudgetUsage _BudgetUsage)
    {
        if (ModelState.IsValid)
        {
            _context.BudgetUsages.Add(_BudgetUsage);
            _context.SaveChanges();

            //var smsService = new SmsService(_configuration);
            var phoneNumbers = new List<string> { "9337713798", "8459408758" };
            string message = $"New expense added: {_BudgetUsage.UsageDescription}, Amount: {_BudgetUsage.AmountUsed} ";
            _smsService.SendSms(phoneNumbers, message);

            // Set success message
            TempData["SuccessMessage"] = "Expense Added successfully!";
            return RedirectToAction("BudgetUsage");
        }
        else
        {
            // Set failure message
            TempData["ErrorMessage"] = "Failed to Expense. Please try again.";
        }

        return View(_BudgetUsage);
    }

    public IActionResult BudgetUsageDelete(int id)
    {
        var budget = _context.BudgetUsages.Find(id);
        if (budget == null)
        {
            return NotFound();
        }
        return View(budget);
    }
    [HttpPost, ActionName("BudgetUsageDelete")]
    public IActionResult BudgetUsageDeleteConfirmed(int id)
    {
        var budget = _context.BudgetUsages.Find(id);
        if (budget == null)
        {
            TempData["ErrorMessage"] = "Expense not found."; // Error message if not found
            return RedirectToAction("BudgetUsage");
        }

        try
        {
            _context.BudgetUsages.Remove(budget);
            _context.SaveChanges();

            //var smsService = new SmsService(_configuration);
            var phoneNumbers = new List<string> { "9337713798", "8459408758" };
            string message = $"Expense Deleted: {budget.UsageDescription}, Amount: {budget.AmountUsed} ";
            _smsService.SendSms(phoneNumbers, message);

            TempData["SuccessMessage"] = "Expense deleted successfully."; // Success message
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error deleting Expense: {ex.Message}"; // Error message in case of exception
        }

        return RedirectToAction("BudgetUsage");
    }



    public async Task<IActionResult> Dashboard(DateTime? startDate, DateTime? endDate)
    {
        // Convert to IST
        var istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        var istNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istTimeZone);

        if (!startDate.HasValue)
        {
            var firstDepositThisMonth = await _context.BudgetDeposits
                .Where(d => d.DepositDate.Month == istNow.Month &&
                            d.DepositDate.Year == istNow.Year)
                .OrderBy(d => d.DepositDate)
                .Select(d => d.DepositDate)
                .FirstOrDefaultAsync();

            var firstUsageThisMonth = await _context.BudgetUsages
                .Where(u => u.DateUsed.Month == istNow.Month &&
                            u.DateUsed.Year == istNow.Year)
                .OrderBy(u => u.DateUsed)
                .Select(u => u.DateUsed)
                .FirstOrDefaultAsync();

            if (firstDepositThisMonth != default && firstUsageThisMonth != default)
            {
                startDate = (firstDepositThisMonth < firstUsageThisMonth)
                    ? firstDepositThisMonth.Date
                    : firstUsageThisMonth.Date;
            }
            else if (firstDepositThisMonth != default)
            {
                startDate = firstDepositThisMonth.Date;
            }
            else if (firstUsageThisMonth != default)
            {
                startDate = firstUsageThisMonth.Date;
            }
            else
            {
                startDate = istNow.AddMonths(-1);
            }
        }

        // Set endDate to IST now if not provided
        endDate ??= istNow;

        var startDateOnly = DateOnly.FromDateTime(startDate.Value);
        var endDateOnly = DateOnly.FromDateTime(endDate.Value);

        var budgetUsages = await _context.BudgetUsages
            .Where(u => DateOnly.FromDateTime(u.DateUsed) >= startDateOnly &&
                        DateOnly.FromDateTime(u.DateUsed) <= endDateOnly)
            .ToListAsync();

        var budgetDeposits = await _context.BudgetDeposits
            .Where(d => DateOnly.FromDateTime(d.DepositDate) >= startDateOnly &&
                        DateOnly.FromDateTime(d.DepositDate) <= endDateOnly)
            .ToListAsync();

        var model = new DashboardViewModel
        {
            BudgetUsages = budgetUsages,
            BudgetDeposits = budgetDeposits,
            StartDate = startDate.Value,
            EndDate = endDate.Value
        };

        return View(model);
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
