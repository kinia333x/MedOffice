using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedOffice.Models
{
    public class AppointmentViewModels
    {
        public class ServiceViewModel
        {
            public int Id { get; set; }

            [Display(Name = "Typ usługi:")]
            public string ServiceType { get; set; }            //TYP USŁUGI
            [Display(Name = "Nazwa usługi:")]
            public string ServiceName { get; set; }            //nazwa usługi
            [Display(Name = "Data usługi:")]
            public DateTime ServiceDate { get; set; }          //DATA WIZYTY
            [Display(Name = "Cena usługi:")]
            public float ServicePrice { get; set; }            //cena usługi
            [Display(Name = "Koszty dodatkowe:")]
            public float SuppliesPrice { get; set; }           //dodatkowe koszty
            [Display(Name = "Suma kosztów:")]
            public float TotalPrice { get; set; }              //suma ceny usługi i kosztów dodatkowych
        }

        public class ServicesViewModel
        {
            public IList<int> SelectedServices { get; set; }
            public IList<ServiceViewModel> AvailableServices { get; set; }

            public ServicesViewModel()
            {
                SelectedServices = new List<int>();
                AvailableServices = new List<ServiceViewModel>();
            }
        }
    }
}