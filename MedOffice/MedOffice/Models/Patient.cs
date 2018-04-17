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

        [Required(ErrorMessage = "Imię jest wymagane.")]
        [Display(Name = "Imię:")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [Display(Name = "Nazwisko:")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Wiek jest wymagany.")]
        [Display(Name = "Pesel:")]
        [RegularExpression("([0-9]{11})", ErrorMessage = "Pole Pesel musi zawierać 11 cyfr.")]
        public string Age { get; set; }

        [Required(ErrorMessage = "Pesel jest wymagany.")]
        [Display(Name = "Wiek:")]
        [RegularExpression("([0-9]*)", ErrorMessage = "Pole Wiek może zawierać wyłącznie cyfry.")]
        public string Pesel { get; set; }

        [Required(ErrorMessage = "Adres jest wymagany.")]
        [Display(Name = "Adres:")]
        public string Address { get; set; }
    }

    public class PatientDBContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
    }
}