using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedOffice.Models
{
    [Table("Appointments")]
    public class Appointment
    {

        public int ID { get; set; }                         //id wizyty. (klucz)


        [Required(ErrorMessage = "Pesel pacjenta jest wymagany.")]
        [Display(Name = "Pesel pacjenta:")]
        public string patients_pesel { get; set; }          //PESEL pacjenta


        [Display(Name = "Szacowany problem:")]
        public string estim_disease { get; set; }           //estymowana choroba


        [Display(Name = "Zdiagnozowana choroba:")]
        public string real_disease { get; set; }            //zdiagnozowana choroba


        [Display(Name = "Opis dolegliwości:")]
        public string dis_descript { get; set; }            //opis dolegliwości

        [Required(ErrorMessage = "Data wizyty jest wymagana.")]
        [Display(Name = "Data wizyty:")]
        [DataType(DataType.Date, ErrorMessage = "DATA")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime appoint_date { get; set; }          //DATA WIZYTY

        [Required(ErrorMessage = "Specjalizacja jest wymagana.")]
        [Display(Name = "Specjalizacja:")]
        public string specialization { get; set; }          //SPECJALIZACJA

        [Required(ErrorMessage = "Lekarz jest wymagany.")]
        [Display(Name = "Lekarz:")]
        public string docs_pesel { get; set; }              //PESEL LEKARZA

        [Required(ErrorMessage = "Typ usługi jest wymagany.")]
        [Display(Name = "Typ usługi:")]
        public string service_type { get; set; }            //TYP USŁUGI

        //[Required(ErrorMessage = "Czas trwania wizyty jest wymagany.")]
        [Display(Name = "Szacowany czas trwania:")]
        public int service_time { get; set; }               //CZAS TRWANIA WIZYTY

        [Display(Name = "Nazwa usługi:")]
        public string service_name { get; set; }            //nazwa usługi


        [Display(Name = "Cena usługi:")]
        public float service_price { get; set; }            //cena usługi

        [Required(ErrorMessage = "Informacja jest wymagana.")]
        [Display(Name = "Czy usługa jest opłacona?")]
        public bool is_paid { get; set; }                   //CZY USŁUGA OPŁACONA?


        [Display(Name = "Koszty dodatkowe:")]
        public float supplies_price { get; set; }           //dodatkowe koszty


    }

    public class AppointmentDBContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
    }
}