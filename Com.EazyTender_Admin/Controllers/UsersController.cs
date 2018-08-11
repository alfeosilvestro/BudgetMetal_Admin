using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Services.Users;
using Com.BudgetMetal.ViewModels.Role;
using Com.BudgetMetal.ViewModels.User;
using Com.EazyTender_Admin.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Com.EazyTender_Admin.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService svs;
        private readonly AppSettings _appSettings;

        public UsersController(IUserService svs, IOptions<AppSettings> appSettings)
        {
            this.svs = svs;
            this._appSettings = appSettings.Value;
        }

        // GET: Users
        public async Task<ActionResult> Index(string keyword, int page, int totalRecords)
        {
            var result = await svs.GetUserByPage(keyword, page, _appSettings.TotalRecordPerPage);

            return View(result);
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Users/Create
        public async Task<ActionResult> Create()
        {
            var obj = await svs.GetFormObject();
            return View(obj);
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VmUserItem _vmObject)
        {
            var roleListArr = Request.Form["RoleList"].ToArray();

            for (int i = 0; i < roleListArr.Length; i++)
            {
                VmRoleItem _lst = new VmRoleItem();

                if (roleListArr[i].ToString().All(char.IsDigit))
                {
                    _lst.Id = int.Parse(roleListArr[i]);
                    _vmObject.RoleList.Add(_lst);
                }
            }

            var result = await svs.Insert(_vmObject);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(_vmObject);
            }
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int _id = (int)id;

            var rItem = await svs.GetUserById(_id);

            if (rItem == null)
            {
                return NotFound();
            }
            return View(rItem);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, VmUserItem _vmObject)
        {
            var roleListArr = Request.Form["RoleList"].ToArray();

            for (int i = 0; i < roleListArr.Length; i++)
            {
                VmRoleItem _lst = new VmRoleItem();

                if (roleListArr[i].ToString().All(char.IsDigit))
                {
                    _lst.Id = int.Parse(roleListArr[i]);
                    _vmObject.RoleList.Add(_lst);
                }
            }

            if (id != _vmObject.Id)
            {
                return NotFound();
            }

            var result = await svs.Update(_vmObject);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(_vmObject);
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Users/Delete/5
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