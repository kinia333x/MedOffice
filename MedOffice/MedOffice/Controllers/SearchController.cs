﻿using MedOffice.Models;
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



        [Authorize(Roles = "Administrator, Kierownik")]
        public ActionResult WorkerSearch(string searching, string sortOrder)
        {
            ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "surname_desc" : "";
            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.UserNameSortParm = sortOrder == "userName" ? "userName_desc" : "userName";
            ViewBag.SpecializationSortParm = sortOrder == "specialization" ? "specialization_desc" : "specialization";

            ApplicationDbContext context = new ApplicationDbContext();

            //           select new
            //           {
            //               UserName = s.UserName,
            //               Name = s.Name,
            //               Role = sa.Name,
            //               Specialization = s.Specialization,
            //               Surname = s.Surname

            //           };

            var userList = (from s in context.Users
                           select s);


            userList = userList.Where(x => x.UserName.Contains(searching)
                                              || x.Name.Contains(searching)
                                              || x.Surname.Contains(searching)
                                              || x.Specialization.Contains(searching)
                                              || searching == null);

            switch (sortOrder)
            {
                case "surname_desc": userList = userList.OrderByDescending(s => s.Surname); break;
                case "name": userList = userList.OrderBy(s => s.Name); break;
                case "name_desc": userList = userList.OrderByDescending(s => s.Name); break;
                case "userName": userList = userList.OrderBy(s => s.UserName); break;
                case "userName_desc": userList = userList.OrderByDescending(s => s.UserName); break;
                case "specialization": userList = userList.OrderBy(s => s.Specialization); break;
                case "specialization_desc": userList = userList.OrderByDescending(s => s.Specialization); break;
                default: userList = userList.OrderBy(s => s.Surname); break;
            }
            
            return View(userList.ToList());
        }



        [Authorize(Roles = "Administrator, Kierownik, Rejestrujący, Lekarz")]
        public ActionResult PatientSearch(string searching, string sortOrder)
        {
            ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "surname_desc" : "";
            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.PeselSortParm = sortOrder == "pesel" ? "pesel_desc" : "pesel";

            PatientDBContext patientContext = new PatientDBContext();
            // kryteria wyszukiwania
            var patientList = patientContext.Patients.Where(x => x.Pesel.Contains(searching)
                                            || x.Name.Contains(searching)
                                            || x.Surname.Contains(searching)
                                            || searching == null);

            switch (sortOrder)
            {
                case "surname_desc": patientList = patientList.OrderByDescending(s => s.Surname); break;
                case "name": patientList = patientList.OrderBy(s => s.Name); break;
                case "name_desc": patientList = patientList.OrderByDescending(s => s.Name); break;
                case "pesel": patientList = patientList.OrderBy(s => s.Pesel); break;
                case "pesel_desc": patientList = patientList.OrderByDescending(s => s.Pesel); break;
                default: patientList = patientList.OrderBy(s => s.Surname); break;
            }

            return View(patientList.ToList());
        }


        [Authorize(Roles = "Administrator, Kierownik, Rejestrujący, Lekarz")]
        public ActionResult AppointmentSearch(string searching, string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "price" ? "price_desc" : "price";
            ViewBag.AddPriceSortParm = sortOrder == "addPrice" ? "addPrice_desc" : "addPrice";
            ViewBag.TotalPriceSortParm = sortOrder == "totalPrice" ? "totalPrice_desc" : "totalPrice";

            AppointmentDBContext context = new AppointmentDBContext();
            // kryteria wyszukiwania
            var appointmentsList = context.Appointments.Where(x => x.service_name.Contains(searching)
                                            || x.service_price.ToString().Contains(searching)
                                            || x.supplies_price.ToString().Contains(searching)
                                            || (x.supplies_price + x.service_price).ToString().Contains(searching)
                                            || searching == null);

            switch (sortOrder)
            {
                case "name_desc": appointmentsList = appointmentsList.OrderByDescending(s => s.service_name); break;
                case "price": appointmentsList = appointmentsList.OrderBy(s => s.service_price); break;
                case "price_desc": appointmentsList = appointmentsList.OrderByDescending(s => s.service_price); break;
                case "addPrice": appointmentsList = appointmentsList.OrderBy(s => s.supplies_price); break;
                case "addPrice_desc": appointmentsList = appointmentsList.OrderByDescending(s => s.supplies_price); break;
                case "totalPrice": appointmentsList = appointmentsList.OrderBy(x => (x.supplies_price + x.service_price).ToString()); break;
                case "totalPrice_desc": appointmentsList = appointmentsList.OrderByDescending(x => (x.supplies_price + x.service_price).ToString()); break;
                default: appointmentsList = appointmentsList.OrderBy(s => s.service_name); break;
            }

            return View(appointmentsList.ToList());
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
