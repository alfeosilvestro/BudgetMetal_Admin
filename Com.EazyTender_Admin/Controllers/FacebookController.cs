using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Services.Facebook;
using Microsoft.AspNetCore.Mvc;

namespace Com.EazyTender_Admin.Controllers
{
    public class FacebookController : Controller
    {
        private readonly IFacebookService svs;
        public FacebookController(IFacebookService svs)
        {
            this.svs = svs;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PostonWall()
        {
            var post = Request.Form["postMessage"];
            svs.PostMessage(post);
            return RedirectToAction("Index");
        }
    }
}