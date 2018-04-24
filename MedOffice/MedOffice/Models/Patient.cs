using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MedOffice.Models
{
    [Table("Patients")]
    public class Patient
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole Imię jest wymagane.")]
        [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+$", ErrorMessage = "Pole Imię może zawierać wyłącznie litery. Dopuszczalne są polskie znaki.")]
        [Display(Name = "Imię:")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Pole Nazwisko jest wymagane.")]
        [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+(?:[\s\-][A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+)?$", ErrorMessage = "Pole Nazwisko może zawierać wyłącznie litery. Dopuszczalne są nazwiska dwuczłonowe i zawierające polskie znaki.")]
        [Display(Name = "Nazwisko:")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Pesel jest wymagany.")]
        [Display(Name = "Pesel:")]
        [RegularExpression("([0-9]{11})", ErrorMessage = "Pole pesel musi zawierać 11 cyfr.")]
        public string Pesel { get; set; }

        [Required(ErrorMessage = "Data urodzenia jest wymagana.")]
        [Display(Name = "Data urodzenia:")]
        [DataType(DataType.Date, ErrorMessage = "DATA")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Adres jest wymagany.")]
        [Display(Name = "Adres:")]
        [RegularExpression(@"^[0-9]{2}[\-][0-9]{3}[\s][A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+\,[\s][A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+([\s\-][A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+){0,}\s[0-9]+(?:\/[0-9]*)?$", ErrorMessage = "Pole Adres musi mieć strukturę: 00-000 Miasto, Ulica 00[/00]. Dopuszczalne są polskie znaki.")]
        public string Address { get; set; }
    }

    public class PatientDBContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
    }
}
