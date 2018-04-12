using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Insert name of the employee.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Insert surname of the employee.")]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Insert how many years of experience the employee has.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "The Years of Experience field may only contain digits.")]
        [Display(Name = "Years of Experience")]
        public string YearsOfExperience { get; set; }

        [Required(ErrorMessage = "Insert how many years the employee works for the firm.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "The Current job seniority field may only contain digits.")]
        [Display(Name = "Current job seniority")] // Pole 'Ile lat w firmie' przetłumaczyłem na mniej więcej 'Staż pracy w obecnym miejscu zatrudnienia'.
        public string YearsOfWork { get; set; }

        [Required(ErrorMessage = "Enter email address of the employee.")]
        [EmailAddress(ErrorMessage = "Enter proper email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter the ID of the employee.")]
        [StringLength(11, ErrorMessage = "The {0} field must contain {2} digits.", MinimumLength = 11)] // Możliwe, że można zrobić to w bardziej elegancki sposób.
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "The ID field may only contain digits.")]
        [Display(Name = "ID")] // Login będący peselem lub ID (w zależności od języka, który wybierzemy).
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter password.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        [Required(ErrorMessage = "Insert profession of the employee.")]
        [Display(Name = "Profession")]
        public string UserRoles { get; set; }

        [Required(ErrorMessage = "Insert specialization of the employee.")]
        [Display(Name = "Specialization")]
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
