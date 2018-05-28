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
        private PatientDBContext dbP = new PatientDBContext();
        private ApplicationDbContext dbL = new ApplicationDbContext();
        private string CurrentUser = System.Web.HttpContext.Current.User.Identity.Name;


        public JsonResult GetDocList (string specialization)
        {
            dbL.Configuration.ProxyCreationEnabled = false;
            return Json(dbL.Users.Where(u=>u.Specialization == specialization), JsonRequestBehavior.AllowGet);
        }


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


        // GET: Appointments/All
        [Authorize(Roles = "Administrator, Kierownik")]
        public ActionResult All()
        {
            return View(db.Appointments.ToList());
        }


        // GET: Appointments/Details/5
        [Authorize(Roles = "Administrator, Rejestrujący")]
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


        // GET: Appointments/Details/5
        [Authorize(Roles = "Administrator, Lekarz")]
        public ActionResult Specifics(int? id)
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


        // GET: Appointments/Particulars/5
        [Authorize(Roles = "Administrator, Kierownik")]
        public ActionResult Particulars(int? id)
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
            List<SelectListItem> Specializations = new List<SelectListItem>()
            {
                new SelectListItem { Text = "" },
                new SelectListItem { Text = "Alergologia" },
                new SelectListItem { Text = "Anestezjologia i intensywna terapia" },
                new SelectListItem { Text = "Angiologia" },
                new SelectListItem { Text = "Audiologia i foniatria" },
                new SelectListItem { Text = "Balneologia i medycyna fizykalna" },
                new SelectListItem { Text = "Chirurgia dziecięca" },
                new SelectListItem { Text = "Chirurgia klatki piersiowej" },
                new SelectListItem { Text = "Chirurgia naczyniowa" },
                new SelectListItem { Text = "Chirurgia ogólna" },
                new SelectListItem { Text = "Chirurgia onkologiczna" },
                new SelectListItem { Text = "Chirurgia plastyczna" },
                new SelectListItem { Text = "Chirurgia stomatologiczna" },
                new SelectListItem { Text = "Chirurgia szczękowo-twarzowa" },
                new SelectListItem { Text = "Chirurgia płuc" },
                new SelectListItem { Text = "Chirurgia płuc dzieci" },
                new SelectListItem { Text = "Choroby płuc" },
                new SelectListItem { Text = "Choroby płuc dzieci" },
                new SelectListItem { Text = "Choroby wewnętrzne" },
                new SelectListItem { Text = "Choroby zakaźne" },
                new SelectListItem { Text = "Dermatologia i wenerologia" },
                new SelectListItem { Text = "Diabetologia" },
                new SelectListItem { Text = "Endokrynologia" },
                new SelectListItem { Text = "Endokrynologia ginekologiczna i rozrodczość" },
                new SelectListItem { Text = "Endokrynologia i diabetologia dziecięca" },
                new SelectListItem { Text = "Epidemiologia" },
                new SelectListItem { Text = "Farmakologia kliniczna" },
                new SelectListItem { Text = "Gastroenterologia" },
                new SelectListItem { Text = "Gastroenterologia dziecięca" },
                new SelectListItem { Text = "Genetyka kliniczna" },
                new SelectListItem { Text = "Geriatria" },
                new SelectListItem { Text = "Ginekologia onkologiczna" },
                new SelectListItem { Text = "Hematologia" },
                new SelectListItem { Text = "Hipertensjologia" },
                new SelectListItem { Text = "Immunologia kliniczna" },
                new SelectListItem { Text = "Intensywna terapia" },
                new SelectListItem { Text = "Kardiochirurgia" },
                new SelectListItem { Text = "Kardiologia" },
                new SelectListItem { Text = "Kardiologia dziecięca" },
                new SelectListItem { Text = "Medycyna lotnicza" },
                new SelectListItem { Text = "Medycyna morska i tropikalna" },
                new SelectListItem { Text = "Medycyna nuklearna" },
                new SelectListItem { Text = "Medycyna paliatywna" },
                new SelectListItem { Text = "Medycyna pracy" },
                new SelectListItem { Text = "Medycyna ratunkowa" },
                new SelectListItem { Text = "Medycyna rodzinna" },
                new SelectListItem { Text = "Medycyna sądowa" },
                new SelectListItem { Text = "Medycyna sportowa" },
                new SelectListItem { Text = "Mikrobiologia lekarska" },
                new SelectListItem { Text = "Nefrologia" },
                new SelectListItem { Text = "Nefrologia dziecięca" },
                new SelectListItem { Text = "Neonatologia" },
                new SelectListItem { Text = "Neurochirurgia" },
                new SelectListItem { Text = "Neurologia" },
                new SelectListItem { Text = "Neurologia dziecięca" },
                new SelectListItem { Text = "Neuropatologia" },
                new SelectListItem { Text = "Okulistyka" },
                new SelectListItem { Text = "Onkologia i hematologia dziecięca" },
                new SelectListItem { Text = "Onkologia kliniczna " },
                new SelectListItem { Text = "Ortodoncja" },
                new SelectListItem { Text = "Ortopedia i traumatologia narządu ruchu" },
                new SelectListItem { Text = "Otorynolaryngologia" },
                new SelectListItem { Text = "Otorynolaryngologia dziecięca" },
                new SelectListItem { Text = "Patomorfologia" },
                new SelectListItem { Text = "Pediatria" },
                new SelectListItem { Text = "Pediatria metaboliczna" },
                new SelectListItem { Text = "Perinatologia" },
                new SelectListItem { Text = "Periodontologia" },
                new SelectListItem { Text = "Położnictwo i ginekologia" },
                new SelectListItem { Text = "Protetyka stomatologiczna" },
                new SelectListItem { Text = "Psychiatria" },
                new SelectListItem { Text = "Psychiatria dzieci i młodzieży" },
                new SelectListItem { Text = "Radiologia i diagnostyka obrazowa" },
                new SelectListItem { Text = "Radioterapia onkologiczna" },
                new SelectListItem { Text = "Rehabilitacja medyczna" },
                new SelectListItem { Text = "Reumatologia" },
                new SelectListItem { Text = "Seksuologia" },
                new SelectListItem { Text = "Stomatologia dziecięca" },
                new SelectListItem { Text = "Stomatologia zachowawcza z endodoncją" },
                new SelectListItem { Text = "Toksykologia kliniczna" },
                new SelectListItem { Text = "Transfuzjologia kliniczna" },
                new SelectListItem { Text = "Transplantologia kliniczna" },
                new SelectListItem { Text = "Urologia" },
                new SelectListItem { Text = "Urologia dziecięca" },
                new SelectListItem { Text = "Zdrowie publiczne " }
            };
            ViewBag.Spec = Specializations;

            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Rejestrujący")]
        public ActionResult Create([Bind(Include = "ID,patients_pesel,estim_disease,real_disease,dis_descript,appoint_date,specialization,docs_pesel,service_type,service_name,service_price,is_paid,supplies_price")] Appointment appointment)
        {

            List<SelectListItem> Specializations = new List<SelectListItem>()
            {
                new SelectListItem { Text = "" },
                new SelectListItem { Text = "Alergologia" },
                new SelectListItem { Text = "Anestezjologia i intensywna terapia" },
                new SelectListItem { Text = "Angiologia" },
                new SelectListItem { Text = "Audiologia i foniatria" },
                new SelectListItem { Text = "Balneologia i medycyna fizykalna" },
                new SelectListItem { Text = "Chirurgia dziecięca" },
                new SelectListItem { Text = "Chirurgia klatki piersiowej" },
                new SelectListItem { Text = "Chirurgia naczyniowa" },
                new SelectListItem { Text = "Chirurgia ogólna" },
                new SelectListItem { Text = "Chirurgia onkologiczna" },
                new SelectListItem { Text = "Chirurgia plastyczna" },
                new SelectListItem { Text = "Chirurgia stomatologiczna" },
                new SelectListItem { Text = "Chirurgia szczękowo-twarzowa" },
                new SelectListItem { Text = "Chirurgia płuc" },
                new SelectListItem { Text = "Chirurgia płuc dzieci" },
                new SelectListItem { Text = "Choroby płuc" },
                new SelectListItem { Text = "Choroby płuc dzieci" },
                new SelectListItem { Text = "Choroby wewnętrzne" },
                new SelectListItem { Text = "Choroby zakaźne" },
                new SelectListItem { Text = "Dermatologia i wenerologia" },
                new SelectListItem { Text = "Diabetologia" },
                new SelectListItem { Text = "Endokrynologia" },
                new SelectListItem { Text = "Endokrynologia ginekologiczna i rozrodczość" },
                new SelectListItem { Text = "Endokrynologia i diabetologia dziecięca" },
                new SelectListItem { Text = "Epidemiologia" },
                new SelectListItem { Text = "Farmakologia kliniczna" },
                new SelectListItem { Text = "Gastroenterologia" },
                new SelectListItem { Text = "Gastroenterologia dziecięca" },
                new SelectListItem { Text = "Genetyka kliniczna" },
                new SelectListItem { Text = "Geriatria" },
                new SelectListItem { Text = "Ginekologia onkologiczna" },
                new SelectListItem { Text = "Hematologia" },
                new SelectListItem { Text = "Hipertensjologia" },
                new SelectListItem { Text = "Immunologia kliniczna" },
                new SelectListItem { Text = "Intensywna terapia" },
                new SelectListItem { Text = "Kardiochirurgia" },
                new SelectListItem { Text = "Kardiologia" },
                new SelectListItem { Text = "Kardiologia dziecięca" },
                new SelectListItem { Text = "Medycyna lotnicza" },
                new SelectListItem { Text = "Medycyna morska i tropikalna" },
                new SelectListItem { Text = "Medycyna nuklearna" },
                new SelectListItem { Text = "Medycyna paliatywna" },
                new SelectListItem { Text = "Medycyna pracy" },
                new SelectListItem { Text = "Medycyna ratunkowa" },
                new SelectListItem { Text = "Medycyna rodzinna" },
                new SelectListItem { Text = "Medycyna sądowa" },
                new SelectListItem { Text = "Medycyna sportowa" },
                new SelectListItem { Text = "Mikrobiologia lekarska" },
                new SelectListItem { Text = "Nefrologia" },
                new SelectListItem { Text = "Nefrologia dziecięca" },
                new SelectListItem { Text = "Neonatologia" },
                new SelectListItem { Text = "Neurochirurgia" },
                new SelectListItem { Text = "Neurologia" },
                new SelectListItem { Text = "Neurologia dziecięca" },
                new SelectListItem { Text = "Neuropatologia" },
                new SelectListItem { Text = "Okulistyka" },
                new SelectListItem { Text = "Onkologia i hematologia dziecięca" },
                new SelectListItem { Text = "Onkologia kliniczna " },
                new SelectListItem { Text = "Ortodoncja" },
                new SelectListItem { Text = "Ortopedia i traumatologia narządu ruchu" },
                new SelectListItem { Text = "Otorynolaryngologia" },
                new SelectListItem { Text = "Otorynolaryngologia dziecięca" },
                new SelectListItem { Text = "Patomorfologia" },
                new SelectListItem { Text = "Pediatria" },
                new SelectListItem { Text = "Pediatria metaboliczna" },
                new SelectListItem { Text = "Perinatologia" },
                new SelectListItem { Text = "Periodontologia" },
                new SelectListItem { Text = "Położnictwo i ginekologia" },
                new SelectListItem { Text = "Protetyka stomatologiczna" },
                new SelectListItem { Text = "Psychiatria" },
                new SelectListItem { Text = "Psychiatria dzieci i młodzieży" },
                new SelectListItem { Text = "Radiologia i diagnostyka obrazowa" },
                new SelectListItem { Text = "Radioterapia onkologiczna" },
                new SelectListItem { Text = "Rehabilitacja medyczna" },
                new SelectListItem { Text = "Reumatologia" },
                new SelectListItem { Text = "Seksuologia" },
                new SelectListItem { Text = "Stomatologia dziecięca" },
                new SelectListItem { Text = "Stomatologia zachowawcza z endodoncją" },
                new SelectListItem { Text = "Toksykologia kliniczna" },
                new SelectListItem { Text = "Transfuzjologia kliniczna" },
                new SelectListItem { Text = "Transplantologia kliniczna" },
                new SelectListItem { Text = "Urologia" },
                new SelectListItem { Text = "Urologia dziecięca" },
                new SelectListItem { Text = "Zdrowie publiczne " }
            };
            ViewBag.Spec = Specializations;

            if (ModelState.IsValid)
            {

                int x = 1;
                if (DateTime.Compare(appointment.appoint_date, DateTime.Now) <= 0)
                {
                    ModelState.AddModelError("appoint_date", "Nie można ustawić daty przeszłej.");
                    x = 0;
                }

                foreach (Patient p in dbP.Patients)
                {
                    if (p.Pesel == appointment.patients_pesel)
                    {
                        if(x == 1)
                        {
                            db.Appointments.Add(appointment);
                            db.SaveChanges();

                            string query = "UPDATE [dbo].[AppointmentsArch] SET DBUSer = '" + CurrentUser + "' WHERE TypeOfChange = 'INSERTED' AND Idd = " + appointment.ID;
                            db.Database.ExecuteSqlCommand(query);

                            //dodanie do kalendarza
                            //"10" to id wizyt, narazie na sztywno
                            new TutorialCS.EventManager().EventCreate(appointment.appoint_date, appointment.appoint_date.AddMinutes(20), appointment.service_type + " " + appointment.patients_pesel, "10", appointment.ID);

                            return RedirectToAction("Index");
                        }
                        else
                            return View(appointment);
                    }
                }

                ModelState.AddModelError("patients_pesel", "Nie ma takiego pacjenta.");
            }
            return View(appointment);
        }

        /////////////////////////////////////// fill dla lekarza

        // GET: Appointments/Fill/5
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
            ViewBag.opt = appointment.service_type;

            return View(appointment);
        }


        // POST: Appointments/Fill/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lekarz")]
        public ActionResult Fill([Bind(Include = "ID,patients_pesel,estim_disease,real_disease,dis_descript,appoint_date,specialization,docs_pesel,service_type,service_name,service_price,is_paid,supplies_price")] Appointment appointment)
        {
            ViewBag.opt = appointment.service_type;

            if (ModelState.IsValid)
            {
                int x = 1;

                if (appointment.service_price < 0)
                {
                    ModelState.AddModelError("service_price", "Koszt usługi nie może być ujemny");
                    x = 0;
                }

                if (appointment.supplies_price < 0)
                {
                    ModelState.AddModelError("supplies_price", "Koszt materiałów nie może być ujemny");
                    x = 0;
                }

                if (x == 1)
                {
                    db.Entry(appointment).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Show");
                }
            }
            return View(appointment);
        }

        ////////////////////////////////////////////koniec fila dla lekarza


        // GET: Appointments/Edit/5
        [Authorize(Roles = "Administrator, Rejestrujący")]
        public ActionResult Edit(int? id)
        {
            List<SelectListItem> Specializations = new List<SelectListItem>()
            {
                new SelectListItem { Text = "" },
                new SelectListItem { Text = "Alergologia" },
                new SelectListItem { Text = "Anestezjologia i intensywna terapia" },
                new SelectListItem { Text = "Angiologia" },
                new SelectListItem { Text = "Audiologia i foniatria" },
                new SelectListItem { Text = "Balneologia i medycyna fizykalna" },
                new SelectListItem { Text = "Chirurgia dziecięca" },
                new SelectListItem { Text = "Chirurgia klatki piersiowej" },
                new SelectListItem { Text = "Chirurgia naczyniowa" },
                new SelectListItem { Text = "Chirurgia ogólna" },
                new SelectListItem { Text = "Chirurgia onkologiczna" },
                new SelectListItem { Text = "Chirurgia plastyczna" },
                new SelectListItem { Text = "Chirurgia stomatologiczna" },
                new SelectListItem { Text = "Chirurgia szczękowo-twarzowa" },
                new SelectListItem { Text = "Chirurgia płuc" },
                new SelectListItem { Text = "Chirurgia płuc dzieci" },
                new SelectListItem { Text = "Choroby płuc" },
                new SelectListItem { Text = "Choroby płuc dzieci" },
                new SelectListItem { Text = "Choroby wewnętrzne" },
                new SelectListItem { Text = "Choroby zakaźne" },
                new SelectListItem { Text = "Dermatologia i wenerologia" },
                new SelectListItem { Text = "Diabetologia" },
                new SelectListItem { Text = "Endokrynologia" },
                new SelectListItem { Text = "Endokrynologia ginekologiczna i rozrodczość" },
                new SelectListItem { Text = "Endokrynologia i diabetologia dziecięca" },
                new SelectListItem { Text = "Epidemiologia" },
                new SelectListItem { Text = "Farmakologia kliniczna" },
                new SelectListItem { Text = "Gastroenterologia" },
                new SelectListItem { Text = "Gastroenterologia dziecięca" },
                new SelectListItem { Text = "Genetyka kliniczna" },
                new SelectListItem { Text = "Geriatria" },
                new SelectListItem { Text = "Ginekologia onkologiczna" },
                new SelectListItem { Text = "Hematologia" },
                new SelectListItem { Text = "Hipertensjologia" },
                new SelectListItem { Text = "Immunologia kliniczna" },
                new SelectListItem { Text = "Intensywna terapia" },
                new SelectListItem { Text = "Kardiochirurgia" },
                new SelectListItem { Text = "Kardiologia" },
                new SelectListItem { Text = "Kardiologia dziecięca" },
                new SelectListItem { Text = "Medycyna lotnicza" },
                new SelectListItem { Text = "Medycyna morska i tropikalna" },
                new SelectListItem { Text = "Medycyna nuklearna" },
                new SelectListItem { Text = "Medycyna paliatywna" },
                new SelectListItem { Text = "Medycyna pracy" },
                new SelectListItem { Text = "Medycyna ratunkowa" },
                new SelectListItem { Text = "Medycyna rodzinna" },
                new SelectListItem { Text = "Medycyna sądowa" },
                new SelectListItem { Text = "Medycyna sportowa" },
                new SelectListItem { Text = "Mikrobiologia lekarska" },
                new SelectListItem { Text = "Nefrologia" },
                new SelectListItem { Text = "Nefrologia dziecięca" },
                new SelectListItem { Text = "Neonatologia" },
                new SelectListItem { Text = "Neurochirurgia" },
                new SelectListItem { Text = "Neurologia" },
                new SelectListItem { Text = "Neurologia dziecięca" },
                new SelectListItem { Text = "Neuropatologia" },
                new SelectListItem { Text = "Okulistyka" },
                new SelectListItem { Text = "Onkologia i hematologia dziecięca" },
                new SelectListItem { Text = "Onkologia kliniczna " },
                new SelectListItem { Text = "Ortodoncja" },
                new SelectListItem { Text = "Ortopedia i traumatologia narządu ruchu" },
                new SelectListItem { Text = "Otorynolaryngologia" },
                new SelectListItem { Text = "Otorynolaryngologia dziecięca" },
                new SelectListItem { Text = "Patomorfologia" },
                new SelectListItem { Text = "Pediatria" },
                new SelectListItem { Text = "Pediatria metaboliczna" },
                new SelectListItem { Text = "Perinatologia" },
                new SelectListItem { Text = "Periodontologia" },
                new SelectListItem { Text = "Położnictwo i ginekologia" },
                new SelectListItem { Text = "Protetyka stomatologiczna" },
                new SelectListItem { Text = "Psychiatria" },
                new SelectListItem { Text = "Psychiatria dzieci i młodzieży" },
                new SelectListItem { Text = "Radiologia i diagnostyka obrazowa" },
                new SelectListItem { Text = "Radioterapia onkologiczna" },
                new SelectListItem { Text = "Rehabilitacja medyczna" },
                new SelectListItem { Text = "Reumatologia" },
                new SelectListItem { Text = "Seksuologia" },
                new SelectListItem { Text = "Stomatologia dziecięca" },
                new SelectListItem { Text = "Stomatologia zachowawcza z endodoncją" },
                new SelectListItem { Text = "Toksykologia kliniczna" },
                new SelectListItem { Text = "Transfuzjologia kliniczna" },
                new SelectListItem { Text = "Transplantologia kliniczna" },
                new SelectListItem { Text = "Urologia" },
                new SelectListItem { Text = "Urologia dziecięca" },
                new SelectListItem { Text = "Zdrowie publiczne " }
            };
            ViewBag.Spec = Specializations;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            foreach (ApplicationUser doc in dbL.Users)
            {
                if (doc.UserName == appointment.docs_pesel)
                {
                    ViewBag.DocsSurname = doc.Surname;
                    ViewBag.opt = appointment.service_type;
                }
            }

            return View(appointment);
        }


        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Rejestrujący")]
        public ActionResult Edit([Bind(Include ="ID,patients_pesel,estim_disease,real_disease,dis_descript,appoint_date,specialization,docs_pesel,service_type,service_name,service_price,is_paid,supplies_price")] Appointment appointment)
        {

            List<SelectListItem> Specializations = new List<SelectListItem>()
            {
                new SelectListItem { Text = "" },
                new SelectListItem { Text = "Alergologia" },
                new SelectListItem { Text = "Anestezjologia i intensywna terapia" },
                new SelectListItem { Text = "Angiologia" },
                new SelectListItem { Text = "Audiologia i foniatria" },
                new SelectListItem { Text = "Balneologia i medycyna fizykalna" },
                new SelectListItem { Text = "Chirurgia dziecięca" },
                new SelectListItem { Text = "Chirurgia klatki piersiowej" },
                new SelectListItem { Text = "Chirurgia naczyniowa" },
                new SelectListItem { Text = "Chirurgia ogólna" },
                new SelectListItem { Text = "Chirurgia onkologiczna" },
                new SelectListItem { Text = "Chirurgia plastyczna" },
                new SelectListItem { Text = "Chirurgia stomatologiczna" },
                new SelectListItem { Text = "Chirurgia szczękowo-twarzowa" },
                new SelectListItem { Text = "Chirurgia płuc" },
                new SelectListItem { Text = "Chirurgia płuc dzieci" },
                new SelectListItem { Text = "Choroby płuc" },
                new SelectListItem { Text = "Choroby płuc dzieci" },
                new SelectListItem { Text = "Choroby wewnętrzne" },
                new SelectListItem { Text = "Choroby zakaźne" },
                new SelectListItem { Text = "Dermatologia i wenerologia" },
                new SelectListItem { Text = "Diabetologia" },
                new SelectListItem { Text = "Endokrynologia" },
                new SelectListItem { Text = "Endokrynologia ginekologiczna i rozrodczość" },
                new SelectListItem { Text = "Endokrynologia i diabetologia dziecięca" },
                new SelectListItem { Text = "Epidemiologia" },
                new SelectListItem { Text = "Farmakologia kliniczna" },
                new SelectListItem { Text = "Gastroenterologia" },
                new SelectListItem { Text = "Gastroenterologia dziecięca" },
                new SelectListItem { Text = "Genetyka kliniczna" },
                new SelectListItem { Text = "Geriatria" },
                new SelectListItem { Text = "Ginekologia onkologiczna" },
                new SelectListItem { Text = "Hematologia" },
                new SelectListItem { Text = "Hipertensjologia" },
                new SelectListItem { Text = "Immunologia kliniczna" },
                new SelectListItem { Text = "Intensywna terapia" },
                new SelectListItem { Text = "Kardiochirurgia" },
                new SelectListItem { Text = "Kardiologia" },
                new SelectListItem { Text = "Kardiologia dziecięca" },
                new SelectListItem { Text = "Medycyna lotnicza" },
                new SelectListItem { Text = "Medycyna morska i tropikalna" },
                new SelectListItem { Text = "Medycyna nuklearna" },
                new SelectListItem { Text = "Medycyna paliatywna" },
                new SelectListItem { Text = "Medycyna pracy" },
                new SelectListItem { Text = "Medycyna ratunkowa" },
                new SelectListItem { Text = "Medycyna rodzinna" },
                new SelectListItem { Text = "Medycyna sądowa" },
                new SelectListItem { Text = "Medycyna sportowa" },
                new SelectListItem { Text = "Mikrobiologia lekarska" },
                new SelectListItem { Text = "Nefrologia" },
                new SelectListItem { Text = "Nefrologia dziecięca" },
                new SelectListItem { Text = "Neonatologia" },
                new SelectListItem { Text = "Neurochirurgia" },
                new SelectListItem { Text = "Neurologia" },
                new SelectListItem { Text = "Neurologia dziecięca" },
                new SelectListItem { Text = "Neuropatologia" },
                new SelectListItem { Text = "Okulistyka" },
                new SelectListItem { Text = "Onkologia i hematologia dziecięca" },
                new SelectListItem { Text = "Onkologia kliniczna " },
                new SelectListItem { Text = "Ortodoncja" },
                new SelectListItem { Text = "Ortopedia i traumatologia narządu ruchu" },
                new SelectListItem { Text = "Otorynolaryngologia" },
                new SelectListItem { Text = "Otorynolaryngologia dziecięca" },
                new SelectListItem { Text = "Patomorfologia" },
                new SelectListItem { Text = "Pediatria" },
                new SelectListItem { Text = "Pediatria metaboliczna" },
                new SelectListItem { Text = "Perinatologia" },
                new SelectListItem { Text = "Periodontologia" },
                new SelectListItem { Text = "Położnictwo i ginekologia" },
                new SelectListItem { Text = "Protetyka stomatologiczna" },
                new SelectListItem { Text = "Psychiatria" },
                new SelectListItem { Text = "Psychiatria dzieci i młodzieży" },
                new SelectListItem { Text = "Radiologia i diagnostyka obrazowa" },
                new SelectListItem { Text = "Radioterapia onkologiczna" },
                new SelectListItem { Text = "Rehabilitacja medyczna" },
                new SelectListItem { Text = "Reumatologia" },
                new SelectListItem { Text = "Seksuologia" },
                new SelectListItem { Text = "Stomatologia dziecięca" },
                new SelectListItem { Text = "Stomatologia zachowawcza z endodoncją" },
                new SelectListItem { Text = "Toksykologia kliniczna" },
                new SelectListItem { Text = "Transfuzjologia kliniczna" },
                new SelectListItem { Text = "Transplantologia kliniczna" },
                new SelectListItem { Text = "Urologia" },
                new SelectListItem { Text = "Urologia dziecięca" },
                new SelectListItem { Text = "Zdrowie publiczne " }
            };
            ViewBag.Spec = Specializations;

            foreach (ApplicationUser doc in dbL.Users)
            {
                if (doc.UserName == appointment.docs_pesel)
                {
                    ViewBag.DocsSurname = doc.Surname;
                    ViewBag.opt = appointment.service_type;
                }
            }

            if (ModelState.IsValid)
            {

                int x = 1;
                if (DateTime.Compare(appointment.appoint_date, DateTime.Now) <= 0)
                {
                    ModelState.AddModelError("appoint_date", "Nie można ustawić daty przeszłej.");
                    x = 0;
                }

                foreach (Patient p in dbP.Patients)
                {
                    if (p.Pesel == appointment.patients_pesel)
                    {
                        if (x == 1)
                        {
                            db.Entry(appointment).State = EntityState.Modified;
                            db.SaveChanges();

                            double time = -1;

                            string query = "UPDATE [dbo].[AppointmentsArch] SET DBUSer = '" + CurrentUser + "' WHERE Idd = '" + appointment.ID + "' AND TypeOfChange = 'UPDATED-INSERTED' AND DateOfChange >= '" + DateTime.Now.AddSeconds(time) + "'";
                            db.Database.ExecuteSqlCommand(query);

                            query = "UPDATE [dbo].[AppointmentsArch] SET DBUSer = '" + CurrentUser + "' WHERE Idd = '" + appointment.ID + "' AND TypeOfChange = 'UPDATED-DELETED' AND DateOfChange >= '" + DateTime.Now.AddSeconds(time) + "'";
                            db.Database.ExecuteSqlCommand(query);

                            return RedirectToAction("Index");
                        }
                        else
                            return View(appointment);
                    }
                }
            }
            ModelState.AddModelError("patients_pesel", "Nie ma takiego pacjenta.");

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

            string query = "UPDATE [dbo].[AppointmentsArch] SET DBUSer = '" + CurrentUser + "' WHERE TypeOfChange = 'DELETED' AND Idd = " + appointment.ID;
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
