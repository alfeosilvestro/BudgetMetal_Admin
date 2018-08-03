using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Services.EmailLog;
using Com.EazyTender_Admin.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Com.EazyTender_Admin.Controllers
{
    public class EmailLogsController : Controller
    {
        private readonly IEmailLogService svs;
        private readonly AppSettings _appSettings;
        public EmailLogsController(IEmailLogService svs, IOptions<AppSettings> appSettings)
        {
            this.svs = svs;
            this._appSettings = appSettings.Value;
        }

        // GET: EmailLogs
        public async Task<ActionResult> Index(string keyword, int page, int totalRecords)
        {
            var result = await svs.GetEmailLogByPage(keyword, page, _appSettings.TotalRecordPerPage);

            return View(result);
        }

        // GET: EmailLogs/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmailLogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmailLogs/Create
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

        // GET: EmailLogs/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EmailLogs/Edit/5
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

        // GET: EmailLogs/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmailLogs/Delete/5
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