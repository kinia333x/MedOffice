using MedOffice.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MedOffice.Controllers
{
    public class SearchController : Controller
    {
<<<<<<< HEAD
<<<<<<< HEAD
=======
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

        private string CurrentUser = System.Web.HttpContext.Current.User.Identity.Name;
>>>>>>> master
=======
        private string CurrentUser = System.Web.HttpContext.Current.User.Identity.Name;
>>>>>>> master

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
                ViewBag.RoleSortParm = sortOrder == "role" ? "role_desc" : "role";

                ApplicationDbContext context = new ApplicationDbContext();

              
                var userList = (from u in context.Users
                             let query = (from ur in context.Set<IdentityUserRole>()
                                          where ur.UserId.Equals(u.Id)
                                          join r in context.Roles on ur.RoleId equals r.Id
                                          select r.Name)
                             select new UserSearchModel() { User = u, Role = query.ToList<string>().FirstOrDefault() });

                userList = userList.Where(x => (x.User.UserName.Contains(searching)
                                                  || x.User.Name.Contains(searching)
                                                  || x.User.Surname.Contains(searching)
                                                  || x.Role.Contains(searching)
                                                  || x.User.Specialization.Contains(searching)
                                                  || searching == null)
                                                  && x.User.UserName != "admin");

               
                switch (sortOrder)
                {
                    case "surname_desc": userList = userList.OrderByDescending(s => s.User.Surname); break;
                    case "name": userList = userList.OrderBy(s => s.User.Name); break;
                    case "name_desc": userList = userList.OrderByDescending(s => s.User.Name); break;
                    case "userName": userList = userList.OrderBy(s => s.User.UserName); break;
                    case "userName_desc": userList = userList.OrderByDescending(s => s.User.UserName); break;
                    case "specialization": userList = userList.OrderBy(s => s.User.Specialization); break;
                    case "specialization_desc": userList = userList.OrderByDescending(s => s.User.Specialization); break;
                    case "role": userList = userList.OrderBy(s => s.Role); break;
                    case "role_desc": userList = userList.OrderByDescending(s => s.Role); break;
                    default: userList = userList.OrderBy(s => s.User.Surname); break;
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
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.TotalPriceSortParm = sortOrder == "totalPrice" ? "totalPrice_desc" : "totalPrice";

            AppointmentDBContext context = new AppointmentDBContext();
            // kryteria wyszukiwania
            //char[] year = new char[5];
            //char[] month = new char[3];
            //char[] day = new char[3];

            //if (searching != null && searching.Length == 10)
            //{
            //    year[0] = searching[0];
            //    year[1] = searching[1];
            //    year[2] = searching[2];
            //    year[3] = searching[3];

            //    month[0] = searching[5];
            //    month[1] = searching[6];

            //    day[0] = searching[8];
            //    day[1] = searching[9];
            //}

            var appointmentsList = context.Appointments.Where(x => x.service_name.Contains(searching) //.AsEnumerable()
                                            //|| (String.Format("{0}-{1}-{2}", x.appoint_date.Year, x.appoint_date.Month, x.appoint_date.Day) == searching)
                                            || (x.service_price.ToString() == searching)
                                            || (x.supplies_price.ToString() == searching)
                                            || ((x.supplies_price + x.service_price).ToString() == searching)
                                            || searching == null);

            switch (sortOrder)
            {
                case "name_desc": appointmentsList = appointmentsList.OrderByDescending(s => s.service_name); break;
                case "price": appointmentsList = appointmentsList.OrderBy(s => s.service_price); break;
                case "price_desc": appointmentsList = appointmentsList.OrderByDescending(s => s.service_price); break;
                case "addPrice": appointmentsList = appointmentsList.OrderBy(s => s.supplies_price); break;
                case "addPrice_desc": appointmentsList = appointmentsList.OrderByDescending(s => s.supplies_price); break;
                case "date": appointmentsList = appointmentsList.OrderBy(s => s.appoint_date); break;
                case "date_desc": appointmentsList = appointmentsList.OrderByDescending(s => s.appoint_date); break;
                case "totalPrice": appointmentsList = appointmentsList.OrderBy(x => (x.supplies_price + x.service_price)); break;
                case "totalPrice_desc": appointmentsList = appointmentsList.OrderByDescending(x => (x.supplies_price + x.service_price)); break;
                default: appointmentsList = appointmentsList.OrderBy(s => s.service_name); break;
            }

            return View(appointmentsList.ToList());
        }

        // GET: Search/Edit/5
        public ActionResult Edit(string Id)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = context.Users.Find(Id);

            // VV EMAIL i SENIORITY nie dzialaja
            var usr = new EditViewModel { Email = user.Email, UserName = user.UserName, Name = user.Name, Surname = user.Surname, Seniority = user.Seniority };

            if (user == null)
            {
                return HttpNotFound();
            }

            //}
            return View(usr);
        }


        // POST: Search/Edit/5 
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,Name,Surname,UserName,Specialization,Roles")] ApplicationUser user)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            AppointmentDBContext Appdb = new AppointmentDBContext();

            if (ModelState.IsValid)
            {
                double time = -1;

                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();

                string query = "UPDATE [dbo].[UsersArch] SET RId = (SELECT RId FROM [dbo].[UsersArch] WHERE TypeOfChange = 'INSERTED' AND UserName = " + user.UserName + "), DBUSer = '" + CurrentUser + "' WHERE TypeOfChange = 'UPDATED-INSERTED' AND UserName = " + user.UserName + " AND DateOfChange >= '" + DateTime.Now.AddSeconds(time) + "'"; ;
                context.Database.ExecuteSqlCommand(query);

                query = "UPDATE [dbo].[UsersArch] SET RId = (SELECT RId FROM [dbo].[UsersArch] WHERE TypeOfChange = 'INSERTED' AND UserName = " + user.UserName + "), DBUSer = '" + CurrentUser + "' WHERE TypeOfChange = 'UPDATED-DELETED' AND UserName = " + user.UserName + " AND DateOfChange >= '" + DateTime.Now.AddSeconds(time) + "'";
                context.Database.ExecuteSqlCommand(query);

                query = "UPDATE [dbo].[Resources] SET fsname = '" + user.Name + " " + user.Surname + "' WHERE name = " + user.UserName + "";
                Appdb.Database.ExecuteSqlCommand(query);

                return RedirectToAction("WorkerSearch");
            }
            return View(user);
        }
    }
}
