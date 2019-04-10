using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Common;
using Com.BudgetMetal.Services.Company;
using Com.BudgetMetal.Services.Roles;
using Com.BudgetMetal.ViewModels.Company;
using Com.EazyTender.WebApp.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Com.BudgetMetal.Services.Rating;

namespace Com.EzTender.WebApp.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ICompanyService svs;
        private readonly AppSettings _appSettings;
        private readonly IRoleService roleService;
        private readonly IRatingService ratingService;

        public CompaniesController(ICompanyService svs, IOptions<AppSettings> appSettings, IRoleService roleService, IRatingService ratingService)
        {
            this.svs = svs;
            this._appSettings = appSettings.Value;
            this.roleService = roleService;
            this.ratingService = ratingService;
        }
        
        // GET: Companies
        public async Task<ActionResult> Index(string keyword, int page, int totalRecords)
        {
            var result = await svs.GetCompanySupplierList(keyword, page, _appSettings.TotalRecordPerPage);
            ViewData["keyword"] = keyword;

            return View(result);
        }

        // GET: PreferSuppliers
        public async Task<ActionResult> PreferSuppliers(int page, string keyword)
        {
            var company_Id = HttpContext.Session.GetString("Company_Id");
            int id = Convert.ToInt32(company_Id);
            var result = await svs.GetSupplierByCompany(id, page, keyword);
            ViewData["keyword"] = keyword;
            return View(result);
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Profile()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetString("Company_Id"));
            var User_Id = HttpContext.Session.GetString("User_Id");
            var userRoles = JsonConvert.DeserializeObject<List<BudgetMetal.ViewModels.Role.VmRoleItem>>(HttpContext.Session.GetString("SelectedRoles"));
            bool isCompanyAdmin = false;
            if (userRoles.Where(e => e.Id == Constants.C_Admin_Role).ToList().Count > 0)
            {
                isCompanyAdmin = true;
            }
            ViewData["isAdmin"] = isCompanyAdmin;
            VmCompanyItem item = await svs.GetCompanyProfileById(id);

            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        public async Task<IActionResult> Details(int id)
        {
            VmCompanyItem item = await svs.GetCompanyProfileById(id);

            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
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

        // GET: Companies/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Companies/Edit/5
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

        // GET: Companies/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Companies/Delete/5
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

        [HttpPost]
        public async Task<JsonResult> EditCompanyAbout(int CompanyId, string About)
        {
            string updatedBy = HttpContext.Session.GetString("EmailAddress");
            var result = await svs.EditCompanyAbout(CompanyId, About, updatedBy);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpPost]
        public async Task<JsonResult> EditCompanyAddress(int CompanyId, string Address)
        {
            string updatedBy = HttpContext.Session.GetString("EmailAddress");
            var result = await svs.EditCompanyAddress(CompanyId, Address, updatedBy);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpPost]
        public async Task<JsonResult> EditCompanyUser(int CompanyId, int UserId, string IsConfirmStatus)
        {
            string updatedBy = HttpContext.Session.GetString("EmailAddress");
            bool isActiveStatus = (IsConfirmStatus == "Active") ? false : true;
            var result = await svs.EditCompanyUser(CompanyId, UserId, isActiveStatus, updatedBy);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetuserRole(string RoleType)
        {
            var result = await roleService.GetActiveRoles(RoleType);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpPost]
        public async Task<JsonResult> EditCompanyUserRole(int CompanyId, int UserId, string[] userRole)
        {
            string updatedBy = HttpContext.Session.GetString("EmailAddress");
            
            var result = await svs.EditCompanyUserRole(CompanyId, UserId, userRole, updatedBy);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetRatingData(int page,int companyId)
        {
            //var Company_Id = HttpContext.Session.GetString("Company_Id");
            //int companyId = (Company_Id != null) ? int.Parse(Company_Id) : 0;
            var result = await ratingService.GetRatingData(page, 10, companyId, 0, "");

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
    }
}