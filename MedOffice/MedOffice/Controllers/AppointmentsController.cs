using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedOffice.Models;

namespace MedOffice.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private AppointmentDBContext db = new AppointmentDBContext();

        // GET: Appointments
        [Authorize(Roles = "Administrator, Rejestrujący")]
        public ActionResult Index()
        {
            return View(db.Appointments.ToList());
        }

        // GET: Appointments
        [Authorize(Roles = "Administrator, Lekarz")]
        public ActionResult Show()
        {
            return View(db.Appointments.ToList());
        }

        // GET: Appointments/Details/5
        [Authorize(Roles = "Administrator, Lekarz, Rejestrujący")]
        public ActionResult Details(int? id)
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

        // GET: Appointments/Create
        [Authorize(Roles = "Administrator, Rejestrujący")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Appointments/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Rejestrujący")]
        public ActionResult Create([Bind(Include = "ID,patients_pesel,estim_disease,real_disease,dis_descript,appoint_date,specialization,docs_pesel,service_type,service_name,service_price,is_paid,supplies_price")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
          
            return View(appointment);
        }

        /////////////////////////////////////// fill dla lekarza


        [Authorize(Roles = "Lekarz")]
        public ActionResult Fill(int? id)
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lekarz")]
        public ActionResult Fill([Bind(Include = "ID,patients_pesel,estim_disease,real_disease,dis_descript,appoint_date,specialization,docs_pesel,service_type,service_name,service_price,is_paid,supplies_price")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Show");
            }
            return View(appointment);
        }

        ////////////////////////////////////////////koniec fila dla lekarza


        // GET: Appointments/Edit/5
        [Authorize(Roles = "Administrator, Rejestrujący")]
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

        // POST: Appointments/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Rejestrujący")]
        public ActionResult Edit([Bind(Include ="ID,patients_pesel,estim_disease,real_disease,dis_descript,appoint_date,specialization,docs_pesel,service_type,service_name,service_price,is_paid,supplies_price")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        [Authorize(Roles = "Administrator, Rejestrujący")]
        public ActionResult Delete(int? id)
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

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Rejestrujący")]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
