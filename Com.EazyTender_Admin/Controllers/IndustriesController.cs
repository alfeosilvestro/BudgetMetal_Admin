using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Services.Industries;
using Com.BudgetMetal.ViewModels.Industries;
using Com.EazyTender_Admin.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Com.EazyTender_Admin.Controllers
{
    public class IndustriesController : Controller
    {
        private readonly IIndustryService svs;
        private readonly AppSettings _appSettings;

        public IndustriesController(IIndustryService svs, IOptions<AppSettings> appSettings)
        {
            this.svs = svs;
            this._appSettings = appSettings.Value;
        }

        // GET: Industries
        public ActionResult Index(string keyword, int page, int totalRecords)
        {
            var result = svs.GetIndustriesByPage(keyword, page, _appSettings.TotalRecordPerPage);

            return View(result);
        }

        // GET: Industries/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Industries/Create
        public ActionResult Create()
        {
            VmIndustryItem roleObj = new VmIndustryItem();
            return View(roleObj);
        }

        // POST: Industries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VmIndustryItem industry)
        {
            var result = svs.Insert(industry);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(industry);
            }
        }

        // GET: Industries/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int _id = (int)id;

            VmIndustryItem rItem = svs.GetIndustryById(_id);

            if (rItem == null)
            {
                return NotFound();
            }
            return View(rItem);
        }

        // POST: Industries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEdit(int id, VmIndustryItem insustry)
        {
            if (id != insustry.Id)
            {
                return NotFound();
            }

            var result = svs.Update(insustry);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(insustry);
            }
        }

        // GET: Industries/Delete/5
        public ActionResult Delete(int id)
        {
            svs.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Industries/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                svs.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}