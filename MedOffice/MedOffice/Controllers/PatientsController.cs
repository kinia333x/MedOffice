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
    [Authorize(Roles = "Administrator, Kierownik, Rejestrujący")]
    public class PatientsController : Controller
    {
        private AppointmentDBContext dbA = new AppointmentDBContext();
        private PatientDBContext db = new PatientDBContext();
        private string CurrentUser = System.Web.HttpContext.Current.User.Identity.Name;

        // GET: Appointments/Changes
        [Authorize(Roles = "Administrator, Kierownik")]
        public ActionResult Changes()
        {
            return View(db.PatientsArch.ToList());
        }

        // GET: Patients
        public ActionResult Index()
        {
            return View(db.Patients.ToList());
        }

        // GET: Patients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patients/History/5
        public ActionResult History(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Patient patient = db.Patients.Find(id);
            var patientList = (from s in dbA.Appointments select s);
            patientList = patientList.Where(x => x.patients_pesel.Contains(patient.Pesel));
            ViewBag.PatientId = patient.Id;
            ViewBag.PatientPesel = patient.Pesel;
            if (patient == null)
            {
                return HttpNotFound();
            }
            //return View(patient);
            return View(patientList.ToList());
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Surname,Pesel,BirthDate,Address")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();

                string query = "UPDATE [dbo].[PatientsArch] SET DBUSer = '" + CurrentUser + "' WHERE TypeOfChange = 'INSERTED' AND Pesel = " + patient.Pesel;
                db.Database.ExecuteSqlCommand(query);

                return RedirectToAction("Index");
            }

            return View(patient);
        }

        // GET: Patients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,Pesel,BirthDate,Address")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                double time = -1;

                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();

                // Dodanie do archiwum użytkownika, który zmienił dane pacjenta:
                string query = "UPDATE [dbo].[PatientsArch] SET DBUSer = '" + CurrentUser + "' WHERE TypeOfChange = 'UPDATED-INSERTED' AND Pesel = " + patient.Pesel + " AND DateOfChange >= '" + DateTime.Now.AddSeconds(time) + "'";
                db.Database.ExecuteSqlCommand(query);

                query = "UPDATE [dbo].[PatientsArch] SET DBUSer = '" + CurrentUser + "' WHERE TypeOfChange = 'UPDATED-DELETED' AND Pesel = " + patient.Pesel + " AND DateOfChange >= '" + DateTime.Now.AddSeconds(time) + "'"; ;
                db.Database.ExecuteSqlCommand(query);

                return RedirectToAction("Index");
            }

            return View(patient);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
            db.SaveChanges();

            // Dodanie do archiwum użytkownika, który usunął dane pacjenta:
            string query = "UPDATE [dbo].[PatientsArch] SET DBUSer = '" + CurrentUser + "' WHERE TypeOfChange = 'DELETED' AND Pesel = " + patient.Pesel;
            db.Database.ExecuteSqlCommand(query);

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
