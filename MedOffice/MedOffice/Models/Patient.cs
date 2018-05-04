using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace MedOffice.Models
{
    [Table("Patients")]
    public class Patient
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole Imię jest wymagane.")]
        [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+$", ErrorMessage = "Pole imię musi mieć strukturę: Imię. Może zawierać wyłącznie litery. Dopuszczalne są polskie znaki.")]
        [Display(Name = "Imię:")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Pole Nazwisko jest wymagane.")]
        [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+(?:[\s\-][A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+)?$", ErrorMessage = "Pole Nazwisko musi mieć strukturę: Nazwisko. Może zawierać wyłącznie litery. Dopuszczalne są nazwiska dwuczłonowe i zawierające polskie znaki.")]
        [Display(Name = "Nazwisko:")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Pesel jest wymagany.")]
        [Display(Name = "Pesel:")]
        [PeselUnique]
        [PeselAndDateOfBirth("BirthDate")]
        [RegularExpression("([0-9]{11})", ErrorMessage = "Pole pesel musi zawierać 11 cyfr.")]
        public string Pesel { get; set; }

        [Required(ErrorMessage = "Data urodzenia jest wymagana.")]
        [Display(Name = "Data urodzenia:")]
        [DataType(DataType.Date)]
        [PastDate]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Adres jest wymagany.")]
        [Display(Name = "Adres:")]
        [RegularExpression(@"^[0-9]{2}[\-][0-9]{3}[\s][A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+([\s\-][A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+){0,}(?:\,[\s][A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+([\s\-][A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+){0,})?\s[0-9]+(?:\/[0-9]*)?$", ErrorMessage = "Pole Adres musi mieć strukturę: 00-000 Miasto[, Ulica] 00[/00]. Dopuszczalne są polskie znaki.")]
        public string Address { get; set; }
    }

    public class PastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && (DateTime)value < DateTime.Now)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Data urodzenia musi być w przeszłości.");
            }
        }
    }
    public class PeselUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PatientDBContext db = new PatientDBContext();
            var patients = db.Patients.FirstOrDefault(p => p.Pesel == (string)value);

            if (patients == null)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Pacjent o padanym peselu już istnieje.");
            }
        }
    }
    public class PeselAndDateOfBirthAttribute : ValidationAttribute
    {
        private string _birthDateName { get; set; }
        public PeselAndDateOfBirthAttribute(string birthDateName)
        {
            _birthDateName = birthDateName;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string _value = value.ToString();
            var _birthDateProperty = validationContext.ObjectType.GetProperty(_birthDateName);
            if (_birthDateProperty == null)
            {
                return new ValidationResult("Error!");
            }

            DateTime _birthDate = (DateTime)_birthDateProperty.GetValue(validationContext.ObjectInstance, null);

            var dt = _birthDate.ToString("yyyyMMdd");

            if (dt.ToString()[2] != _value[0])
            {
                return new ValidationResult("Data urodzenia oraz pesel nie zgadzają się. Rok jest niepoprawny.");
            }
            if (dt.ToString()[3] != _value[1])
            {
                return new ValidationResult("Data urodzenia oraz pesel nie zgadzają się. Rok jest niepoprawny.");
            }

            if (dt.ToString()[4] != _value[2])
            {
                return new ValidationResult("Data urodzenia oraz pesel nie zgadzają się. Miesiąc jest niepoprawny.");
            }
            if (dt.ToString()[5] != _value[3])
            {
                return new ValidationResult("Data urodzenia oraz pesel nie zgadzają się. Miesiąc jest niepoprawny.");
            }

            if (dt.ToString()[6] != _value[4])
            {
                return new ValidationResult("Data urodzenia oraz pesel nie zgadzają się. Dzień jest niepoprawny.");
            }
            if (dt.ToString()[7] != _value[5])
            {
                return new ValidationResult("Data urodzenia oraz pesel nie zgadzają się. Dzień jest niepoprawny.");
            }

            return ValidationResult.Success;
        }
    }
    public class PatientDBContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
    }
}
