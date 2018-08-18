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
using Com.BudgetMetal.ViewModels.EzyTender;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Com.GenericPlatform.WebApp.Controllers
{
    public class RfqController : Controller
    {
        private readonly IIndustryService industryService;
        private readonly IServiceTagsService serviceTagsService;
        private readonly ICompanyService companyService;
        private readonly IRFQService rfqService;
        private readonly IUserService userService;
        private readonly IRoleService roleService;

        public RfqController(IIndustryService industryService, IServiceTagsService serviceTagsService, ICompanyService companyService, IRFQService rfqService, IUserService userService, IRoleService roleService)
        {
            this.industryService = industryService;
            this.serviceTagsService = serviceTagsService;
            this.companyService = companyService;
            this.rfqService = rfqService;
            this.userService = userService;
            this.roleService = roleService;
        }

        // GET: Rfq
        public async Task<ActionResult> Index()
        {
            string queryPage = HttpContext.Request.Query["page"];
            int page = 1;
            if (queryPage != null)
            {
                page = Convert.ToInt32(queryPage);
            }
            var result = await rfqService.GetRfqByPage(0, page, 10);
            return View(result);
        }

        // GET: Rfq/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Rfq/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rfq/Create
        [HttpPost]
        public ActionResult Create(VmRfq Rfq)
        {
            try
            {
                Rfq.SelectedTags = Request.Form["SelectedTags"].ToString();
                var listAttachment = new List<VmAttachment>();
                int i = 0;
                foreach (var itemFile in Request.Form.Files)
                {
                    if (itemFile.Length > 0)
                    {
                        var att = new VmAttachment();

                        att.FileName = itemFile.FileName;
                        att.FileSize = itemFile.Length;
                        att.FileBinary = Convert.ToBase64String(ConvertFiletoBytes(itemFile));
                        att.Description = Request.Form["fileDescriptionRFQ[]"].ToArray()[i].ToString();
                        att.CreatedBy = Rfq.CreatedBy;
                        att.UpdatedBy = Rfq.UpdatedBy;
                        listAttachment.Add(att);

                    }
                    i++;
                }

                Rfq.Document.Attachment = listAttachment;

                var listInvitedSupplier = new List<VmInvitedSupplier>();
                var arrInvitedSupplier = Request.Form["supplier_list[]"].ToArray();
                foreach (var itemSupplier in arrInvitedSupplier)
                {
                    var supplier = new VmInvitedSupplier();
                    supplier.Company_Id = Convert.ToInt32(itemSupplier);

                    listInvitedSupplier.Add(supplier);
                }
                Rfq.InvitedSupplier = listInvitedSupplier;

                string documentNo = rfqService.SaveRFQ(Rfq);

                return RedirectToAction("Index");
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
    }
}