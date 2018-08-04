using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Services.Company;
using Com.BudgetMetal.ViewModels.Company;
using Com.EazyTender_Admin.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Com.EazyTender_Admin.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ICompanyService svs;
        private readonly AppSettings _appSettings;

        public CompaniesController(ICompanyService svs, IOptions<AppSettings> appSettings)
        {
            this.svs = svs;
            this._appSettings = appSettings.Value;
        }

        // GET: CodeCategory
        public async Task<ActionResult> Index(string keyword, int page, int totalRecords)
        {
            var result = await svs.GetCompanyByPage(keyword, page, _appSettings.TotalRecordPerPage);

            return View(result);
        }

        // GET: Companies/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VmCompanyItem codeCompanyItem)
        {
            var result = svs.Insert(codeCompanyItem);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(codeCompanyItem);
            }
        }

        // GET: Companies/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int _id = (int)id;

            VmCompanyItem rItem = await svs.GetCompanyById(_id);

            if (rItem == null)
            {
                return NotFound();
            }
            return View(rItem);
        }

        // POST: Companies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VmCompanyItem companyItem)
        {
            if (id != companyItem.Id)
            {
                return NotFound();
            }

            var result = await svs.Update(companyItem);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(companyItem);
            }
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Companies/Delete/5
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