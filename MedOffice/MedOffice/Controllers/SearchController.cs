using MedOffice.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MedOffice.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search

        
        

        public ActionResult WorkerSearch(string searching, int sortOpt = 1)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            //var list = from s in context.Users
            //           join sa in context.Roles on s.Id equals sa.Id
            //           select new
            //           {
            //               UserName = s.UserName,
            //               Name = s.Name,
            //               Role = sa.Name,
            //               Specialization = s.Specialization,
            //               Surname = s.Surname

            //           };


            // kryteria wyszukiwania
            var userList = context.Users.Where(x => x.UserName.Contains(searching)
                                              || x.Name.Contains(searching)
                                              || x.Surname.Contains(searching)
                                              || x.Specialization.Contains(searching)
                                              || searching == null).ToList().OrderBy(s => s.Surname);
            switch (sortOpt) // to gówno nie działa
            {
                case 1: userList.ToList().OrderBy(s => s.Name); break;
                case 2: userList.ToList().OrderBy(s => s.Surname); break;
                case 3: userList.ToList().OrderBy(s => s.UserName); break;
                case 4: userList.ToList().OrderBy(s => s.Specialization); break;
            }
            
            return View(userList);
        }


        

        public ActionResult PatientSearch(string searching)
        {
            PatientDBContext patientContext = new PatientDBContext();
            // kryteria wyszukiwania
            var patientList = patientContext.Patients.Where(x => x.Pesel.Contains(searching)
                                            || x.Name.Contains(searching)
                                            || x.Surname.Contains(searching)
                                            || searching == null).ToList().OrderBy(s => s.Surname);
            return View(patientList);
        }

        // GET: Patients/Edit/5
        public ActionResult Edit(string Id)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = context.Users.Find(Id);
            if (user == null)
            {
                return HttpNotFound();
            }
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

            return View(user);
        }

        // POST: Patients/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,UserName,Specialization")] ApplicationUser user)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            if (ModelState.IsValid)
            {
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("WorkerSearch");
            }
            return View(user);
        }

    }
}