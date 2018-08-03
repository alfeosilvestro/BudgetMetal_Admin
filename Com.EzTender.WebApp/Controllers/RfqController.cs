using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.ViewModels.EzyTender;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Com.GenericPlatform.WebApp.Controllers
{
    public class RfqController : Controller
    {
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

        // GET: Rfq/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Rfq/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Rfq/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Rfq/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}