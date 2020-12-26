using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Globalization;

namespace SQL_CRUD_SP.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Nestedtable()
        {
            return View();
        }
        public ActionResult CrudModel()
        {
            return View();
        }   
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Pagination()
        {
            return View();
        }
        [HttpPost]
        public JObject UpdateUser(string userid, string Firstname, string Lastname, string Emai)
        {
            string ConnectionString = ConfigurationManager.AppSettings["Kiwi_partnersEntities"];

            SqlConnection con = new SqlConnection();

            con.ConnectionString = ConnectionString;

            JArray json;
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();


                cmd.Connection = con;
                if (userid == "true" || userid == "")
                {
                    cmd.CommandText = "User_Insert";
                }
                else
                {
                    cmd.CommandText = "User_Update";
                    cmd.Parameters.AddWithValue("@UserId", userid);
                }
                
                cmd.Parameters.AddWithValue("@Firstname", Firstname);
                cmd.Parameters.AddWithValue("@Lastname", Lastname);
                cmd.Parameters.AddWithValue("@Email", Emai);
                cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                dt.Load(cmd.ExecuteReader());
                json = DataTableToJson(dt);
                con.Close();
                cmd.Dispose();
                cmd = null;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
                con.Dispose();
                con = null;
            }
            return new JObject(new JProperty("User", json));
        }
        public JObject getuserdata(string userid)
        {
            string ConnectionString = ConfigurationManager.AppSettings["Kiwi_partnersEntities"];

            SqlConnection con = new SqlConnection();

            con.ConnectionString = ConnectionString;

            JArray json;
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();


                cmd.Connection = con;
                cmd.CommandText = "getuserdata";
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                dt.Load(cmd.ExecuteReader());
                json = DataTableToJson(dt);
                con.Close();
                cmd.Dispose();
                cmd = null;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
                con.Dispose();
                con = null;
            }

            return new JObject(new JProperty("User", json));
        }
        [HttpPost]
        public JObject Deletedata(string userid)
        {
            string ConnectionString = ConfigurationManager.AppSettings["Kiwi_partnersEntities"];

            SqlConnection con = new SqlConnection();

            con.ConnectionString = ConnectionString;

            JArray json;
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();


                cmd.Connection = con;
                cmd.CommandText = "User_delete";
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                dt.Load(cmd.ExecuteReader());
                json = DataTableToJson(dt);
                con.Close();
                cmd.Dispose();
                cmd = null;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
                con.Dispose();
                con = null;
            }

            return new JObject(new JProperty("User", json));
        }
        public JObject getdata(string Searchdata, int? pagenumber, int? pagesize, string FDate, string EDate )
        {
            string ConnectionString = ConfigurationManager.AppSettings["Kiwi_partnersEntities"];
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConnectionString;
            JArray json;
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "Cte_SearcD";
                cmd.Parameters.AddWithValue("@DisplayLen", pagesize);
                cmd.Parameters.AddWithValue("@DisplayStar", pagenumber );
                cmd.Parameters.AddWithValue("@Sortcol", 10);
                cmd.Parameters.AddWithValue("@Search", Searchdata);
                cmd.Parameters.AddWithValue("@DateFrom", FDate);
                cmd.Parameters.AddWithValue("@DateTo", EDate); 
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                dt.Load(cmd.ExecuteReader());
                json = DataTableToJson(dt);
                con.Close();
                cmd.Dispose();
                cmd = null;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
                con.Dispose();
                con = null;
            }

            return new JObject(new JProperty("User", json));
        }
        public static JArray DataTableToJson(DataTable source)
        {
            JArray result = new JArray();
            JObject row;
            foreach (System.Data.DataRow dr in source.Rows)
            {
                row = new JObject();
                foreach (System.Data.DataColumn col in source.Columns)
                {
                    row.Add(col.ColumnName.Trim(), JToken.FromObject(dr[col]));
                }
                result.Add(row);
            }
            return result;
        }

    }
}