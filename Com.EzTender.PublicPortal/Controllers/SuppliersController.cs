using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Services.Company;
using Com.BudgetMetal.Services.RFQ;
using Com.BudgetMetal.ViewModels.Company;
using Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Com.EzTender.PublicPortal.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ICompanyService svs;
        private readonly AppSettings _appSettings;
        private readonly IRFQService rfqService;

        public SuppliersController(ICompanyService svs, IOptions<AppSettings> appSettings, IRFQService rfqService)
        {
            this.svs = svs;
            this._appSettings = appSettings.Value;
            this.rfqService = rfqService;
        }

        // GET: Suppliers
        public async Task<ActionResult> Index(string keyword, int page, int totalRecords)
        {
            var result = await svs.GetCompanySupplierList(keyword, page, _appSettings.TotalRecordPerPage);

            return View(result);
        }

        // GET: Suppliers/Details/5        
        public async Task<IActionResult> Details(int id)
        {
            VmCompanyItem item = await svs.GetCompanyById(id);

            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
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

        // GET: Suppliers/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Suppliers/Edit/5
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

        // GET: Suppliers/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Suppliers/Delete/5
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

        [HttpGet]
        public async Task<JsonResult> GetSupplier(int page, string status, string Company_Id, string skeyword)
        {
            var result = await svs.GetCompanySupplierList(skeyword, page, 3);
            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetPublicRFQ(int page, string status, string skeyword)
        {
            var result = await rfqService.GetPublicRfqByPage(page, 2, Convert.ToInt32(status),
                skeyword == null ? "" : skeyword);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
    }
}