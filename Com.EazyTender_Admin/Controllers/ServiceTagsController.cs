using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Services.ServiceTags;
using Com.BudgetMetal.ViewModels.ServiceTags;
using Com.EazyTender_Admin.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Com.EazyTender_Admin.Controllers
{
    public class ServiceTagsController : Controller
    {
        private readonly IServiceTagsService svs;
        private readonly AppSettings _appSettings;

        public ServiceTagsController(IServiceTagsService svs, IOptions<AppSettings> appSettings)
        {
            this.svs = svs;
            this._appSettings = appSettings.Value;
        }

        // GET: ServiceTags
        public async Task<ActionResult> Index(string keyword, int page, int totalRecords)
        {
            var result = await svs.GetServiceTagsByPage(keyword, page, _appSettings.TotalRecordPerPage);

            return View(result);
        }

        // GET: ServiceTags/Details/5
        public ActionResult Details(int id)
        {   
            return View();
        }

        // GET: ServiceTags/Create
        public ActionResult Create()
        {
            VmServiceTagsItem roleObj = new VmServiceTagsItem();
            return View(roleObj);
        }

        // POST: ServiceTags/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VmServiceTagsItem serviceTagsItem)
        {
            var result = svs.Insert(serviceTagsItem);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(serviceTagsItem);
            }
        }

        // GET: ServiceTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int _id = (int)id;

            VmServiceTagsItem rItem = await svs.GetServiceTagsById(_id);

            if (rItem == null)
            {
                return NotFound();
            }
            return View(rItem);

        }

        // POST: ServiceTags/Edit/5
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

        // GET: ServiceTags/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ServiceTags/Delete/5
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