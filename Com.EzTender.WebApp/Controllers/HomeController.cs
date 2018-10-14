using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Com.EzTender.WebApp.Models;
using Com.BudgetMetal.Services.RFQ;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Com.EzTender.WebApp.Filters;

namespace Com.GenericPlatform.WebApp.Controllers
{
    [EzyTenderActionFilter]
    public class HomeController : Controller
    {
        private readonly IRFQService rfqService;

        public HomeController(IRFQService rfqService)
        {
            this.rfqService = rfqService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Company_Id = HttpContext.Session.GetString("Company_Id");

            return View();
        }

        public async Task<IActionResult> PublicRFQ()
        {
            ViewBag.Company_Id = HttpContext.Session.GetString("Company_Id");

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

        [HttpGet]
        public async Task<JsonResult> GetRFQForDashboard(int page, string status, string Company_Id, string skeyword)
        {
           
            var result = await rfqService.GetRfqByPage(Convert.ToInt32(Company_Id), page, 2,Convert.ToInt32(status), 
                skeyword == null ? "": skeyword);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetPublicRFQ(int page, string status, string skeyword)
        {
            var result = await rfqService.GetPublicRfqByPage(page, 2, Convert.ToInt32(status),
                skeyword == null ? "" : skeyword);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
    }
}
