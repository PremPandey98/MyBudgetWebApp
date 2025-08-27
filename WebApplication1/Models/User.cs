using WebApplication1.Models;
using BudgetMobApp.Models;
using System.ComponentModel.DataAnnotations;

namespace BudgetMobApp.Models
{
    public enum AccountType
    {
        Personal,
        Group
    }

    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")] 
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Name can only contain letters and spaces.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required")] 
        [EmailAddress(ErrorMessage = "Invalid email address")] 
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits")]
        [RegularExpression(@"^[6-9][0-9]{9}$", ErrorMessage = "Enter a valid 10-digit Indian mobile number")] 
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Username is required")] 
        [StringLength(30, ErrorMessage = "Username cannot be longer than 30 characters.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")] 
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }

        [Required]
        public AccountType AccountType { get; set; }
        public int? GroupId { get; set; }
        public Group? Group { get; set; }
    }
}
