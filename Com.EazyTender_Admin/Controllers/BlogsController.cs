using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.Services.Blogs;
using Com.BudgetMetal.ViewModels.Blogs;
using Com.EazyTender_Admin.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Com.EazyTender_Admin.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogService svs;
        private readonly AppSettings _appSettings;
        private readonly DataContext dataContext;
        public BlogsController(IBlogService svs, IOptions<AppSettings> appSettings, DataContext dataContext)
        {
            this.svs = svs;
            this._appSettings = appSettings.Value;
            this.dataContext = dataContext;
        }
        // GET: Blogs
        public async Task<ActionResult> Index(string keyword, int page, int totalRecords)
        {
            var result = await svs.GetBlogsByPage(keyword, page, _appSettings.TotalRecordPerPage);

            return View(result);
        }

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int id)
        {
            VmBlogItem item = await svs.GetBlogById(id);

            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // GET: Blogs/Create
        public ActionResult Create()
        {
            VmBlogItem BlogObj = new VmBlogItem();
            return View(BlogObj);
        }

        // POST: Blogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id, Blog, BlogCode")] VmBlogItem Blogs)
        public async Task<IActionResult> Create(VmBlogItem Blogs)
        {
            var result = svs.Insert(Blogs);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(Blogs);
            }
        }

        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int _id = (int)id;

            VmBlogItem rItem = await svs.GetBlogById(_id);
            
            if (rItem == null)
            {
                return NotFound();
            }
            return View(rItem);
        }

        // POST: Blogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id, Blog, BlogCode, UpdatedBy, CreatedDate, CreatedBy, Version, IsActive")] Blogs Blogs)
        //public async Task<IActionResult> Edit(int id, [Bind("Id, Blog, BlogCode, UpdatedBy, Version, IsActive")] VmBlogItem Blogs)
        public async Task<IActionResult> Edit(int id, VmBlogItem Blogs)
        {
            if (id != Blogs.Id)
            {
                return NotFound();
            }

            var result = await svs.Update(Blogs);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(Blogs);
            }
        }

        // GET: Blogs/Delete/5
        public ActionResult Delete(int id)
        {
            svs.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Blogs/Delete/5
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