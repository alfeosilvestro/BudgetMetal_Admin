using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Common;
using Com.BudgetMetal.Services.Attachment;
using Com.BudgetMetal.Services.Company;
using Com.BudgetMetal.Services.Industries;
using Com.BudgetMetal.Services.Quotation;
using Com.BudgetMetal.Services.RFQ;
using Com.BudgetMetal.Services.Roles;
using Com.BudgetMetal.Services.ServiceTags;
using Com.BudgetMetal.Services.Users;
using Com.BudgetMetal.ViewModels.Attachment;
using Com.BudgetMetal.ViewModels.DocumentActivity;
using Com.BudgetMetal.ViewModels.DocumentUser;
using Com.BudgetMetal.ViewModels.InvitedSupplier;
using Com.BudgetMetal.ViewModels.Rfq;
using Com.BudgetMetal.ViewModels.Role;
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
        private readonly IAttachmentService attachmentService;
        private readonly IQuotationService quotationService;

        public RfqController(IIndustryService industryService, IServiceTagsService serviceTagsService, ICompanyService companyService, IRFQService rfqService, IUserService userService, IRoleService roleService, IAttachmentService attachmentService, IQuotationService quotationService)
        {
            this.industryService = industryService;
            this.serviceTagsService = serviceTagsService;
            this.companyService = companyService;
            this.rfqService = rfqService;
            this.userService = userService;
            this.roleService = roleService;
            this.attachmentService = attachmentService;
            this.quotationService = quotationService;
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

            var Company_Id = HttpContext.Session.GetString("Company_Id");
            var User_Id = HttpContext.Session.GetString("User_Id");
            var userRoles = JsonConvert.DeserializeObject<List<VmRoleItem>>(HttpContext.Session.GetString("SelectedRoles"));
            bool isCompanyAdmin = false;
            if (userRoles.Where(e => e.Id == Constants.C_Admin_Role).ToList().Count > 0)
            {
                isCompanyAdmin = true;
            }
            var result = await rfqService.GetRfqByPage(Convert.ToInt32(User_Id), Convert.ToInt32(Company_Id), page, 10, isCompanyAdmin);
            return View(result);
        }

        public async Task<ActionResult> Listing()
        {
            string queryPage = HttpContext.Request.Query["page"];
            int page = 1;
            if (queryPage != null)
            {
                page = Convert.ToInt32(queryPage);
            }

            var Company_Id = HttpContext.Session.GetString("Company_Id");

            var result = await rfqService.GetRfqForSupplierByPage(Convert.ToInt32(Company_Id), page, 10);
            return View(result);
        }

        // GET: Rfq/Edit/5
        [HttpGet]
        public async Task<ActionResult> View(int id)
        {
            try
            {
                var result = await rfqService.GetSingleRfqById(id);

                return View(result);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }

        }

        // GET: Rfq/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var result = await rfqService.GetSingleRfqById(id);

                return View(result);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public ActionResult Edit(VmRfqItem Rfq)
        {
            try
            {
                var submitType = Request.Form["btnType"];
                string documentAction = "";
                if (submitType.ToString().ToLower() == "draft")
                {
                    Rfq.Document.DocumentStatus_Id = Constants_CodeTable.Code_RFQ_Draft;
                    documentAction = "Save as Draft";
                }
                else
                {
                    Rfq.Document.DocumentStatus_Id = Constants_CodeTable.Code_RFQ_Submitted;
                    documentAction = "Submitted";
                }
                Rfq.SelectedTags = Request.Form["SelectedTags"].ToString();
                Rfq.Document.UpdatedBy = HttpContext.Session.GetString("EmailAddress");
                Rfq.UpdatedBy = HttpContext.Session.GetString("EmailAddress");
                //var listAttachment = new List<VmAttachmentItem>();
                int i = 0;
                foreach (var itemFile in Request.Form.Files)
                {
                    if (itemFile.Length > 0)
                    {
                        var tmpFileNameArr = itemFile.FileName.ToString().Split("\\");
                        string tmpFileName = tmpFileNameArr.Last();
                        var att = new VmAttachmentItem
                        {
                            FileName = tmpFileName,
                            FileSize = itemFile.Length,
                            FileBinary = Convert.ToBase64String(ConvertFiletoBytes(itemFile)),
                            Description = Request.Form["fileDescriptionRFQ[]"].ToArray()[i].ToString(),
                            CreatedBy = Rfq.CreatedBy,
                            UpdatedBy = Rfq.UpdatedBy
                        };

                        Rfq.Document.Attachment.Add(att);

                    }
                    i++;
                }

                //Rfq.Document.Attachment = listAttachment;

                var listInvitedSupplier = new List<VmInvitedSupplierItem>();
                var arrInvitedSupplier = Request.Form["supplier_list"].ToArray();
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

                var listDocumentActivity = new List<VmDocumentActivityItem>();
                var DocumentActivity = new VmDocumentActivityItem()
                {
                    User_Id = Convert.ToInt32(HttpContext.Session.GetString("User_Id")),
                    IsRfq = true,
                    Action = documentAction,

                };
                listDocumentActivity.Add(DocumentActivity);
                //Rfq.DocumentActivityList = listDocumentActivity;
                Rfq.Document.DocumentActivityList = listDocumentActivity;

                //string documentNo = rfqService.UpdateRFQ(Rfq);

                //return RedirectToAction("Index");

                var result = rfqService.UpdateRFQ(Rfq);
                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Rfq/Create
        public ActionResult Create()
        {
            ViewBag.User_Id = HttpContext.Session.GetString("User_Id");
            ViewBag.Company_Id = HttpContext.Session.GetString("Company_Id");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.FullName = HttpContext.Session.GetString("ContactName");
            ViewBag.EmailAddress = HttpContext.Session.GetString("EmailAddress");
            if (rfqService.CheckRFQLimit(Convert.ToInt32(HttpContext.Session.GetString("Company_Id"))) == false)
            {
                TempData["message"] = "RFQ Limitation per week is exceed. please contact admin to upgrade your account.";
                return RedirectToAction("Index");
            }

            return View();
        }

        // POST: Rfq/Create
        [HttpPost]
        public ActionResult Create(VmRfqItem Rfq)
        {
            try
            {
                var submitType = Request.Form["btnType"];
                string documentAction = "";
                if (submitType.ToString().ToLower() == "draft")
                {
                    Rfq.Document.DocumentStatus_Id = Constants_CodeTable.Code_RFQ_Draft;
                    documentAction = "Save as Draft";
                }
                else
                {
                    Rfq.Document.DocumentStatus_Id = Constants_CodeTable.Code_RFQ_Submitted;
                    documentAction = "Submitted";
                }
                Rfq.SelectedTags = Request.Form["SelectedTags"].ToString();
                var listAttachment = new List<VmAttachmentItem>();
                int i = 0;
                foreach (var itemFile in Request.Form.Files)
                {
                    if (itemFile.Length > 0)
                    {
                        var tmpFileNameArr = itemFile.FileName.ToString().Split("\\");
                        string tmpFileName = tmpFileNameArr.Last();
                        var att = new VmAttachmentItem
                        {
                            FileName = tmpFileName,
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
                var arrInvitedSupplier = Request.Form["supplier_list"].ToArray();
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

                var listDocumentActivity = new List<VmDocumentActivityItem>();
                var DocumentActivity = new VmDocumentActivityItem()
                {
                    User_Id = Convert.ToInt32(HttpContext.Session.GetString("User_Id")),
                    IsRfq = true,
                    Action = documentAction,

                };
                listDocumentActivity.Add(DocumentActivity);
                Rfq.Document.DocumentActivityList = listDocumentActivity;

                var result = rfqService.SaveRFQ(Rfq);
                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
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
        public async Task<JsonResult> GetQuotationByRfqId(int RfqId, int page, string keyword)
        {
            var result = await quotationService.GetQuotationByRfqId(RfqId, page, 10, 0, (keyword == null ? "" : keyword));

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
            var result = await roleService.GetActiveRoles("rfq");

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetSupplierByServiceTagsId(string serviceTagsId, int page, string searchKeyword = "")
        {
            int companyId = Convert.ToInt32(HttpContext.Session.GetString("Company_Id"));
            var result = await companyService.GetSupplierByServiceTagsId(companyId, serviceTagsId, page, searchKeyword);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> WithdrawnRfq(int documentId)
        {

            var result = await rfqService.WithdrawnRfq(documentId, Convert.ToInt32(HttpContext.Session.GetString("User_Id")), HttpContext.Session.GetString("UserName"));

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> DeleteRfq(int documentId)
        {

            var result = await rfqService.DeleteRfq(documentId, Convert.ToInt32(HttpContext.Session.GetString("User_Id")), HttpContext.Session.GetString("UserName"));

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> CheckQuotationByRfqId(int rfqId)
        {

            var result = await rfqService.CheckQuotationByRfqId(rfqId, Convert.ToInt32(HttpContext.Session.GetString("Company_Id")));

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> LoadSelectedSupplier(int RfqId)
        {
            var result = await rfqService.LoadSelectedSupplier(RfqId);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        public async Task<FileResult> AttachmentDownload(int fileid)
        {
            var result = await attachmentService.GetAttachmentById(fileid);

            var fileByeArray = result.FileBinary;
            string fileName = result.FileName;
            var readStream = new MemoryStream(Convert.FromBase64String(fileByeArray));
            var mimeType = "application/zip";
            return File(readStream, mimeType, fileName);
        }
    }
}