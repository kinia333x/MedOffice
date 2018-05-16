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

namespace MedOffice.Controllers
{
    [Authorize(Roles = "Administrator, Kierownik, Księgowa")]
    public class AccountantController : Controller
    {
        private AppointmentDBContext db = new AppointmentDBContext();

        public ActionResult Index(string searching, string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "price" ? "price_desc" : "price";
            ViewBag.AddPriceSortParm = sortOrder == "addPrice" ? "addPrice_desc" : "addPrice";
            ViewBag.TotalPriceSortParm = sortOrder == "totalPrice" ? "totalPrice_desc" : "totalPrice";
            ViewBag.DataSortParm = sortOrder == "data" ? "data_desc" : "data";

            var services = db.Appointments.Where(x => x.service_name.Contains(searching)
                                          || x.service_price.ToString().Contains(searching)
                                          || x.supplies_price.ToString().Contains(searching)
                                          || (x.supplies_price + x.service_price).ToString().Contains(searching)
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
                default: services = services.OrderBy(s => s.ServiceName); break;
            }

            return View(services.ToList());
        }

        public ActionResult Report()
        {
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

            return new PdfActionResult("Report", services);
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
                context.Entry(appointment).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appointment);
        }
    }
}
