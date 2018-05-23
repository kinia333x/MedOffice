using MedOffice.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MedOffice.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search

        //public async Task<ActionResult> GetRolesForUser(string userId)  // moze sie przyda
        //{
        //    using (
        //        var userManager =
        //            new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        //    {
        //        var rolesForUser = await userManager.GetRolesAsync(userId);

        //        return this.View(rolesForUser);
        //    }
        //}



        [Authorize(Roles = "Administrator, Kierownik")]
        public ActionResult WorkerSearch(string searching, string sortOrder)
        {
            using (var userManager =
                    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
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

                //var Roles = userManager.GetRolesAsync(userManager.Users.Id);
                var userList = (from s in context.Users
                                select s);

                userList = userList.Where(x => (x.UserName.Contains(searching)
                                                  || x.Name.Contains(searching)
                                                  || x.Surname.Contains(searching)
                                                  || x.Specialization.Contains(searching)
                                                  || searching == null)
                                                  && x.UserName != "admin");

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
                case "totalPrice": appointmentsList = appointmentsList.OrderBy(x => (x.supplies_price + x.service_price)); break;
                case "totalPrice_desc": appointmentsList = appointmentsList.OrderByDescending(x => (x.supplies_price + x.service_price)); break;
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
            var usr = new EditViewModel { UserName = user.UserName, Name = user.Name, Surname = user.Surname};

            if (user == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole("Administrator"))
            {
                ViewBag.UserRoles = new SelectList(context.Roles.Where(u => !u.Name.Contains("Administrator"))
                                            .ToList(), "Name", "Name");
            }
            else if (User.IsInRole("Manager"))
            {
                ViewBag.UserRoles = new SelectList(context.Roles.Where(u => !u.Name.Contains("Administrator") && !u.Name.Contains("Manager"))
                               .ToList(), "Name", "Name");
            }
            else
            {
                return HttpNotFound();
            }
            
            //}
            return View(usr);
        }


        // POST: Patients/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,UserName,Specialization,Roles")] ApplicationUser user)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            if (ModelState.IsValid)
            {
                
                //if (user.Roles.FirstOrDefault != "d843c219-de88-4571-83b6-0b5ed9bf90d7")
                //{
                //    user.Specialization = null;
                //}

                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("WorkerSearch");
            }
            return View(user);
        }
        
    }
}
