using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Com.EzTender.WebApp.Controllers
{
    public class QuotationController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}