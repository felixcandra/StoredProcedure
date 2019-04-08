using StoredProcManual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Data;

namespace StoredProcManual.Controllers
{
    public class GuestController : Controller
    {
        MyContext _context = new MyContext();
        // GET: Guest
        public ActionResult Index()
        {
            string CS = ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString;
            using(SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SelectAllGuest", con);
                cmd.CommandType = CommandType.StoredProcedure;

                //in case query bukan select *
                //cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("@Age", SqlDbType.Int, 2).Direction = ParameterDirection.Output;

                con.Open();
                //cmd.ExecuteNonQuery();
                SqlDataReader dr = cmd.ExecuteReader();
                List<Guest> guestList = new List<Guest>();
                while (dr.Read())
                {
                    guestList.Add(new Guest()
                    {
                        //Id = Convert.ToInt32(dr[0]),
                        FirstName = Convert.ToString(dr[1]),
                        LastName = Convert.ToString(dr[2]),
                        Age = Convert.ToInt32(dr[3])
                    });
                }
                return View(guestList.ToList());
            }
        }

        //GET: Guest/Create
        public ActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Age")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                string CS = ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString;
                using(SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("spAddGuest", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FirstName", guest.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", guest.LastName);
                    cmd.Parameters.AddWithValue("@Age", guest.Age);

                    SqlParameter outputParam = new SqlParameter();
                    outputParam.ParameterName = "@Id"; //parameter name
                    outputParam.SqlDbType = System.Data.SqlDbType.Int; //data type
                    outputParam.Direction = System.Data.ParameterDirection.Output; //direction
                    cmd.Parameters.Add(outputParam);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    var guestNewId = outputParam.Value;
                    return RedirectToAction("Index");
                }
            }
            return View(guest);
        }

        //GET: Guest/Edit/1
        public ActionResult Edit(int? id)
        {
            Guest guest = new Guest();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string CS = ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString;
            using(SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spGetGuestById", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Age", SqlDbType.Int, 2).Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();

                guest.FirstName = Convert.ToString(cmd.Parameters["@FirstName"].Value);
                guest.LastName = Convert.ToString(cmd.Parameters["@LastName"].Value);
                guest.Age = Convert.ToInt16(cmd.Parameters["@Age"].Value);
                return View(guest);
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Age")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                string CS = ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString;
                using(SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("spUpdateGuest", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", guest.Id);
                    cmd.Parameters.AddWithValue("@FirstName", guest.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", guest.LastName);
                    cmd.Parameters.AddWithValue("@Age", guest.Age);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
            return View(guest);
        }

        //GET: Guest/Delete
        public ActionResult Delete(int? id)
        {
            Guest guest = new Guest();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string CS = ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spGetGuestById", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Age", SqlDbType.Int, 2).Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();

                guest.FirstName = Convert.ToString(cmd.Parameters["@FirstName"].Value);
                guest.LastName = Convert.ToString(cmd.Parameters["@LastName"].Value);
                guest.Age = Convert.ToInt16(cmd.Parameters["@Age"].Value);
                return View(guest);
            }
        }

        //DELETE
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            string CS = ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString;
            using(SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spDeleteGuest", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();

                return RedirectToAction("Index");
            }
        }
    }
}