using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Services.RFQ;
using Com.BudgetMetal.ViewModels.Rfq;
using Com.EazyTender_Admin.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Com.EazyTender_Admin.Controllers
{
    public class RfqsController : Controller
    {
        private readonly IRFQService svs;
        private readonly AppSettings _appSettings;

        public RfqsController(IRFQService svs, IOptions<AppSettings> appSettings)
        {
            this.svs = svs;
            this._appSettings = appSettings.Value;
        }

        // GET: Rfqs
        public async Task<ActionResult> Index(string keyword, int page, int totalRecords)
        {
            var result = await svs.GetRfqByPage(keyword, page, _appSettings.TotalRecordPerPage);

            return View(result);
        }

        // GET: Rfqs/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Rfqs/Create
        public async Task<ActionResult> Create()
        {
            var obj = await svs.GetFormObject();
            return View(obj);
        }

        // POST: Rfqs/Create 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VmRfqItem vmItem)
        {
            var result = svs.Insert(vmItem);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(vmItem);
            }
        }

        // GET: Rfqs/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var obj = await svs.GetRfqtById(id);
            return View(obj);
        }

        // POST: Rfqs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, VmRfqItem vmItem)
        {
            if (id != vmItem.Id)
            {
                return NotFound();
            }

            var result = await svs.Update(vmItem);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(vmItem);
            }
        }

        // GET: Rfqs/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            await svs.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Rfqs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                await svs.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}