using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations;

namespace MedOffice.Models
{
    [Table("PatientsArch")]
    public class PatientsArch
    {
        public int Id { get; set; }
        public int PId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Pesel { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public DateTime DateOfChange { get; set; }
        public string DBUser { get; set; }
        public string TypeOfChange { get; set; }
    }
}