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

    public class AdminController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }

      

        public IActionResult Dashboard()
        {
            using (var connection = DapperHelper.GetOpenConnection())
            {
                int jobSeekersCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM tblsignup WHERE Role = 1");
                int recruitersCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM tblsignup WHERE Role = 2");
                int adminsCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM tblsignup WHERE Role = 4");

                ViewBag.JobSeekersCount = jobSeekersCount;
                ViewBag.RecruitersCount = recruitersCount;
                ViewBag.AdminsCount = adminsCount;
            }
            // Pass the counts to the view using ViewBag
           
            return View();
        }
        public IActionResult new_admin()
        {
            using (var connection = DapperHelper.GetOpenConnection())
            {
                var countries = connection.Query<country>("GetCountries", commandType: CommandType.StoredProcedure)
                    .Select(c => new SelectListItem { Text = c.cname, Value = c.countryid.ToString() })
                    .ToList();

                ViewBag.Countries = new SelectList(countries, "Value", "Text");

                return View();
            }
        }

        [HttpPost]
        public IActionResult new_admin(signup signup)
        {
            using (var connection = DapperHelper.GetOpenConnection())
            {
                int rowsAffected = connection.Execute("insertdata", new
                {
                    p_Firstname = signup.Firstname,
                    p_Lastname = signup.Lastname,
                    p_Username = signup.Username,
                    p_Password = signup.Password,
                    p_Phonenumber = signup.Phonenumber,

                    p_SelectedCountryId = signup.countryid,
                    p_SelectedStateId = signup.stateid,
                    p_SelectedCityId = signup.cityid,
                    p_Role = signup.Role,
                    p_Gender = signup.Gender,
                    p_Address = signup.Address
                },
                    commandType: CommandType.StoredProcedure);


                var countries = connection.Query<country>("GetCountries", commandType: CommandType.StoredProcedure)
                    .Select(c => new SelectListItem { Text = c.cname, Value = c.countryid.ToString() })
                    .ToList();

                ViewBag.Countries = new SelectList(countries, "Value", "Text");

                return RedirectToAction("Dashboard","Admin");
            }
        }
        public IActionResult admin()
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
        public IActionResult viewresume()
        {
            return View();

        }
        public IActionResult PostJob()
        {
            return View();

        }
       
        [HttpPost]
        public IActionResult PostJob(Jobpost jobpost)
        {
            // Insert the job post into the database
            int rowsAffected = DapperHelper.InsertJobpost(jobpost);

            // Redirect to the job post list page
            return RedirectToAction("Dashboard","Admin");
        }
        public IActionResult joblist()
        {
            using (var connection = DapperHelper.GetOpenConnection())
            {
                var sql = "select id,posted_date, location, company_name, vacancy, job_nature, salary, application_date, job_description, required_skills, experience from job_overview";
                var jobposts = connection.Query<Jobpost>(sql).ToList();
                return View(jobposts);



            }
        }

    }
}
