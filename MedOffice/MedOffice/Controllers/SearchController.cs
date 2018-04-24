using MedOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                                              || searching == null);
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
            List<Patient> patientList = patientContext.Patients.Where(x => x.Pesel.Contains(searching)
                                            || x.Name.Contains(searching)
                                            || x.Surname.Contains(searching)
                                            || searching == null).ToList();
            patientList.OrderBy(s => s.Surname);
            return View(patientList);
        }
    }
}