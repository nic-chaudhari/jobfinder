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
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace jobfinder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;


        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
    
        public IActionResult Index()
        {

            try
            {
                using var connection = new MySqlConnection("Server=localhost;port=3306;Database=jobfinder;Uid=root;Pwd=root;");
                var tblsignup = connection.Query<signup>("select * from tblsignup ");

            }
            catch (Exception ex)
            {

                throw;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult signup()
        {
            using (var connection = DapperHelper.GetOpenConnection())
            {
                var countries = connection.Query<country>("GetCountries", commandType: CommandType.StoredProcedure)
                    .Select(c => new SelectListItem { Text = c.cname, Value = c.countryid.ToString() })
                    .ToList();

                ViewBag.Countries = new SelectList(countries, "Value", "Text");

                return View(new signup());
            }
        }

        [HttpPost]
        public IActionResult signup(signup signup)
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

                return RedirectToAction("Index");
            }
        }

        public IActionResult GetStatesByCountryId(int countryId)
        {
            using (var connection = DapperHelper.GetOpenConnection())
            {
                var states = connection.Query<state>("GetStatesByCountryId", new { p_countryId = countryId }, commandType: CommandType.StoredProcedure).ToList();
                var stateList = states.Select(s => new SelectListItem { Value = s.stateid.ToString(), Text = s.sname }).ToList();

                return Json(stateList);
            }

        }

        public IActionResult GetCitiesByStateId(int stateId)
        {
            using (var connection = DapperHelper.GetOpenConnection())
            {
                var cities = connection.Query<city>("GetCitiesByStateId", new { p_stateId = stateId }, commandType: CommandType.StoredProcedure)
                    .Select(c => new SelectListItem { Text = c.cname, Value = c.cityid.ToString() }).ToList();

                
                return Json(cities);
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(signup model)
        {
            using (var connection = DapperHelper.GetOpenConnection())
            {
                var sql = "SELECT * FROM tblsignup WHERE Username = @Username AND Password = @Password";
                // Check if the username and password are valid
                var user = connection.QueryFirstOrDefault<signup>(sql, new { model.Username, model.Password });
                //var admin_user= connection.QuerySingleOrDefault<signup>("SELECT * FROM tblsignup WHERE Role = 3 ", new { model.Role});
                if (user != null)
                {
                    if (user.Role == 1) // recruiter
                    {
                       // HttpContext.Session.SetInt32("id", model.Regid);

                        HttpContext.Session.SetString("Username", model.Username);
                        return RedirectToAction("recruiter","Home");
                    }
                    else if (user.Role == 3)
                    {
                        HttpContext.Session.SetString("Username", model.Username);
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if (user.Role == 2) // jobseeker
                    {
                        //HttpContext.Session.SetInt32("id", model.Regid);

                        HttpContext.Session.SetString("Username", model.Username);

                        return RedirectToAction("Jobseeker","Home");
                    }
                    else if (user.Role == 4)
                    {
                        HttpContext.Session.SetString("Username", model.Username);
                        return RedirectToAction("ADashboard","AAdmin");
                    }
                   
                    else
                    {
                        return RedirectToAction("Index");
                    }
                   
                }

                else
                {
                    // Display an error message
                    ModelState.AddModelError("", "Invalid username or password");
                    return View(model);
                }
            }
        }

       
        public IActionResult Jobseeker()
        {
            int id = HttpContext.Session.GetInt32("id") ?? 0;

            string userName = HttpContext.Session.GetString("Username");
            ViewBag.UserName = userName;

            return View();
        }

        public IActionResult Profile()
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
        public IActionResult Edit()
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
        public IActionResult Edit(signup model)
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
                return RedirectToAction("Jobseeker","Home");
            }
        }
        public IActionResult Logout()
        {
            // Clear the authentication cookie
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect the user to the login page
            return RedirectToAction("Index","Home");
        }
        public IActionResult showjob(int id)
        {
            using (var connection = DapperHelper.GetOpenConnection())
            {
                var sql = "select posted_date, location, company_name, vacancy, job_nature, salary, application_date, job_description, required_skills, experience from job_overview";
                var jobposts = connection.Query<Jobpost>(sql).ToList();
                ViewData["JobId"] = id;
                return View(jobposts);



            }
        }
        [HttpPost]
        public IActionResult showjob(JobApplication jobseeker, IFormFile resume, int job_id)
        {
            int rowsAffected = DapperHelper.InsertJobseeker(jobseeker, resume, Convert.ToInt32(RouteData.Values["id"]));
            return RedirectToAction("Index");
        }


        public IActionResult Find_job()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}