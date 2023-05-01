using Microsoft.AspNetCore.Mvc;
using jobfinder.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Dapper;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Connections;
using System.Diagnostics.Metrics;
using System.Data.SqlClient;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

using NuGet.Protocol.Core.Types;

using System.Data;

namespace jobfinder.Controllers
{
    public class AAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
            public IActionResult ADashboard()
            {
            using (var connection = DapperHelper.GetOpenConnection())
            {
                int jobSeekersCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM tblsignup WHERE Role = 1");
                int recruitersCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM tblsignup WHERE Role = 2");

                ViewBag.JobSeekersCount = jobSeekersCount;
                ViewBag.RecruitersCount = recruitersCount;

            }
            // Pass the counts to the view using ViewBag


            return View();
        }

        public IActionResult Aviewresume()
        {
            return View();

        }

        public IActionResult APostJob()
            {
                return View();

            }

            [HttpPost]
            public IActionResult APostJob(Jobpost jobpost)
            {
                // Insert the job post into the database
                int rowsAffected = DapperHelper.InsertJobpost(jobpost);

                // Redirect to the job post list page
                return RedirectToAction("ADashboard", "AAdmin");
            }
            public IActionResult Ajoblist()
            {
                using (var connection = DapperHelper.GetOpenConnection())
                {
                    var sql = "select id,posted_date, location, company_name, vacancy, job_nature, salary, application_date, job_description, required_skills, experience from job_overview";
                    var jobposts = connection.Query<Jobpost>(sql).ToList();
                    return View(jobposts);
                }
            }
        public IActionResult AProfile()
        {
            string userName = HttpContext.Session.GetString("Username");
            ViewBag.UserName = userName;

            using (var connection = DapperHelper.GetOpenConnection())
            {
                var query = "SELECT * FROM tblsignup WHERE Username = @username";
                var records = connection.QueryFirstOrDefault<signup>(query, new { userName = userName });
                return View(records);
            }
        }
        public IActionResult AEdit()
        {
            int? userId = HttpContext.Session.GetInt32("id");
            string userName = HttpContext.Session.GetString("Username");
            ViewBag.UserName = userName;
            using (var connection = DapperHelper.GetOpenConnection())
            {
                var query = "SELECT * FROM tblsignup WHERE Username = @userName";
                var records = connection.QueryFirstOrDefault<signup>(query, new { userName = userName });
                return View(records);
            }
        }

        [HttpPost]
        public IActionResult AEdit(signup model)
        {
            using (var connection = DapperHelper.GetOpenConnection())
            {
                var query = "UPDATE tblsignup SET Firstname = @Firstname, Lastname = @Lastname, Username = @Username, Password = @Password, Phonenumber = @Phonenumber WHERE Username = @userName";
                var parameters = new
                {

                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Username = model.Username,
                    Password = model.Password,
                    Phonenumber = model.Phonenumber
                };
                connection.Execute(query, parameters);
                return RedirectToAction("ADashboard", "AAdmin");
            }
        }

    }
    }


