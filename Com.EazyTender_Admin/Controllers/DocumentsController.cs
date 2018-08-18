using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Services.Document;
using Com.BudgetMetal.ViewModels.Document;
using Com.EazyTender_Admin.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Com.EazyTender_Admin.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly IDocumentService svs;
        private readonly AppSettings _appSettings;

        public DocumentsController(IDocumentService svs, IOptions<AppSettings> appSettings)
        {
            this.svs = svs;
            this._appSettings = appSettings.Value;
        }

        // GET: Document
        public async Task<ActionResult> Index(string keyword, int page, int totalRecords)
        {
            var result = await svs.GetDocumentByPage(keyword, page, _appSettings.TotalRecordPerPage);

            return View(result);
        }

        // GET: Document/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Document/Create
        public async Task<ActionResult> Create()
        {
            var obj = await svs.GetFormObject();
            return View(obj);
        }

        // POST: Document/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VmDocumentItem vmItem)
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

        // GET: Document/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var obj = await svs.GetDocumentById(id);
            return View(obj);
        }

        // POST: Document/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, VmDocumentItem vmItem)
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

        // GET: Document/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            await svs.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Document/Delete/5
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