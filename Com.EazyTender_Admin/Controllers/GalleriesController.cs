using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Services.Gallery;
using Com.BudgetMetal.Services.GalleryAPI.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Com.EazyTender_Admin.Controllers
{
    public class GalleriesController : Controller
    {
        private readonly IGalleryService svs;
        private readonly AppSettings _appSettings;

        public GalleriesController(IGalleryService svs, IOptions<AppSettings> appSettings)
        {
            this.svs = svs;
            this._appSettings = appSettings.Value;
        }
        // GET: Galleries
        public ActionResult Index(string keyword, int page, int totalRecords)
        {
            var result = svs.GetGalleriesByPage(keyword, page, _appSettings.TotalRecordPerPage, _appSettings.App_Identity.Identity);

            return View(result);
        }

        // GET: Galleries/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Galleries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Galleries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: Galleries/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Galleries/Edit/5
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

        // GET: Galleries/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Galleries/Delete/5
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