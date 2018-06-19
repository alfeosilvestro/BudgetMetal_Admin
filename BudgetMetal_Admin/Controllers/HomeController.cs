using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BudgetMetal_Admin.Models;
using Microsoft.AspNetCore.Http;
using BudgetMetal_Admin.DB;
using Microsoft.Extensions.Configuration;

namespace BudgetMetal_Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public HomeController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Login()
        {
            ViewData["Message"] = "Login page.";
            if (HttpContext.Request.Query["e"].ToString() != "")
            {
                ViewData["Error"] = "Username or password is invalid!";
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Username,Password")] bm_user bm_user)
        {
            string username = "";
            if(bm_user.Username != null)
            {
                username = bm_user.Username.Trim();
            }
            string password = "";
            if (bm_user.Password != null)
            {
                password = bm_user.Password.Trim();
            }
            var Identity = _configuration.GetConnectionString("Identity");
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(password);
            string passwordBase64 = Identity.ToString() + Convert.ToBase64String(plainTextBytes);
            bool authenticate = bm_userExists(username, passwordBase64);
            if (authenticate)
            {
                HttpContext.Session.SetString("LogOnUser", username);
                bool SiteAdmin =  _context.bm_user.FirstOrDefault(e => e.Username == username && e.Password == passwordBase64 && e.IsActive == true).SiteAdmin;
                HttpContext.Session.SetString("SiteAdmin", SiteAdmin.ToString());
                return RedirectToAction("Index", "Gallery");
            }
            else
            {
                
                return RedirectToAction("Login","Home",new { e= "invalid" });
            }
               
           
        }

        public IActionResult Logout()
        {
            //HttpContext.Session.SetString("LogOnUser", null);
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private bool bm_userExists(string username, string password)
        {
            
            return _context.bm_user.Any(e => e.Username == username && e.Password == password && e.IsActive == true);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
