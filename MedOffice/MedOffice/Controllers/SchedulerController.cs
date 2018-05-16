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

namespace TutorialCS.Controllers
{
    public class SchedulerController : Controller
    {
        //
        // GET: /Scheduler/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Backend()
        {
            return new Dps().CallBack(this);
        }

        class Dps : DayPilotScheduler
        {
            protected override void OnInit(InitArgs e)
            {
                LoadResources();

                UpdateWithMessage("Welcome!", CallBackUpdateType.Full);
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
                    UpdateWithMessage("Recurrence exception was created.");
                }
                else
                {
                    new EventManager().EventMove(e.Id, e.NewStart, e.NewEnd, e.Resource);
                    UpdateWithMessage("The event was resized.");
                }
            }

            protected override void OnEventMove(EventMoveArgs e)
            {
                if (e.Recurrent && !e.RecurrentException)
                {
                    new EventManager().EventCreateException(e.NewStart, e.NewEnd, e.Text, e.NewResource, RecurrenceRule.EncodeExceptionModified(e.RecurrentMasterId, e.OldStart));
                    UpdateWithMessage("Recurrence exception was created.");
                }
                else
                {
                    new EventManager().EventMove(e.Id, e.NewStart, e.NewEnd, e.NewResource);
                    UpdateWithMessage("The event was moved.");
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
                            UpdateWithMessage("Event deleted.");
                        }
                        else
                        {
                            string prefix = RecurrenceRule.Prefix(e.RecurrentMasterId);

                            new EventManager().EventDeleteWholeRecurrence(prefix);
                            UpdateWithMessage("Series of events deleted.");
                        }
                        Update();
                        break;
                }
            }

            protected override void OnCommand(CommandArgs e)
            {
                switch (e.Command)
                {
                    case "refresh":
                        UpdateWithMessage("Refreshed");
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

                // Nie działa, powinny się wyświetlać znaczki na każdym evencie. Może jest coś z Themes, nie wiem.
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
                    e.InnerHtml = String.Format("<span style=\"font - weight: bold\">Tydzień (trzeba dodac obliczanie ktory jest tydzien) </span>");
                }
                if (e.Level == 1)
                {
                    e.InnerHtml = String.Format("<span style=\"font - weight: bold\">{0} </span>", e.Start.ToShortDateString());
                }
                
            }
        }
    }
}
