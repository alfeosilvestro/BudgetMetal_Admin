using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Services.Code_Category;
using Com.BudgetMetal.ViewModels.CodeCategory;
using Com.EazyTender_Admin.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Com.EazyTender_Admin.Controllers
{
    public class CodeCategoriesController : Controller
    {
        private readonly ICodeCategoryService svs;
        private readonly AppSettings _appSettings;

        public CodeCategoriesController(ICodeCategoryService svs, IOptions<AppSettings> appSettings)
        {
            this.svs = svs;
            this._appSettings = appSettings.Value;
        }

        // GET: CodeCategory
        public ActionResult Index(string keyword, int page, int totalRecords)
        {
            var result = svs.GetCodeCategoryByPage(keyword, page, _appSettings.TotalRecordPerPage);

            return View(result);
        }

        // GET: CodeCategory/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CodeCategory/Create
        public ActionResult Create()
        {
            VmCodeCategoryItem roleObj = new VmCodeCategoryItem();
            return View(roleObj);
        }

        // POST: CodeCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VmCodeCategoryItem codeCategoryItem)
        {
            var result = svs.Insert(codeCategoryItem);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(codeCategoryItem);
            }
        }

        // GET: CodeCategory/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int _id = (int)id;

            VmCodeCategoryItem rItem = svs.GetCodeCategoryById(_id);

            if (rItem == null)
            {
                return NotFound();
            }
            return View(rItem);
        }


        // POST: CodeCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VmCodeCategoryItem codeCategoriesItem)
        {
            if (id != codeCategoriesItem.Id)
            {
                return NotFound();
            }

            var result = svs.Update(codeCategoriesItem);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(codeCategoriesItem);
            }
        }

        // GET: CodeCategory/Delete/5
        public ActionResult Delete(int id)
        {
            svs.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: CodeCategory/Delete/5
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