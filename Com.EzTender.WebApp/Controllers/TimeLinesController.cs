using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Com.EzTender.WebApp.Controllers
{
    public class TimeLinesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public async Task<JsonResult> GetQuotationByRfqId(int page, string keyword)
        //{
        //    var result = await quotationService.GetQuotationByRfqId(RfqId, page, 10, 0, (keyword == null ? "" : keyword));

        //    return new JsonResult(result, new JsonSerializerSettings()
        //    {
        //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //    });
        //}

    }
}