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
        [Display(Name = "Kod")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Pamiętać przeglądarkę?")]
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
        [Display(Name = "Pesel")]
    
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętać mnie?")]
        public bool RememberMe { get; set; }
    }
    
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Wprowadź imię pracownika.")]
        [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+$", ErrorMessage = "Pole Imię może zawierać wyłącznie litery. Dopuszczalne są polskie znaki.")]
        [Display(Name = "Imię:")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Wprowadź nazwisko pracownika.")]
        [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+(?:[\s\-][A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+)?$", ErrorMessage = "Pole Nazwisko może zawierać wyłącznie litery. Dopuszczalne są nazwiska dwuczłonowe i zawierające polskie znaki.")]
        [Display(Name = "Nazwisko:")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Wprowadź datę pozyskania odpowiednich kwalifikacji do wykonywania zawodu przez pracownika.")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Display(Name = "Data uzyskania kwalifikacji:")]
        public DateTime Experience { get; set; }

        [Required(ErrorMessage = "Wprowadź datę rozpoczęcia przez pracownika pracy w obecnym miejscu zatrudnienia.")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Display(Name = "Data zatrudnienia:")]
        public DateTime Seniority { get; set; }

        [Required(ErrorMessage = "Wprowadź adres email pracownika.")]
        [EmailAddress(ErrorMessage = "Wprowadź poprawny adres email pracownika.")]
        [Display(Name = "Email:")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wprowadź pesel pracownika.")]
        [RegularExpression("([0-9]{11})", ErrorMessage = "Pole Pesel musi zawierać 11 cyfr.")]
        [Display(Name = "Pesel:")] // Login
        public string UserName { get; set; }

        [Required(ErrorMessage = "Wprowadź hasło.")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło:")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło:")]
        [Compare("Password", ErrorMessage = "Wprowadzone hasła różnią się od siebie.")]
        public string ConfirmPassword { get; set; }
        
        [Required(ErrorMessage = "Wprowadź zawód pracownika.")]
        [Display(Name = "Zawód:")]
        public string UserRoles { get; set; }

        [Required(ErrorMessage = "Wprowadź specjalizację pracownika.")]
        [Display(Name = "Specjalizacja:")]
        public string Specialization { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Hasło musi zawierać przynajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Wprowadzone hasła różnią się od siebie.")]
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
