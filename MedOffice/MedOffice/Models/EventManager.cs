using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DayPilot.Web.Mvc.Recurrence;

namespace TutorialCS
{
    /// <summary>
    /// Summary description for EventManager
    /// </summary>
    public class EventManager
    {
        public DataTable FilteredData(DateTime start, DateTime end)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [WorkingTime] WHERE NOT (([eventend] <= @start) OR ([eventstart] >= @end))", ConfigurationManager.ConnectionStrings["AppointmentDBContext"].ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("start", start);
            da.SelectCommand.Parameters.AddWithValue("end", end);

            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }

        public void EventEdit(string id, string name, DateTime start, DateTime end, string resource)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AppointmentDBContext"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("UPDATE [WorkingTime] SET [name] = @name, [eventstart] = @start, [eventend] = @end, [resource] = @resource WHERE [id] = @id", con);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("start", start);
                cmd.Parameters.AddWithValue("end", end);
                cmd.Parameters.AddWithValue("resource", resource);
                cmd.ExecuteNonQuery();

            }
        }

        public void EventEdit(string id, string name, DateTime start, DateTime end, string resource, string recurrence)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AppointmentDBContext"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("UPDATE [WorkingTime] SET [name] = @name, [eventstart] = @start, [eventend] = @end, [resource] = @resource, [recurrence] = @recurrence WHERE [id] = @id", con);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("start", start);
                cmd.Parameters.AddWithValue("end", end);
                cmd.Parameters.AddWithValue("resource", resource);
                cmd.Parameters.AddWithValue("recurrence", (object)recurrence ?? DBNull.Value);
                cmd.ExecuteNonQuery();

            }
        }

        public DataTable GetResources()
        {
            return GetResources("name");
        }

        public DataTable GetResources(string orderBy)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Resources]", ConfigurationManager.ConnectionStrings["AppointmentDBContext"].ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dt.DefaultView.Sort = orderBy;

            return dt.DefaultView.ToTable();

        }

        public DataTable GetResourcesOneUser(string ID)
        {
            string CurrentUser = System.Web.HttpContext.Current.User.Identity.Name;

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Resources] WHERE name = '" + ID + "'", ConfigurationManager.ConnectionStrings["AppointmentDBContext"].ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt.DefaultView.ToTable();
        }

        public void EventMove(string id, DateTime start, DateTime end, string resource)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AppointmentDBContext"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("UPDATE [WorkingTime] SET [eventstart] = @start, [eventend] = @end, [resource] = @resource WHERE [id] = @id", con);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("start", start);
                cmd.Parameters.AddWithValue("end", end);
                cmd.Parameters.AddWithValue("resource", resource);
                cmd.ExecuteNonQuery();

            }
        }

        public Event Get(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return null;
            }

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [WorkingTime] WHERE id = @id", ConfigurationManager.ConnectionStrings["AppointmentDBContext"].ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("id", id);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                return new Event
                {
                    Id = id,
                    Text = (string)dr["name"],
                    Start = (DateTime)dr["eventstart"],
                    End = (DateTime)dr["eventend"],
                    Resource = new SelectList(ResourceSelectList(), "Value", "Text", dr["resource"]),
                    Recurrence = dr.IsNull("recurrence") ? null : (string)dr["recurrence"]
                };
            }
            return null;
        }

        public IEnumerable<SelectListItem> ResourceSelectList()
        {
            return
                GetResources().AsEnumerable().Select(u => new SelectListItem
                {
                    Value = Convert.ToString(u.Field<int>("id")),
                    Text = u.Field<string>("name")
                });
        }

        internal void EventCreate(DateTime start, DateTime end, string text, string resource, string recurrenceJson)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AppointmentDBContext"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO [WorkingTime] (eventstart, eventend, name, resource) VALUES (@start, @end, @name, @resource); ", con);  // SELECT SCOPE_IDENTITY();
                cmd.Parameters.AddWithValue("start", start);
                cmd.Parameters.AddWithValue("end", end);
                cmd.Parameters.AddWithValue("name", text);
                cmd.Parameters.AddWithValue("resource", resource);
                cmd.ExecuteScalar();

                cmd = new SqlCommand("select @@identity;", con);
                int id = Convert.ToInt32(cmd.ExecuteScalar());

                RecurrenceRule rule = RecurrenceRule.FromJson(id.ToString(), start, recurrenceJson);
                string recurrenceString = rule.Encode();
                if (!String.IsNullOrEmpty(recurrenceString))
                {
                    cmd = new SqlCommand("update [WorkingTime] set [recurrence] = @recurrence where [id] = @id", con);
                    cmd.Parameters.AddWithValue("recurrence", rule.Encode());
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //przeladowane EventCreate dla dodawania wizyt
        internal void EventCreate(DateTime start, DateTime end, string text, string resource, int appointmentID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AppointmentDBContext"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO [WorkingTime] (eventstart, eventend, name, resource, appointment_id) VALUES (@start, @end, @name, @resource, @appointmentID); ", con);
                cmd.Parameters.AddWithValue("start", start);
                cmd.Parameters.AddWithValue("end", end);
                cmd.Parameters.AddWithValue("name", text);
                cmd.Parameters.AddWithValue("resource", resource);
                cmd.Parameters.AddWithValue("appointmentID", appointmentID);
                cmd.ExecuteScalar();

                cmd = new SqlCommand("select @@identity;", con);
                int id = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public class Event
        {
            public string Id { get; set; }
            public string Text { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public SelectList Resource { get; set; }
            public string Recurrence { get; set; }
        }

        public void EventDelete(string id)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AppointmentDBContext"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM [WorkingTime] WHERE id = @id", con);
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public void EventDeleteWholeRecurrence(string recurrence)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AppointmentDBContext"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM [WorkingTime] WHERE recurrence LIKE '" + recurrence + "%'", con);
                //cmd.Parameters.AddWithValue("recurrence", recurrence);
                cmd.ExecuteNonQuery();
            }
        }

        // Recurrence

        public void EventCreateException(DateTime start, DateTime end, string text, string resource, string encodedRecurrence)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AppointmentDBContext"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO [WorkingTime] (eventstart, eventend, name, resource, recurrence) VALUES (@start, @end, @name, @resource, @recurrence); ", con);  // SELECT SCOPE_IDENTITY();
                cmd.Parameters.AddWithValue("start", start);
                cmd.Parameters.AddWithValue("end", end);
                cmd.Parameters.AddWithValue("name", text);
                cmd.Parameters.AddWithValue("resource", resource);
                cmd.Parameters.AddWithValue("recurrence", encodedRecurrence);
                cmd.ExecuteScalar();
            }

        }
    }

}