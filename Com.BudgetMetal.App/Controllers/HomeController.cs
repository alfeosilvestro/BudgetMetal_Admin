using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Com.BudgetMetal.App.Models;
using Com.BudgetMetal.App.Configurations;
using Microsoft.Extensions.Options;

namespace Com.BudgetMetal.App.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppSettings _appSettings;
        public HomeController(IOptions<AppSettings> appSettings)
        {
            this._appSettings = appSettings.Value;
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Gallery()
        {
            ViewData["MainSiteURL"] = _appSettings.MainSiteURL;
            ViewData["APIURL"] = _appSettings.APIURL;
            ViewData["DefaultUEN"] = _appSettings.DefaultUEN;

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
