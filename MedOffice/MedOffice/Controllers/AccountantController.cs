using MedOffice.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcRazorToPdf;
using System.Net;
using System.Data.Entity;
using static MedOffice.Models.AppointmentViewModels;

namespace MedOffice.Controllers
{
    [Authorize(Roles = "Administrator, Kierownik, Księgowa")]
    public class AccountantController : Controller
    {
        private AppointmentDBContext db = new AppointmentDBContext();
        private string CurrentUser = System.Web.HttpContext.Current.User.Identity.Name;
        public ActionResult Index(string searching, string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "price" ? "price_desc" : "price";
            ViewBag.AddPriceSortParm = sortOrder == "addPrice" ? "addPrice_desc" : "addPrice";
            ViewBag.TotalPriceSortParm = sortOrder == "totalPrice" ? "totalPrice_desc" : "totalPrice";
            ViewBag.DataSortParm = sortOrder == "data" ? "data_desc" : "data";
            ViewBag.TypeSortParm = sortOrder == "type" ? "type_desc" : "type";

            DateTime dateTime;
            DateTime.TryParse(searching, out dateTime);

            var services = db.Appointments.Where(x => x.service_name.Contains(searching)
                                          || (x.appoint_date.Year == dateTime.Year 
                                          && x.appoint_date.Month == dateTime.Month
                                          && x.appoint_date.Day == dateTime.Day)
                                          || x.service_price.ToString() == searching
                                          || x.supplies_price.ToString() == searching
                                          || (x.supplies_price + x.service_price).ToString() == searching
                                          || x.service_type.Contains(searching)
                                          || searching == null)
                                          .Select(a => new AppointmentViewModels.ServiceViewModel()
                                          {
                                              Id = a.ID,
                                              ServiceType = a.service_type,
                                              ServiceName = a.service_name,
                                              ServiceDate = a.appoint_date,
                                              ServicePrice = a.service_price,
                                              SuppliesPrice = a.supplies_price,
                                              TotalPrice = a.service_price + a.supplies_price
                                          });

            switch (sortOrder)
            {
                case "name_desc": services = services.OrderByDescending(s => s.ServiceName); break;
                case "price": services = services.OrderBy(s => s.ServicePrice); break;
                case "price_desc": services = services.OrderByDescending(s => s.ServicePrice); break;
                case "addPrice": services = services.OrderBy(s => s.SuppliesPrice); break;
                case "addPrice_desc": services = services.OrderByDescending(s => s.SuppliesPrice); break;
                case "totalPrice": services = services.OrderBy(s => s.TotalPrice); break;
                case "totalPrice_desc": services = services.OrderByDescending(s => s.TotalPrice); break;
                case "data": services = services.OrderBy(s => s.ServiceDate); break;
                case "data_desc": services = services.OrderByDescending(s => s.ServiceDate); break;
                case "type": services = services.OrderBy(s => s.ServiceType); break;
                case "type_desc": services = services.OrderByDescending(s => s.ServiceType); break;
                default: services = services.OrderBy(s => s.ServiceName); break;
            }

            var model = new  ServicesViewModel
            {
                AvailableServices = services.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string SubmitButton, string searching, string sortOrder, ServicesViewModel model)
        {
            switch (SubmitButton)
            {
                case "Szukaj":
                    return (Index(searching, sortOrder));
                case "Wygeneruj raport":
                    return (Report(model, false));
                case "Wygeneruj pełny raport":
                    return (Report(model, true));
                default:
                    return (Index(searching, sortOrder));
            }
        }
        
        public ActionResult Report(ServicesViewModel model, Boolean isFullReport)
        {
            if (ModelState.IsValid)
            {
                List<ServiceViewModel> Services = new List<ServiceViewModel>();

                if (isFullReport)
                {
                    Services = db.Appointments.ToList().Select(a => new AppointmentViewModels.ServiceViewModel()
                    {
                        Id = a.ID,
                        ServiceType = a.service_type,
                        ServiceName = a.service_name,
                        ServiceDate = a.appoint_date,
                        ServicePrice = a.service_price,
                        SuppliesPrice = a.supplies_price,
                        TotalPrice = a.service_price + a.supplies_price
                    }).ToList();
                    return new PdfActionResult("Report", Services);
                }

                foreach (var item in model.SelectedServices)
                {
                    var s = db.Appointments.ToList().Where(a => a.ID == item).Select(a => new AppointmentViewModels.ServiceViewModel()
                    {
                        Id = a.ID,
                        ServiceType = a.service_type,
                        ServiceName = a.service_name,
                        ServiceDate = a.appoint_date,
                        ServicePrice = a.service_price,
                        SuppliesPrice = a.supplies_price,
                        TotalPrice = a.service_price + a.supplies_price
                    }).FirstOrDefault();

                    Services.Add(s);
                }
                return new PdfActionResult("Report", Services);
            }

            var services = db.Appointments.ToList().Select(a => new AppointmentViewModels.ServiceViewModel()
            {
                Id = a.ID,
                ServiceType = a.service_type,
                ServiceName = a.service_name,
                ServiceDate = a.appoint_date,
                ServicePrice = a.service_price,
                SuppliesPrice = a.supplies_price,
                TotalPrice = a.service_price + a.supplies_price
            }).ToList();

            model.AvailableServices = services;

            return View(model);
           
        }

        [Authorize(Roles = "Administrator, Księgowa")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            return View(appointment);
        }

        // POST: Accountant/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Księgowa")]
        public ActionResult Edit([Bind(Include = "ID, patients_pesel, estim_disease, real_disease, dis_descript, appoint_date, specialization, docs_pesel, service_type, service_name, service_price, is_paid, supplies_price")] Appointment appointment)
        {
            AppointmentDBContext context = new AppointmentDBContext();

            if (ModelState.IsValid)
            {
                double time = -1;

                context.Entry(appointment).State = EntityState.Modified;
                context.SaveChanges();

                string query = "UPDATE [dbo].[AppointmentsArch] SET DBUSer = '" + CurrentUser + "' WHERE Idd = '" + appointment.ID + "' AND TypeOfChange = 'UPDATED-INSERTED' AND DateOfChange >= '" + DateTime.Now.AddSeconds(time) + "'";
                db.Database.ExecuteSqlCommand(query);

                query = "UPDATE [dbo].[AppointmentsArch] SET DBUSer = '" + CurrentUser + "' WHERE Idd = '" + appointment.ID + "' AND TypeOfChange = 'UPDATED-DELETED' AND DateOfChange >= '" + DateTime.Now.AddSeconds(time) + "'";
                db.Database.ExecuteSqlCommand(query);

                return RedirectToAction("Index");
            }

            return View(appointment);
        }
    }
}
