using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Services.Industries;
using Com.BudgetMetal.Services.ServiceTags;
using Com.BudgetMetal.ViewModels.EzyTender;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Com.GenericPlatform.WebApp.Controllers
{
    public class RfqController : Controller
    {
        private readonly IIndustryService industryService;
        private readonly IServiceTagsService serviceTagsService;

        public RfqController(IIndustryService industryService, IServiceTagsService serviceTagsService)
        {
            this.industryService = industryService;
            this.serviceTagsService = serviceTagsService;
        }

        // GET: Rfq
        public ActionResult Index()
        {
            return View();
        }

        // GET: Rfq/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Rfq/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rfq/Create
        [HttpPost]
        public ActionResult Create(VmRfq Rfq)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetActiveIndustries()
        {
            var result =  industryService.GetActiveIndustries();

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetServiceTagByIndustry(int Id)
        {
            var result = serviceTagsService.GetVmServiceTagsByIndustry(Id);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

    }
}