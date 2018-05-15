using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Web;
using System.Web.Mvc;
using DayPilot.Web.Mvc.Json;
using DayPilot.Web.Mvc.Recurrence;
using TutorialCS;

public class EventController : Controller
{
    public ActionResult Edit(string id)
    {
        var mode = Mode(id);

        ViewData["Mode"] = mode;

        var masterId = Request.QueryString["master"];

        var row = new EventManager().Get(id) ?? new EventManager.Event();
        var master = new EventManager().Get(masterId) ?? new EventManager.Event();
        var e = new EventManager.Event();
        RecurrenceRule _rule;

        switch (mode)
        {
            case EventMode.Master:
                _rule = RecurrenceRule.Decode(master.Recurrence);
                e.Start = master.Start;
                e.End = master.End;
                e.Text = master.Text;
                e.Resource = master.Resource;
                break;
            case EventMode.NewException:
                _rule = RecurrenceRule.Exception;
                DateTime start = Occurrence;
                TimeSpan duration = master.End - master.Start;
                DateTime end = start + duration;
                e.Start = start;
                e.End = end;
                e.Text = master.Text;
                e.Resource = master.Resource;
                break;
            case EventMode.Exception:
                _rule = RecurrenceRule.Exception;
                e.Start = row.Start;
                e.End = row.End;
                e.Text = row.Text;
                e.Resource = row.Resource;
                break;
            case EventMode.Regular:
                _rule = RecurrenceRule.NoRepeat;
                e.Start = row.Start;
                e.End = row.End;
                e.Text = row.Text;
                e.Resource = row.Resource;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        ViewData["RecurrenceJson"] = new HtmlString(_rule.ToJson());

        return View(e);
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult Edit(FormCollection form)
    {
        string id = form["Id"];
        string masterId = Request.QueryString["master"];

        string recurrence = form["Recurrence"];
        DateTime start = Convert.ToDateTime(form["Start"]);
        DateTime end = Convert.ToDateTime(form["End"]);
        string text = form["Text"];
        string resource = form["Resource"];

        var row = new EventManager().Get(id) ?? new EventManager.Event();
        var master = new EventManager().Get(masterId) ?? new EventManager.Event();

        switch (Mode(id))
        {
            case EventMode.Master:
                RecurrenceRule rule = RecurrenceRule.FromJson(masterId, master.Start, recurrence);
                new EventManager().EventEdit(masterId, text, start, end, resource, rule.Encode());
                break;
            case EventMode.NewException:
                new EventManager().EventCreateException(start, end, text, resource, RecurrenceRule.EncodeExceptionModified(masterId, Occurrence));
                break;
            case EventMode.Exception:
                new EventManager().EventEdit(id, text, start, end, resource);
                break;
            case EventMode.Regular:
                new EventManager().EventEdit(id, text, start, end, resource, RecurrenceRule.FromJson(id, row.Start, recurrence).Encode());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return JavaScript(SimpleJsonSerializer.Serialize("OK"));
    }

    public ActionResult RecurrentEditMode()
    {
        return View();
    }

    public ActionResult RecurrentDeleteMode()
    {
        return View();
    }


    public ActionResult Create()
    {

        ViewData["RecurrenceJson"] = new HtmlString(RecurrenceRule.NoRepeat.ToJson());

        return View(new EventManager.Event
        {
            Start = Convert.ToDateTime(Request.QueryString["start"]),
            End = Convert.ToDateTime(Request.QueryString["end"]),
            Resource = new SelectList(new EventManager().ResourceSelectList(), "Value", "Text", Request.QueryString["resource"])
        });
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult Create(FormCollection form)
    {
        DateTime start = Convert.ToDateTime(form["Start"]);
        DateTime end = Convert.ToDateTime(form["End"]);
        string text = form["Text"];
        string resource = form["Resource"];
        string recurrence = form["Recurrence"];

        new EventManager().EventCreate(start, end, text, resource, recurrence);
        return JavaScript(SimpleJsonSerializer.Serialize("OK"));
    }

    public ActionResult New()
    {
        return View();
    }

    #region Recurrence helpers

    private DateTime Occurrence
    {
        get
        {
            return Convert.ToDateTime(Request.QueryString["start"]);
        }
    }

    private EventMode Mode(string id)
    {
        if (!String.IsNullOrEmpty(Request.QueryString["master"]))
        {
            if (!String.IsNullOrEmpty(Request.QueryString["start"]))
            {
                return EventMode.NewException;
            }
            if (!String.IsNullOrEmpty(id))
            {
                return EventMode.Exception;
            }
            return EventMode.Master;
        }

        return EventMode.Regular;
    }

    enum EventMode
    {
        Master,
        NewException,
        Exception,
        Regular
    }

    #endregion

}
