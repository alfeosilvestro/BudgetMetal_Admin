using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.DB;
using Com.BudgetMetal.DB.Entities;
using Com.BudgetMetal.Services.Role;
using Com.BudgetMetal.ViewModels.Role;
using Com.EazyTender_Admin.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Com.EazyTender_Admin.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRoleService svs;
        private readonly AppSettings _appSettings;
        private readonly DataContext dataContext;
        public RolesController(IRoleService svs, IOptions<AppSettings> appSettings, DataContext dataContext)
        {
            this.svs = svs;
            this._appSettings = appSettings.Value;
            this.dataContext = dataContext;
        }
        // GET: Roles
        public ActionResult Index(string keyword, int page, int totalRecords)
        {
            var result = svs.GetRolesByPage(keyword, page, _appSettings.TotalRecordPerPage);

            return View(result);
        }

        // GET: Roles/Details/5
        public IActionResult Details(int id)
        {
            VmRoleItem item = svs.GetRoleById(id);

            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            VmRoleItem roleObj = new VmRoleItem();
            return View(roleObj);
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id, Role, RoleCode")] VmRoleItem roles)
        public async Task<IActionResult> Create(VmRoleItem roles)
        {
            var result = svs.Insert(roles);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(roles);
            }
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int _id = (int)id;

            VmRoleItem rItem = svs.GetRoleById(_id);
            
            if (rItem == null)
            {
                return NotFound();
            }
            return View(rItem);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id, Role, RoleCode, UpdatedBy, CreatedDate, CreatedBy, Version, IsActive")] roles roles)
        //public async Task<IActionResult> Edit(int id, [Bind("Id, Role, RoleCode, UpdatedBy, Version, IsActive")] VmRoleItem roles)
        public async Task<IActionResult> Edit(int id, VmRoleItem roles)
        {
            if (id != roles.Id)
            {
                return NotFound();
            }

            var result = svs.Update(roles);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(roles);
            }
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int id)
        {
            svs.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Roles/Delete/5
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