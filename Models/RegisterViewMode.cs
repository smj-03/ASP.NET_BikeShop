namespace BikeShop.Models;
using System.ComponentModel.DataAnnotations;
public class RegisterViewModel
{
    [Required]
    [EmailAddress] 
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]  // ← i tutaj
    [Compare("Password", ErrorMessage = "Hasła się nie zgadzają.")]
    public string ConfirmPassword { get; set; }
    
    [Required]
    [Display(Name = "Imię")]
    public string FirstName { get; set; }
    
    [Required]
    [Display(Name = "Nazwisko")]
    public string LastName { get; set; }
    
    [Required]
    [RegularExpression(@"^\d{9,15}$", ErrorMessage = "Pole może zawierać tylko cyfry (9-15 znaków).")]
    public string PhoneNumber { get; set; }
}