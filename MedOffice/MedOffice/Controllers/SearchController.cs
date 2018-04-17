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

        ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult WorkerSearch(string searching)
        {
            // kryteria wyszukiwania
            return View(context.Users.Where(x=>x.UserName.Contains(searching) 
                                            || x.Name.Contains(searching)
                                            || x.Surname.Contains(searching)
                                            || x.Specialization.Contains(searching)
                                            || searching == null).ToList());
        }
    }
}