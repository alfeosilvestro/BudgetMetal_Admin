using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Com.BudgetMetal.App.Models;

namespace Com.BudgetMetal.App.Controllers
{
    public class HomeController : Controller
    {
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Gallery()
        {
            string token = HttpContext.Request.Query["token"].ToString();
            if(token == null)
            {
                ViewData["token"] = "";
            }
            else
            {
                ViewData["token"] = token;
            }

            string fileid = HttpContext.Request.Query["fileid"].ToString();
            if (fileid == null)
            {
                ViewData["fileid"] = "";
            }
            else
            {
                ViewData["fileid"] = fileid;
            }
            return View();
        }
    }
}
