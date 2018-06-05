using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Enums;
using DayPilot.Web.Mvc.Events.Scheduler;
using DayPilot.Web.Mvc.Json;
using DayPilot.Web.Mvc.Recurrence;
using DayPilot.Web.Mvc.Data;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MedOffice.Models;
using System.Net;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TutorialCS.Controllers
{
    public class SchedulerController : Controller
    {
        // GET: /Scheduler/id
        public ActionResult Index(string Id)
        {
            string name = Id;

            if (name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AppointmentDBContext db = new AppointmentDBContext();
            Resources Resource = db.Resources.Find(name);
           
            ApplicationDbContext db2 = new ApplicationDbContext();
            ApplicationUser usr = db2.Users
                .Where(y => y.UserName.Equals(name))
                .ToList()
                .FirstOrDefault();

            var RoleName = db2.UsersArch
                .Where(y => y.UserName.Equals(name) && y.TypeOfChange.Equals("INSERTED"))
                .Select(y => y.RId)
                .FirstOrDefault();

            usr = db2.Users.Find(usr.Id);
            ViewBag.RoleName = RoleName.ToLower();

            if (Resource == null || usr == null)
            {
                return HttpNotFound();
            }

            Session["ID"] = name;
            return View(usr);
        }

        [Authorize(Roles = "Administrator, Kierownik, Rejestrujący, Lekarz")]
        public ActionResult Appointments(string viewType)
        {
            Session["ID"] = "wizyty";
            ViewBag.Type = viewType;
            return View();
        }

        public ActionResult Backend()
        {
            string ID = Session["ID"].ToString();

            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return new Dps(ID).CallBack(this);
        }

    class Dps : DayPilotScheduler
        {
            private string ID;

            public Dps(string ID)
            {
                this.ID = ID;
            }

            private string CurrentUser = System.Web.HttpContext.Current.User.Identity.Name;
            private bool RoleChecker = System.Web.HttpContext.Current.User.IsInRole("Administrator") || System.Web.HttpContext.Current.User.IsInRole("Kierownik");

            protected override void OnInit(InitArgs e)
            {
                LoadResourcesOneUser(ID);
                UpdateWithMessage("Wita!", CallBackUpdateType.Full);
            }

            private void LoadResources(string orderBy)
            {
                Resources.Clear();
                foreach (DataRow r in new EventManager().GetResources(orderBy).Rows)
                {
                    Resource res = new Resource((string)r["name"], Convert.ToString(r["id"]));
                    res.DataItem = r;
                    Resources.Add(res);
                }
            }

            private void LoadResourcesOneUser(string ID)
            {
                Resources.Clear();
                foreach (DataRow r in new EventManager().GetResourcesOneUser(ID).Rows)
                {
                    Resource res = new Resource((string)r["name"], Convert.ToString(r["id"]));
                    res.DataItem = r;
                    Resources.Add(res);
                }
            }

            private void LoadResources()
            {
                LoadResources("name");
            }

            protected override void OnBeforeResHeaderRender(BeforeResHeaderRenderArgs e)
            {
                e.Columns[0].Html = "" + e.DataItem["fsname"];
            }

            protected override void OnEventResize(EventResizeArgs e)
            {
                if (e.Recurrent && !e.RecurrentException)
                {
                    new EventManager().EventCreateException(e.NewStart, e.NewEnd, e.Text, e.Resource, RecurrenceRule.EncodeExceptionModified(e.RecurrentMasterId, e.OldStart));
                    UpdateWithMessage("Wyjątek od powtarzalnego wydarzenia został utworzony.");
                }
                else
                {
                    new EventManager().EventMove(e.Id, e.NewStart, e.NewEnd, e.Resource);
                    UpdateWithMessage("Czas trwania wydarzenia został zmieniony.");
                }
            }

            protected override void OnEventMove(EventMoveArgs e)
            {
                if (e.Recurrent && !e.RecurrentException)
                {
                    new EventManager().EventCreateException(e.NewStart, e.NewEnd, e.Text, e.NewResource, RecurrenceRule.EncodeExceptionModified(e.RecurrentMasterId, e.OldStart));
                    UpdateWithMessage("Wyjątek od powtarzalnego wydarzenia został utworzony.");
                }
                else
                {
                    new EventManager().EventMove(e.Id, e.NewStart, e.NewEnd, e.NewResource);
                    UpdateWithMessage("Wydarzenie zostało przeniesione.");
                }
            }

            protected override void OnEventMenuClick(EventMenuClickArgs e)
            {
                switch (e.Command)
                {
                    case "Delete":
                        if (e.Recurrent == false)
                        {
                            new EventManager().EventDelete(e.Id);
                            UpdateWithMessage("Wydarzenie usunięte.");
                        }
                        else
                        {
                            string prefix = RecurrenceRule.Prefix(e.RecurrentMasterId);

                            new EventManager().EventDeleteWholeRecurrence(prefix);
                            UpdateWithMessage("Seria wydarzeń usunięta.");
                        }
                        Update();
                        break;

                    case "Edit":
                        Redirect("/Appointments/Edit/"+e.Text);
                        Update();
                        break;

                }
            }

            protected override void OnCommand(CommandArgs e)
            {
                switch (e.Command)
                {
                    case "refresh":
                        UpdateWithMessage("Harmonogram odświeżony.");
                        break;
                    case "sort":
                        LoadResources((string)e.Data["field"]);
                        Update(CallBackUpdateType.Full);
                        break;
                }
            }

            protected override void OnFinish()
            {
                if (UpdateType == CallBackUpdateType.None)
                {
                    return;
                }

                Events = new EventManager().FilteredData(StartDate, StartDate.AddDays(Days)).AsEnumerable();

                DataIdField = "id";
                DataTextField = "name";
                DataStartField = "eventstart";
                DataEndField = "eventend";
                DataResourceField = "resource";
                DataRecurrenceField = "recurrence";
            }

            protected override void OnBeforeEventRender(BeforeEventRenderArgs e)
            {
                if (e.Recurrent)
                {
                    if (e.RecurrentException)
                    {
                        e.Areas.Add(new Area().Right(5).Top(5).CssClass("area_recurring_ex"));
                    }
                    else
                    {
                        e.Areas.Add(new Area().Right(5).Top(5).CssClass("area_recurring"));
                    }
                }
            }

            protected override void OnBeforeTimeHeaderRender(BeforeTimeHeaderRenderArgs e)
            {
                if (e.Level == 0)
                {
                    e.InnerHtml = String.Format("<span style=\"font - weight: bold\">{0} - {1}</span>", e.Start.Day, e.Start.AddDays(6).ToShortDateString());
                }

            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public ActionResult DailyScheduler()
        {
            return View();
        }

        public ActionResult DailyBackend()
        {
            return new DpsDaily().CallBack(this);
        }

        class DpsDaily : DayPilotScheduler
        {
            protected override void OnInit(InitArgs e)
            {
                LoadResources();
                UpdateWithMessage("Witaj!", CallBackUpdateType.Full);
            }

            private void LoadResources(string orderBy)
            {
                Resources.Clear();

                foreach (DataRow r in new EventManager().GetResources(orderBy).Rows)
                {
                    Resource res = new Resource((string)r["name"], Convert.ToString(r["id"]));

                    res.DataItem = r;
                    res.Columns.Add(new ResourceColumn(r["fsname"].ToString()));
                    Resources.Add(res);
                }
            }

            private void LoadResources()
            {
                LoadResources("name");
            }

            protected override void OnEventResize(EventResizeArgs e)
            {
                if (e.Recurrent && !e.RecurrentException)
                {
                    new EventManager().EventCreateException(e.NewStart, e.NewEnd, e.Text, e.Resource, RecurrenceRule.EncodeExceptionModified(e.RecurrentMasterId, e.OldStart));
                    UpdateWithMessage("Wyjątek od powtarzalnego wydarzenia został utworzony.");
                }
                else
                {
                    new EventManager().EventMove(e.Id, e.NewStart, e.NewEnd, e.Resource);
                    UpdateWithMessage("Czas trwania wydarzenia został zmieniony.");
                }
            }

            protected override void OnEventMove(EventMoveArgs e)
            {
                if (e.Recurrent && !e.RecurrentException)
                {
                    new EventManager().EventCreateException(e.NewStart, e.NewEnd, e.Text, e.NewResource, RecurrenceRule.EncodeExceptionModified(e.RecurrentMasterId, e.OldStart));
                    UpdateWithMessage("Wyjątek od powtarzalnego wydarzenia został utworzony.");
                }
                else
                {
                    new EventManager().EventMove(e.Id, e.NewStart, e.NewEnd, e.NewResource);
                    UpdateWithMessage("Wydarzenie zostało przeniesione.");
                }
            }

            protected override void OnEventMenuClick(EventMenuClickArgs e)
            {
                switch (e.Command)
                {
                    case "Delete":
                        if (e.Recurrent == false)
                        {
                            new EventManager().EventDelete(e.Id);
                            UpdateWithMessage("Wydarzenie usunięte.");
                        }
                        else
                        {
                            string prefix = RecurrenceRule.Prefix(e.RecurrentMasterId);

                            new EventManager().EventDeleteWholeRecurrence(prefix);
                            UpdateWithMessage("Seria wydarzeń usunięta.");
                        }
                        Update();
                        break;

                    case "Edit":
                        Redirect("/Appointments/Edit/" + e.Text);
                        Update();
                        break;
                }
            }

            protected override void OnCommand(CommandArgs e)
            {
                switch (e.Command)
                {
                    case "refresh":
                        UpdateWithMessage("Harmonogram odświeżony.");
                        break;
                    case "sort":
                        LoadResources((string)e.Data["field"]);
                        Update(CallBackUpdateType.Full);
                        break;
                    case "navigate":
                        StartDate = (DateTime)e.Data["start"];
                        Update(CallBackUpdateType.Full);
                        break;
                }
            }

            protected override void OnFinish()
            {
                if (UpdateType == CallBackUpdateType.None)
                {
                    return;
                }

                Events = new EventManager().FilteredData(StartDate, StartDate.AddDays(Days)).AsEnumerable();

                DataIdField = "id";
                DataTextField = "name";
                DataStartField = "eventstart";
                DataEndField = "eventend";
                DataResourceField = "resource";
                DataRecurrenceField = "recurrence";
            }

            protected override void OnBeforeEventRender(BeforeEventRenderArgs e)
            {
                if (e.Recurrent)
                {
                    if (e.RecurrentException)
                    {
                        e.Areas.Add(new Area().Right(5).Top(5).CssClass("area_recurring_ex"));
                    }
                    else
                    {
                        e.Areas.Add(new Area().Right(5).Top(5).CssClass("area_recurring"));
                    }
                }
            }

            protected override void OnBeforeTimeHeaderRender(BeforeTimeHeaderRenderArgs e)
            {
                if (e.Level == 0)
                {
                    e.InnerHtml = String.Format("<span style=\"font - weight: bold\">{0} </span>", e.Start.ToShortDateString());
                }
            }
        }

        public ActionResult NavigatorBackend()
        {
            return new Dpn().CallBack(this);
        }

        public class Dpn : DayPilotNavigator
        {
            protected override void OnFinish()
            {
                Events = new EventManager().FilteredData(StartDate, StartDate.AddDays(1)).AsEnumerable();

                DataIdField = "id";
                DataStartField = "eventstart";
                DataEndField = "eventend";
                DataRecurrenceField = "recurrence"; // Nie działa!
            }
        }
    }
}
