using System;
using System.Collections.Generic;
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
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Age { get; set; }
        public string Pesel { get; set; }
        public string Address { get; set; }
    }

    public class PatientDBContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
    }
}