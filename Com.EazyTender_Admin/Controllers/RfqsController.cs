using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Services.Company;
using Com.BudgetMetal.Services.Industries;
using Com.BudgetMetal.Services.RFQ;
using Com.BudgetMetal.Services.Roles;
using Com.BudgetMetal.Services.ServiceTags;
using Com.BudgetMetal.Services.Users;
using Com.BudgetMetal.ViewModels.Attachment;
using Com.BudgetMetal.ViewModels.DocumentUser;
using Com.BudgetMetal.ViewModels.InvitedSupplier;
using Com.BudgetMetal.ViewModels.Rfq;
using Com.EazyTender_Admin.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Com.EazyTender_Admin.Controllers
{
    public class RfqsController : Controller
    {
        private readonly IRFQService svs;
        private readonly AppSettings _appSettings;
        private readonly IIndustryService industryService;
        private readonly IServiceTagsService serviceTagsService;
        private readonly ICompanyService companyService;
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        
        public RfqsController(IIndustryService industryService, IServiceTagsService serviceTagsService, ICompanyService companyService, IUserService userService, IRoleService roleService, IRFQService svs, IOptions<AppSettings> appSettings)
        {
            this.svs = svs;
            this._appSettings = appSettings.Value;
            this.industryService = industryService;
            this.serviceTagsService = serviceTagsService;
            this.companyService = companyService;
            this.userService = userService;
            this.roleService = roleService;
        }

        // GET: Rfqs
        //public async Task<ActionResult> Index(int documentOwner, int page, int totalRecords)
        //{
        //    var result = await svs.GetRfqByPage(documentOwner, page, _appSettings.TotalRecordPerPage);

        //    return View(result);
        //}

        public async Task<ActionResult> Index()
        {
            string queryPage = HttpContext.Request.Query["page"];
            int page = 1;
            if (queryPage != null)
            {
                page = Convert.ToInt32(queryPage);
            }
            var result = await svs.GetRfqByPage(0, page, 10);
            return View(result);
        }

        // GET: Rfqs/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Rfq/Create
        public ActionResult Create()
        {
            HttpContext.Session.SetString("User_Id", "1");
            HttpContext.Session.SetString("Company_Id", "1");
            HttpContext.Session.SetString("UserName", "Peter");
            HttpContext.Session.SetString("FullName", "Peter");

            ViewBag.User_Id = HttpContext.Session.GetString("User_Id");
            ViewBag.Company_Id = HttpContext.Session.GetString("Company_Id");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.FullName = HttpContext.Session.GetString("FullName");

            return View();
        }

        // POST: Rfq/Create
        [HttpPost]
        public ActionResult Create(VmRfqItem Rfq)
        {
            try
            {
                Rfq.SelectedTags = Request.Form["SelectedTags"].ToString();
                var listAttachment = new List<VmAttachmentItem>();
                int i = 0;
                foreach (var itemFile in Request.Form.Files)
                {
                    if (itemFile.Length > 0)
                    {
                        var att = new VmAttachmentItem
                        {
                            FileName = itemFile.FileName,
                            FileSize = itemFile.Length,
                            FileBinary = Convert.ToBase64String(ConvertFiletoBytes(itemFile)),
                            Description = Request.Form["fileDescriptionRFQ[]"].ToArray()[i].ToString(),
                            CreatedBy = Rfq.CreatedBy,
                            UpdatedBy = Rfq.UpdatedBy
                        };

                        listAttachment.Add(att);

                    }
                    i++;
                }

                Rfq.Document.Attachment = listAttachment;

                var listInvitedSupplier = new List<VmInvitedSupplierItem>();
                var arrInvitedSupplier = Request.Form["supplier_list[]"].ToArray();
                foreach (var itemSupplier in arrInvitedSupplier)
                {
                    var supplier = new VmInvitedSupplierItem();
                    supplier.Company_Id = Convert.ToInt32(itemSupplier);

                    listInvitedSupplier.Add(supplier);
                }
                Rfq.InvitedSupplier = listInvitedSupplier;

                var listDocumentUser = new List<VmDocumentUserItem>();
                var arrUser = Request.Form["documentUserId[]"].ToArray();
                var arrRole = Request.Form["documentUserRole[]"].ToArray();
                for (int j = 0; j < arrUser.Length; j++)
                {
                    var userId = arrUser[j];
                    var rolesId = arrRole[j];
                    var roles = rolesId.Split(',');
                    foreach (var item in roles)
                    {
                        var documentUser = new VmDocumentUserItem
                        {
                            User_Id = Convert.ToInt32(userId),
                            Role_Id = Convert.ToInt32(item)
                        };
                        listDocumentUser.Add(documentUser);
                    }
                }
                Rfq.Document.DocumentUser = listDocumentUser;

                string documentNo = svs.SaveRFQ(Rfq);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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

        private byte[] ConvertFiletoBytes(IFormFile file)
        {
            byte[] p1 = null;
            if (file != null)
            {
                if (file.Length > 0)
                //Convert Image to byte and save to database
                {
                    using (var fs1 = file.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                }
            }
            return p1;
        }

        [HttpGet]
        public async Task<JsonResult> GetActiveIndustries()
        {
            var result = await industryService.GetActiveIndustries();

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetServiceTagByIndustry(int Id)
        {
            var result = await serviceTagsService.GetVmServiceTagsByIndustry(Id);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetActiveUserByCompany(int CompanyId)
        {
            var result = await userService.GetUserByCompany(CompanyId);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetActiveRoles()
        {
            var result = await roleService.GetActiveRoles();

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetSupplierByServiceTagsId(string serviceTagsId, int page)
        {
            var result = await companyService.GetSupplierByServiceTagsId(serviceTagsId, page);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetActiveCompanies()
        {
            var result = await companyService.GetActiveCompanies();

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
    }
}