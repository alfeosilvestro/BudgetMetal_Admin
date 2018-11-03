using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Services.TimeLine;
using Com.EzTender.WebApp.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Com.EzTender.WebApp.Controllers
{
    [EzyTenderActionFilter]
    public class TimeLinesController : Controller
    {
        private readonly ITimeLineService timelineService;

        public TimeLinesController(ITimeLineService timelineService)
        {
            this.timelineService = timelineService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetTimeLineData(int page)
        {
            var Company_Id = HttpContext.Session.GetString("Company_Id");
            int companyId = (Company_Id != null) ? int.Parse(Company_Id) : 0;
            var result = await timelineService.GetTimeLineData(page, companyId, 1, "");

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
    }
}