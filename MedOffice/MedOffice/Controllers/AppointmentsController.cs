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
            ViewBag.D = new List<SelectListItem>();
            ViewBag.A = "...";
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
            ViewBag.D = dbL.Users.Where(u => u.UserName == "").ToList();

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

            List<SelectListItem> Docs = new List<SelectListItem>();
            ViewBag.D = Docs;
            ViewBag.A = "...";
            if (ModelState.IsValid)
            {

                /*String s = appointment.service_time.ToString();
                int x = 0;

                if (Int32.TryParse(s, out x))
                {
                    if (x <=0)
                    {
                        ModelState.AddModelError("service_time", "Proszę podać czas w minutachhhhhhh1.");
                        return View(appointment);
                    }

                }else
                {
                    ModelState.AddModelError("service_time", "Proszę podać czas w minutachhhhhhh2.");
                    return View(appointment);
                }*/


                foreach(Patient p in dbP.Patients)
                {
                    if (p.Pesel == appointment.patients_pesel)
                    {
                        {
                            db.Appointments.Add(appointment);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            ModelState.AddModelError("patients_pesel", "Nie ma takiego pacjenta.");
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
            List<SelectListItem> Specializations = new List<SelectListItem>()
            {
                new SelectListItem { Text = "- Wybierz jedno -" },
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

            List<SelectListItem> Specializations = new List<SelectListItem>()
            {
                new SelectListItem { Text = "- Wybierz jedno -" },
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
                foreach (Patient p in dbP.Patients)
                {
                    if (p.Pesel == appointment.patients_pesel)
                    {
                        {
                            db.Entry(appointment).State = EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
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
