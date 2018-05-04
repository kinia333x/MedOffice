using MedOffice.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcRazorToPdf;

namespace MedOffice.Controllers
{
    [Authorize(Roles = "Administrator, Kierownik, Księgowa")]
    public class AccountantController : Controller
    {
        private AppointmentDBContext db = new AppointmentDBContext();

        // GET: Accountant
        public ActionResult Index()
        {
            return View(db.Appointments.ToList());
        }
        
        public ActionResult Report()
        {
            return new PdfActionResult("Report", db.Appointments.ToList());
        }
    }
}
