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
    [Table("AppointmentsArch")]
    public class AppointmentsArch
    {
        public int ID { get; set; }
        public int Idd{ get; set; }
        public string patients_pesel { get; set; }
        public string estim_disease { get; set; }
        public string real_disease { get; set; }
        public string dis_descript { get; set; }
        public DateTime appoint_date { get; set; }
        public string specialization { get; set; }
        public string docs_pesel { get; set; }
        public string service_type { get; set; }
        public int service_time { get; set; }
        public string service_name { get; set; }
        public float service_price { get; set; }
        public bool is_paid { get; set; }
        public float supplies_price { get; set; }
        public DateTime DateOfChange { get; set; }
        public string DBUser { get; set; }
        public string TypeOfChange { get; set; }
    }
}