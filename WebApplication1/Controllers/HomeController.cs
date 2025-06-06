using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using BudgetMobApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using WebApplication1.Data;
using LicenseContext = OfficeOpenXml.LicenseContext;
using System.Drawing;
using WebApplication1.Models;

namespace BudgetMobApp.Controllers;

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

    public ActionResult ExportBudgetUsageToExcel()
    {

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var BudgetusagesData = _context.BudgetUsages.ToList();
        var DepositData = _context.BudgetDeposits.ToList();

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Budget Usage");

            // Get all properties of BudgetUsage using reflection
            var properties = typeof(BudgetUsage).GetProperties();

            // Add header row dynamically
            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = properties[i].Name;
            }

            // Format header
            using (var range = worksheet.Cells[1, 1, 1, properties.Length])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            }

            // Add data rows dynamically
            for (int row = 0; row < BudgetusagesData.Count; row++)
            {
                for (int col = 0; col < properties.Length; col++)
                {
                    var value = properties[col].GetValue(BudgetusagesData[row], null);
                    worksheet.Cells[row + 2, col + 1].Value = value;

                    // Optional: Format specific data types
                    if (value is DateTime)
                    {
                        worksheet.Cells[row + 2, col + 1].Style.Numberformat.Format = "yyyy-mm-dd";
                    }
                    else if (value is decimal)
                    {
                        worksheet.Cells[row + 2, col + 1].Style.Numberformat.Format = "#,##0.00";
                    }
                }
            }

            int nextStartRow = BudgetusagesData.Count + 4;


            var DepositProperties = typeof(BudgetDeposit).GetProperties();

            for (int i = 0; i < DepositProperties.Length; i++)
            {
                worksheet.Cells[nextStartRow, i + 1].Value = DepositProperties[i].Name;
            }

            using (var range = worksheet.Cells[nextStartRow, 1, nextStartRow, DepositProperties.Length])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
            }

            for (int row = 0; row < DepositData.Count; row++)
            {
                for (int col = 0; col < DepositProperties.Length; col++)
                {
                    var value = DepositProperties[col].GetValue(DepositData[row]);
                    worksheet.Cells[nextStartRow + row + 1, col + 1].Value = value;

                    if (value is DateTime)
                        worksheet.Cells[nextStartRow + row + 1, col + 1].Style.Numberformat.Format = "yyyy-mm-dd";
                    else if (value is decimal)
                        worksheet.Cells[nextStartRow + row + 1, col + 1].Style.Numberformat.Format = "#,##0.00";
                }
            }

            // Auto-fit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            return File(package.GetAsByteArray(),
                      "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                      "BudgetUsage_Export.xlsx");
        }
    }


    [HttpPost]
    public async Task<IActionResult> ImportBudgetUsage(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select a valid Excel file.";
                return RedirectToAction("Index");
            }

            // ✅ Set EPPlus license context (for non-commercial use)
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        TempData["ErrorMessage"] = "No worksheet found in the Excel file.";
                        return RedirectToAction("Index");
                    }

                    var allowedProperties = new[] { "AmountUsed", "DateUsed", "UsageDescription" };
                    var entityProperties = typeof(BudgetUsage).GetProperties()
                        .Where(p => p.CanWrite && allowedProperties.Contains(p.Name))
                        .ToList();

                    // Read headers
                    var excelHeaders = new List<string>();
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        excelHeaders.Add(worksheet.Cells[1, col].Text.Trim());
                    }

                    // Map headers to properties
                    var propertyMap = new Dictionary<PropertyInfo, int>();
                    foreach (var prop in entityProperties)
                    {
                        var headerName = prop.Name;

                        var displayAttr = prop.GetCustomAttribute<DisplayNameAttribute>();
                        if (displayAttr != null)
                            headerName = displayAttr.DisplayName;

                        var colIndex = excelHeaders.FindIndex(h =>
                            h.Equals(headerName, StringComparison.OrdinalIgnoreCase));

                        if (colIndex >= 0)
                            propertyMap.Add(prop, colIndex + 1);
                    }

                    if (!propertyMap.Any())
                    {
                        TempData["ErrorMessage"] = "No matching columns found between Excel and model.";
                        return RedirectToAction("Index");
                    }

                    var newRecords = new List<BudgetUsage>();
                    var errorMessages = new List<string>();
                    bool hasErrors = false;

                    // Read rows
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        try
                        {
                            var record = new BudgetUsage();
                            bool rowHasValue = false;

                            foreach (var mapping in propertyMap)
                            {
                                var prop = mapping.Key;
                                var col = mapping.Value;
                                var cellValue = worksheet.Cells[row, col].Text;

                                if (!string.IsNullOrWhiteSpace(cellValue))
                                {
                                    rowHasValue = true;
                                    try
                                    {
                                        object value = Convert.ChangeType(
                                            cellValue,
                                            Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                                        prop.SetValue(record, value);
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception($"Column '{prop.Name}': {ex.Message}");
                                    }
                                }
                            }

                            if (rowHasValue)
                            {
                                var validationResults = new List<ValidationResult>();
                                if (Validator.TryValidateObject(record, new ValidationContext(record), validationResults, true))
                                {
                                    newRecords.Add(record);
                                }
                                else
                                {
                                    hasErrors = true;
                                    errorMessages.Add($"Row {row}: {string.Join(", ", validationResults.Select(v => v.ErrorMessage))}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            hasErrors = true;
                            errorMessages.Add($"Row {row}: {ex.Message}");
                        }
                    }

                    if (hasErrors)
                    {
                        TempData["ErrorDetails"] = errorMessages;
                        TempData["ErrorMessage"] = $"Import completed with {errorMessages.Count} errors.";
                    }
                    else
                    {
                        _context.BudgetUsages.AddRange(newRecords);
                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = $"Successfully imported {newRecords.Count} records.";
                    }

                    return RedirectToAction("Index");
                }
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Unexpected error during import: {ex.Message}";
            return RedirectToAction("Index");
        }
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
