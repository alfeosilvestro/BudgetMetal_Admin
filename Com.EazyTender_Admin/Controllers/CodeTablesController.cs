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
        public ActionResult Index(string keyword, int page, int totalRecords)
        {
            var result = svs.GetCodeTableByPage(keyword, page, _appSettings.TotalRecordPerPage);

            return View(result);
        }

        // GET: CodeTables/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CodeTables/Create
        public ActionResult Create()
        {
            VmCodeTableItem roleObj = new VmCodeTableItem();
            return View(roleObj);
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CodeTables/Edit/5
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

        // GET: CodeTables/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CodeTables/Delete/5
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