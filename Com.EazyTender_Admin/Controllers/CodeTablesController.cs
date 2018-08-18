using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.Services.Code_Table;
using Com.BudgetMetal.ViewModels.CodeTable;
using Com.EazyTender_Admin.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Com.EazyTender_Admin.Controllers
{
    public class CodeTablesController : Controller
    {
        private readonly ICodeTableService svs;
        private readonly AppSettings _appSettings;
        private readonly DataContext dataContext;
        public CodeTablesController(ICodeTableService svs, IOptions<AppSettings> appSettings, DataContext dataContext)
        {
            this.svs = svs;
            this._appSettings = appSettings.Value;
            this.dataContext = dataContext;
        }

        // GET: CodeTables
        public async Task<ActionResult> Index(string keyword, int page, int totalRecords)
        {
            var result = await svs.GetCodeTableByPage(keyword, page, _appSettings.TotalRecordPerPage);

            return View(result);
        }

        // GET: CodeTables/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

        // GET: CodeTables/Create
        public async Task<ActionResult> Create()
        {            
            var obj = await svs.GetFormObject();
            return View(obj);
        }

        // POST: CodeTables/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VmCodeTableItem codeTableItem)
        {
            var result = svs.Insert(codeTableItem);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(codeTableItem);
            }
        }

        // GET: CodeTables/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var obj = await svs.GetCodeTableById(id);
            return View(obj);
        }

        // POST: CodeTables/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, VmCodeTableItem codeTableItem)
        {
            if (id != codeTableItem.Id)
            {
                return NotFound();
            }

            var result = await svs.Update(codeTableItem);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(codeTableItem);
            }
        }

        // GET: CodeTables/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            await svs.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: CodeTables/Delete/5
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