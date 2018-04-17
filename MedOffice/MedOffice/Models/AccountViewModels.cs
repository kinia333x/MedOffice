using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace MedOffice.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "UserName")]
    
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
    
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Wprowadź imię pracownika.")]
        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Wprowadź nazwisko pracownika.")]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Wprowadź datę pozyskania odpowiednich kwalifikacji do wykonywania zawodu przez pracownika.")]
        //[RegularExpression("([0-9]*)", ErrorMessage = "The Years of Experience field may only contain digits.")]
        [Display(Name = "Data uzyskania kwalifikacji")]
        [DataType(DataType.Date, ErrorMessage = "siema")]
        public DateTime Experience { get; set; }

        [Required(ErrorMessage = "Wprowadź datę rozpoczęcia przez pracownika pracy w obecnym miejscu zatrudnienia.")]
        //[RegularExpression("([0-9]*)", ErrorMessage = "The Current job seniority field may only contain digits.")]
        [Display(Name = "Data zatrudnienia")] // Pole 'Ile lat w firmie' przetłumaczyłem na mniej więcej 'Staż pracy w obecnym miejscu zatrudnienia'.
        [DataType(DataType.Date, ErrorMessage = "DATA")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Seniority { get; set; }

        [Required(ErrorMessage = "Wprowadź adres email pracownika.")]
        [EmailAddress(ErrorMessage = "Wprowadź poprawny adres email pracownika.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wprowadź pesel pracownika.")]
        [StringLength(11, ErrorMessage = "Pole Pesel musi zawierać dokładnie {2} cyfr.", MinimumLength = 11)] // Możliwe, że można zrobić to w bardziej elegancki sposób.
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Pole Pesel może zawierać wyłącznie cyfry.")]
        [Display(Name = "Pesel")] // Login będący peselem lub ID (w zależności od języka, który wybierzemy).
        public string UserName { get; set; }

        [Required(ErrorMessage = "Wprowadź hasło.")]
        [StringLength(100, ErrorMessage = "Hasło musi zawierać przynajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Wprowadzone hasła różnią się od siebie. ")]
        public string ConfirmPassword { get; set; }
        
        [Required(ErrorMessage = "Wprowadź zawód pracownika.")]
        [Display(Name = "Zawód")]
        public string UserRoles { get; set; }

        [Required(ErrorMessage = "Wprowadź specjalizację pracownika.")]
        [Display(Name = "Specjalizacja")]
        public string Specialization { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
