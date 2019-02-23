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
using Com.BudgetMetal.ViewModels.Role;
using Com.BudgetMetal.Common;

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

        public IActionResult ErrorForUser()
        {
            string access = Request.Query["access"];
            if(access != null)
            {
                if(access.ToString().ToLower().Trim() == "denied")
                {
                    TempData["ErrorMessage"] = "You are not authorized to access this page.";
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetRFQForDashboard(int page, string status, string skeyword)
        {
            
            string currentCompanyType = HttpContext.Session.GetString("C_BusinessType");
            if (currentCompanyType == Constants_CodeTable.Code_C_Buyer.ToString())
            {
                var Company_Id = HttpContext.Session.GetString("Company_Id");
                var User_Id = HttpContext.Session.GetString("User_Id");
                var userRoles = JsonConvert.DeserializeObject<List<VmRoleItem>>(HttpContext.Session.GetString("SelectedRoles"));
                bool isCompanyAdmin = false;
                if (userRoles.Where(e => e.Id == Constants.C_Admin_Role).ToList().Count > 0)
                {
                    isCompanyAdmin = true;
                }
                var result = await rfqService.GetRfqByPageForDashboard(Convert.ToInt32(User_Id), Convert.ToInt32(Company_Id), page, 10, isCompanyAdmin, Convert.ToInt32(status), skeyword == null ? "" : skeyword);

                return new JsonResult(result, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            else
            {
               

                var Company_Id = HttpContext.Session.GetString("Company_Id");

                var result = await rfqService.GetRfqForSupplierByPage(Convert.ToInt32(Company_Id), page, 10);

                return new JsonResult(result, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }

            
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
