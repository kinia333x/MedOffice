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
    [Table("UsersArch")]
    public class UsersArch 
    {
        public UsersArch ()
        {
            Experience = DateTime.Now;
            Seniority = DateTime.Now;
        }

        public int Id { get; set; }
        public string UId { get; set; }
        public string RId { get; set; }
        public string Email { get; set; }
        public Boolean EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public Boolean PhoneNumberConfirmed { get; set; }
        public Boolean TwoFactorEnabled { get; set; }
        public DateTime LockoutEndDateUtc { get; set; }
        public Boolean LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public string Specialization { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Experience { get; set; }
        public DateTime Seniority { get; set; }
        public DateTime DateOfChange { get; set; }
        public string DBUser { get; set; }
        public string TypeOfChange { get; set; }
    }
}