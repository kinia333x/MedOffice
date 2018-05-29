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
    [Table("WorkingTime")]
    public class WorkingTime
    {
        public int id { get; set; } // Id wpisu
        public string pesel { get; set; } // Pesel pracownika
        public DateTime eventstart { get; set; } // Rozpoczęcie pracy
        public DateTime eventend { get; set; } // Koniec pracy
        public String name { get; set; } // Tekst, np. "Praca 10 - 15"
        public int resource { get; set; } // Lista rozwijana, aby łatwiej nawigować między pracownikami
        public int recurrence { get; set; }
    }

    [Table("Resources")]
    public class Resources
    {
        [Key]
        public string name { get; set; } // Pesel pracownika
        public string fsname { get; set; }
        public int id { get; set; }
    }
}