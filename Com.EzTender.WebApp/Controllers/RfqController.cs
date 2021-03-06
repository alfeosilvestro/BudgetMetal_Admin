﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.BudgetMetal.Common;
using Com.BudgetMetal.Services.Attachment;
using Com.BudgetMetal.Services.Company;
using Com.BudgetMetal.Services.Facebook;
using Com.BudgetMetal.Services.Industries;
using Com.BudgetMetal.Services.Quotation;
using Com.BudgetMetal.Services.RFQ;
using Com.BudgetMetal.Services.Roles;
using Com.BudgetMetal.Services.ServiceTags;
using Com.BudgetMetal.Services.Users;
using Com.BudgetMetal.Services.Rating;
using Com.BudgetMetal.ViewModels;
using Com.BudgetMetal.ViewModels.Attachment;
using Com.BudgetMetal.ViewModels.DocumentActivity;
using Com.BudgetMetal.ViewModels.DocumentUser;
using Com.BudgetMetal.ViewModels.InvitedSupplier;
using Com.BudgetMetal.ViewModels.Requirement;
using Com.BudgetMetal.ViewModels.Rfq;
using Com.BudgetMetal.ViewModels.RfqPenalty;
using Com.BudgetMetal.ViewModels.RfqPriceSchedule;
using Com.BudgetMetal.ViewModels.Role;
using Com.BudgetMetal.ViewModels.Sla;
using Com.BudgetMetal.ViewModels.TemplateModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Com.BudgetMetal.ViewModels.Rating;

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
        private readonly IRatingService ratingService;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IFacebookService fbService;

        public RfqController(IIndustryService industryService, IServiceTagsService serviceTagsService, ICompanyService companyService, IRFQService rfqService, IUserService userService, IRoleService roleService, IAttachmentService attachmentService, IQuotationService quotationService, IHostingEnvironment hostingEnvironment, IFacebookService fbService, IRatingService ratingService)
        {
            this.industryService = industryService;
            this.serviceTagsService = serviceTagsService;
            this.companyService = companyService;
            this.rfqService = rfqService;
            this.userService = userService;
            this.roleService = roleService;
            this.attachmentService = attachmentService;
            this.quotationService = quotationService;
            this._hostingEnvironment = hostingEnvironment;
            this.ratingService = ratingService;
            this.fbService = fbService;
            this.ratingService = ratingService;
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

        public async Task<ActionResult> TemplateListing()
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

        // GET: Rfq/Edit/5
        [HttpGet]
        public async Task<ActionResult> View(int id)
        {
            try
            {
                string User_Id = HttpContext.Session.GetString("User_Id");
                string Company_Id = HttpContext.Session.GetString("Company_Id");
                string currentCompanyType = HttpContext.Session.GetString("C_BusinessType");

                var userRoles = JsonConvert.DeserializeObject<List<VmRoleItem>>(HttpContext.Session.GetString("SelectedRoles"));
                bool isCompanyAdmin = false;
                if (userRoles.Where(e => e.Id == Constants.C_Admin_Role).ToList().Count > 0)
                {
                    isCompanyAdmin = true;
                }
                ViewBag.isCompanyAdmin = isCompanyAdmin;

                var checkPermissionResult = await rfqService.CheckPermissionForRFQ(Convert.ToInt32(Company_Id), Convert.ToInt32(currentCompanyType), Convert.ToInt32(User_Id), id, isCompanyAdmin);

                if (checkPermissionResult.IsSuccess)
                {
                    var result = await rfqService.GetSingleRfqById(id);

                    bool isRfqApprover = false;
                    int tmpUserId = Convert.ToInt32(User_Id);
                    if (result.Document.DocumentUserDisplay != null)
                    {
                        var tmpDocumentUser = result.Document.DocumentUserDisplay.Where(e => e.User_Id == tmpUserId).FirstOrDefault();
                        if (tmpDocumentUser != null)
                        {
                            if (tmpDocumentUser.Roles.Contains("RFQ Approver"))
                            {
                                isRfqApprover = true;
                            }
                        }
                    }
                    ViewBag.isRfqApprover = isRfqApprover;
                    return View(result);
                }
                else
                {
                    TempData["ErrorMessage"] = "You are not authorized to access this RFQ.";
                    return RedirectToAction("ErrorForUser", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ErrorForUser", "Home");
            }

        }

        [HttpGet]
        public async Task<ActionResult> Revise(int id)
        {
            try
            {
                ViewBag.User_Id = HttpContext.Session.GetString("User_Id");
                ViewBag.Company_Id = HttpContext.Session.GetString("Company_Id");
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                ViewBag.FullName = HttpContext.Session.GetString("ContactName");
                ViewBag.EmailAddress = HttpContext.Session.GetString("EmailAddress");

                string User_Id = HttpContext.Session.GetString("User_Id");
                string Company_Id = HttpContext.Session.GetString("Company_Id");
                string currentCompanyType = HttpContext.Session.GetString("C_BusinessType");

                var userRoles = JsonConvert.DeserializeObject<List<VmRoleItem>>(HttpContext.Session.GetString("SelectedRoles"));
                bool isCompanyAdmin = false;
                if (userRoles.Where(e => e.Id == Constants.C_Admin_Role).ToList().Count > 0)
                {
                    isCompanyAdmin = true;
                }

                var checkPermissionResult = await rfqService.CheckPermissionForRFQ(Convert.ToInt32(Company_Id), Convert.ToInt32(currentCompanyType), Convert.ToInt32(User_Id), id, isCompanyAdmin);

                if (checkPermissionResult.IsSuccess)
                {
                    var result = await rfqService.GetSingleRfqById(id);
                    result.Document.DocumentStatus.Name = "New";
                    result.Document.DocumentStatus_Id = BudgetMetal.Common.Constants_CodeTable.Code_RFQ_Draft;
                    result.Document.DocumentNo = "";
                    result.Document_Id = 0;
                    result.Id = 0;
                    return View(result);
                }
                else
                {
                    TempData["ErrorMessage"] = "You are not authorized to access this RFQ.";
                    return RedirectToAction("ErrorForUser", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ErrorForUser", "Home");
            }

        }

        [HttpPost]
        public async Task<ActionResult> Revise(VmRfqItem Rfq)
        {
            try
            {
                var submitType = Request.Form["btnType"];
                string documentAction = "";
                if (submitType.ToString().ToLower() == "save as draft")
                {
                    Rfq.Document.DocumentStatus_Id = Constants_CodeTable.Code_RFQ_Draft;
                    documentAction = "Save as Draft";
                }
                else
                {
                    Rfq.Document.DocumentStatus_Id = Constants_CodeTable.Code_RFQ_RequiredApproval;
                    documentAction = "Submitted";
                }
                Rfq.SelectedTags = Request.Form["SelectedTags"].ToString();

                // check attachments
                if (Request.Form.Files.Count() > 0)
                {
                    var listAttachment = new List<VmAttachmentItem>();
                    int i = 0;

                    var fileInfoArray = Request.Form["fileDescriptionRFQ[]"].ToArray();

                    var attachedFiles = Request.Form.Files.Where(f => f.Length > 0);

                    if (attachedFiles.Count() > 0 && fileInfoArray.Length > 0)
                    {
                        foreach (var fileAttachment in attachedFiles)
                        {
                            if (fileAttachment.Name.ToLower() != "fupload")
                            {
                                if (fileAttachment.Length > 0)
                                {
                                    var tmpFileNameArr = fileAttachment.FileName.ToString().Split("\\");
                                    string tmpFileName = tmpFileNameArr.Last();
                                    var desc = fileInfoArray[i].ToString();

                                    var att = new VmAttachmentItem
                                    {
                                        FileName = tmpFileName,
                                        FileSize = fileAttachment.Length,
                                        FileBinary = Convert.ToBase64String(ConvertFiletoBytes(fileAttachment)),
                                        Description = desc,
                                        CreatedBy = Rfq.CreatedBy,
                                        UpdatedBy = Rfq.UpdatedBy
                                    };
                                    listAttachment.Add(att);

                                }

                                i++;
                            }


                        }
                    }

                    // assign attachment list
                    Rfq.Document.Attachment = listAttachment;
                }


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
                Rfq.Id = 0;
                var result = await rfqService.SaveRFQ(Rfq);
                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error while creating RFQ. Please contact system administrator.";
                    return RedirectToAction("ErrorForUser", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ErrorForUser", "Home");
            }
        }

        // GET: Rfq/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                string User_Id = HttpContext.Session.GetString("User_Id");
                string Company_Id = HttpContext.Session.GetString("Company_Id");
                string currentCompanyType = HttpContext.Session.GetString("C_BusinessType");

                var userRoles = JsonConvert.DeserializeObject<List<VmRoleItem>>(HttpContext.Session.GetString("SelectedRoles"));
                bool isCompanyAdmin = false;
                if (userRoles.Where(e => e.Id == Constants.C_Admin_Role).ToList().Count > 0)
                {
                    isCompanyAdmin = true;
                }

                var checkPermissionResult = await rfqService.CheckPermissionForRFQ(Convert.ToInt32(Company_Id), Convert.ToInt32(currentCompanyType), Convert.ToInt32(User_Id), id, isCompanyAdmin);

                if (checkPermissionResult.IsSuccess)
                {
                    var result = await rfqService.GetSingleRfqById(id);
                    return View(result);
                }
                else
                {
                    TempData["ErrorMessage"] = "You are not authorized to access this RFQ.";
                    return RedirectToAction("ErrorForUser", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ErrorForUser", "Home");
            }

        }

        [HttpPost]
        public async Task<ActionResult> Edit(VmRfqItem Rfq)
        {
            try
            {
                var submitType = Request.Form["btnType"];
                string documentAction = "";
                if (submitType.ToString().ToLower() == "save as draft")
                {
                    Rfq.Document.DocumentStatus_Id = Constants_CodeTable.Code_RFQ_Draft;
                    documentAction = "Save as Draft";
                }
                else
                {
                    Rfq.Document.DocumentStatus_Id = Constants_CodeTable.Code_RFQ_RequiredApproval;
                    documentAction = "Submitted";
                }
                Rfq.SelectedTags = Request.Form["SelectedTags"].ToString();
                Rfq.Document.UpdatedBy = HttpContext.Session.GetString("UserName");
                Rfq.UpdatedBy = HttpContext.Session.GetString("UserName");
                //var listAttachment = new List<VmAttachmentItem>();
                int i = 0;

                if (Request.Form.Files.Count() > 0)
                {
                    Rfq.Document.Attachment = new List<VmAttachmentItem>();

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

                var result = await rfqService.UpdateRFQ(Rfq);
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
                TempData["ErrorMessage"] = "RFQ Limitation per week is exceed. please contact admin to upgrade your account.";

                return RedirectToAction("ErrorForUser", "Home");
            }

            return View();
        }

        // POST: Rfq/Create
        [HttpPost]
        public async Task<ActionResult> Create(VmRfqItem Rfq)
        {
            try
            {
                var submitType = Request.Form["btnType"];
                string documentAction = "";
                if (submitType.ToString().ToLower() == "save as draft")
                {
                    Rfq.Document.DocumentStatus_Id = Constants_CodeTable.Code_RFQ_Draft;
                    documentAction = "Save as Draft";
                }
                else
                {
                    Rfq.Document.DocumentStatus_Id = Constants_CodeTable.Code_RFQ_RequiredApproval;
                    documentAction = "Submitted";
                }
                Rfq.SelectedTags = Request.Form["SelectedTags"].ToString();

                // check attachments
                if (Request.Form.Files.Count() > 0)
                {
                    var listAttachment = new List<VmAttachmentItem>();
                    int i = 0;

                    var fileInfoArray = Request.Form["fileDescriptionRFQ[]"].ToArray();

                    var attachedFiles = Request.Form.Files.Where(f => f.Length > 0);

                    if (attachedFiles.Count() > 0 && fileInfoArray.Length > 0)
                    {
                        foreach (var fileAttachment in attachedFiles)
                        {
                            if (fileAttachment.Name.ToLower() != "fupload")
                            {
                                if (fileAttachment.Length > 0)
                                {
                                    var tmpFileNameArr = fileAttachment.FileName.ToString().Split("\\");
                                    string tmpFileName = tmpFileNameArr.Last();
                                    var desc = fileInfoArray[i].ToString();

                                    var att = new VmAttachmentItem
                                    {
                                        FileName = tmpFileName,
                                        FileSize = fileAttachment.Length,
                                        FileBinary = Convert.ToBase64String(ConvertFiletoBytes(fileAttachment)),
                                        Description = desc,
                                        CreatedBy = Rfq.CreatedBy,
                                        UpdatedBy = Rfq.UpdatedBy
                                    };
                                    listAttachment.Add(att);
                                }

                                i++;
                            }


                        }
                    }

                    // assign attachment list
                    Rfq.Document.Attachment = listAttachment;
                }

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

                var result = await rfqService.SaveRFQ(Rfq);
                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error while creating RFQ. Please contact system administrator.";
                    return RedirectToAction("ErrorForUser", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ErrorForUser", "Home");
            }
        }


        // GET: Rfq/Create
        public ActionResult CreateTemplate()
        {
            ViewBag.User_Id = HttpContext.Session.GetString("User_Id");
            ViewBag.Company_Id = HttpContext.Session.GetString("Company_Id");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.FullName = HttpContext.Session.GetString("ContactName");
            ViewBag.EmailAddress = HttpContext.Session.GetString("EmailAddress");


            return View();
        }

        // POST: Rfq/Create
        [HttpPost]
        public async Task<ActionResult> CreateTemplate(VmRfqItem Rfq)
        {
            try
            {
               
                Rfq.Document.DocumentStatus_Id = Constants_CodeTable.Code_RFQ_Saved;
                
                


                var result = await rfqService.SaveRFQTemplate(Rfq);
                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error while creating RFQ. Please contact system administrator.";
                    return RedirectToAction("ErrorForUser", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ErrorForUser", "Home");
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
        public async Task<JsonResult> RejectRfq(int documentId)
        {

            var result = await rfqService.RejectRfq(documentId, Convert.ToInt32(HttpContext.Session.GetString("User_Id")), HttpContext.Session.GetString("UserName"));

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> ApproveRfq(int documentId)
        {

            var result = await rfqService.ApproveRfq(documentId, Convert.ToInt32(HttpContext.Session.GetString("User_Id")), HttpContext.Session.GetString("UserName"));

            if (result.IsSuccess)
            {
                var resultForFB = await rfqService.GetRfqInfoForFacebook(documentId);

                if (resultForFB.IsSuccess)
                {
                    var fbResult = await fbService.PostMessage(resultForFB.MessageToUser);
                }
            }
           

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> AddComment(int documentId, string clarification)
        {

            var result = await rfqService.AddClarification(documentId, Convert.ToInt32(HttpContext.Session.GetString("User_Id")), HttpContext.Session.GetString("UserName"), clarification, 0);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> ReplyComment(int documentId, string clarification, int commentId)
        {

            var result = await rfqService.AddClarification(documentId, Convert.ToInt32(HttpContext.Session.GetString("User_Id")), HttpContext.Session.GetString("UserName"), clarification, commentId);

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
        public async Task<JsonResult> NotRelevantRfq(int rfqId)
        {

            var result = await rfqService.NotRelevantRfq(rfqId, Convert.ToInt32(HttpContext.Session.GetString("Company_Id")), HttpContext.Session.GetString("UserName"));

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

        public JsonResult UploadTemplate()
        {
            var resultTemplate = new VmTemplate();
            resultTemplate.result = new VmGenericServiceResult();
            resultTemplate.List_Requirement = new List<VmRequirementItem>();
            resultTemplate.List_SLA = new List<VmSlaItem>();
            resultTemplate.List_Panalty = new List<VmPenaltyItem>();
            resultTemplate.List_Pricing = new List<VmRfqPriceScheduleItem>();

            string newPath = "";

            try
            {
                IFormFile file = Request.Form.Files[0];
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.WebRootPath;
                newPath = Path.Combine(webRootPath, folderName, DateTime.Now.ToString("ddMMyyyyHHmmss"));
                StringBuilder sb = new StringBuilder();
                if (!Directory.Exists(newPath))
                {

                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();

                    int countSheet = 0;
                    string fullPath = Path.Combine(newPath, file.FileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                            countSheet = hssfwb.NumberOfSheets;
                            for (int i = 0; i < countSheet; i++)
                            {
                                ISheet sheet = hssfwb.GetSheetAt(i);
                                GetResultTemplate(resultTemplate, sheet);

                                //if (sheet.SheetName.ToString().ToLower() == "requirements")
                                //{
                                //    resultTemplate.List_Requirement = getRequirementsFromTemplate(sheet);
                                //}
                                //else if (sheet.SheetName.ToString().ToLower() == "sla")
                                //{
                                //    resultTemplate.List_SLA = getSLAFromTemplate(sheet);
                                //}
                                //else if (sheet.SheetName.ToString().ToLower() == "penalty")
                                //{
                                //    resultTemplate.List_Panalty = getPaneltyFromTemplate(sheet);
                                //}
                                //else if (sheet.SheetName.ToString().ToLower() == "pricing")
                                //{
                                //    resultTemplate.List_Pricing = getPricingFromTemplate(sheet);
                                //}
                                //else
                                //{
                                //    //nothing
                                //}
                            }

                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                            countSheet = hssfwb.NumberOfSheets;
                            for (int i = 0; i < countSheet; i++)
                            {
                                ISheet sheet = hssfwb.GetSheetAt(i);

                                GetResultTemplate(resultTemplate, sheet);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resultTemplate.result.IsSuccess = false;
                resultTemplate.result.MessageToUser = "Error while uploading template." + ex.Message + ". Target path - " + newPath;
            }



            return new JsonResult(resultTemplate, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        private void GetResultTemplate(VmTemplate resultTemplate, ISheet sheet)
        {
            switch (sheet.SheetName.ToString().ToLower())
            {
                case "product req":
                    resultTemplate.List_Requirement = getRequirementsFromTemplate(sheet);
                    break;

                case "support req":
                    resultTemplate.List_SLA = getSLAFromTemplate(sheet);
                    break;

                case "commercial req":
                    resultTemplate.List_Panalty = getPaneltyFromTemplate(sheet);
                    break;

                case "pricing req":
                    resultTemplate.List_Panalty = getPaneltyFromTemplate(sheet);
                    break;

                case "pricing format":
                    resultTemplate.List_Pricing = getProductPricingFrom(sheet, Com.BudgetMetal.Common.Constants_CodeTable.Code_RfqPriceCategory_Product);
                    resultTemplate.List_Service_Pricing = getServicesPricingFrom(sheet, Com.BudgetMetal.Common.Constants_CodeTable.Code_RfqPriceCategory_Service);
                    resultTemplate.List_Waranty_Pricing = getWarrantyPricingFrom(sheet, Com.BudgetMetal.Common.Constants_CodeTable.Code_RfqPriceCategory_Warranty);
                    break;

                //case "service pricing":
                //    resultTemplate.List_Service_Pricing = getPricingFromTemplate(sheet, Com.BudgetMetal.Common.Constants_CodeTable.Code_RfqPriceCategory_Service);
                //    break;

                //case "warranty pricing":
                //    resultTemplate.List_Waranty_Pricing = getPricingFromTemplate(sheet, Com.BudgetMetal.Common.Constants_CodeTable.Code_RfqPriceCategory_Warranty);
                //    break;

            }
        }

        private List<VmRfqPriceScheduleItem> getProductPricingFrom(ISheet sheet, int categoryId)
        {
            var result = new List<VmRfqPriceScheduleItem>();
            IRow headerRow = sheet.GetRow(0); //Get Header Row
            bool tfProcess = false;
            try
            {
                int cellCount = headerRow.LastCellNum;

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    try
                    {
                        if (row.GetCell(0).ToString() == "Products")
                        {
                            tfProcess = true;
                            continue;
                        }

                        if (tfProcess)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(1).ToString()) || !string.IsNullOrEmpty(row.GetCell(2).ToString()) || !string.IsNullOrEmpty(row.GetCell(3).ToString()))
                            {
                                var resultItem = new VmRfqPriceScheduleItem();
                                resultItem.ItemName = row.GetCell(1).ToString();
                                resultItem.ItemDescription = row.GetCell(2).ToString();
                                //resultItem.InternalRefrenceCode = row.GetCell(3).ToString();
                                resultItem.QuantityRequired = row.GetCell(3).ToString();
                                resultItem.CategoryId = categoryId;
                                result.Add(resultItem);
                            }
                        }

                        if (row.GetCell(0).ToString() == "Services") break;
                        if (row.GetCell(0).ToString() == "Warranty") break;
                    }
                    catch
                    {
                        //nothing
                    }
                }
            }
            catch
            {

            }
            return result;
        }

        private List<VmRfqPriceScheduleItem> getServicesPricingFrom(ISheet sheet, int categoryId)
        {
            var result = new List<VmRfqPriceScheduleItem>();
            IRow headerRow = sheet.GetRow(0); //Get Header Row
            bool tfProcess = false;
            try
            {
                int cellCount = headerRow.LastCellNum;

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    try
                    {
                        if (row.GetCell(0).ToString() == "Services")
                        {
                            tfProcess = true;
                            continue;
                        }

                        if (tfProcess)
                        {
                            var resultItem = new VmRfqPriceScheduleItem();
                            if (!string.IsNullOrEmpty(row.GetCell(1).ToString()) || !string.IsNullOrEmpty(row.GetCell(2).ToString()) || !string.IsNullOrEmpty(row.GetCell(3).ToString()))
                            {
                                resultItem.ItemName = row.GetCell(1).ToString();
                                resultItem.ItemDescription = row.GetCell(2).ToString();
                                //resultItem.InternalRefrenceCode = row.GetCell(3).ToString();
                                resultItem.QuantityRequired = row.GetCell(3).ToString();
                                resultItem.CategoryId = categoryId;
                                result.Add(resultItem);
                            }
                        }

                        if (row.GetCell(0).ToString() == "Warranty") break;
                    }
                    catch
                    {
                        //nothing
                    }
                }
            }
            catch
            {

            }
            return result;
        }

        private List<VmRfqPriceScheduleItem> getWarrantyPricingFrom(ISheet sheet, int categoryId)
        {
            var result = new List<VmRfqPriceScheduleItem>();
            IRow headerRow = sheet.GetRow(0); //Get Header Row
            bool tfProcess = false;
            try
            {
                int cellCount = headerRow.LastCellNum;

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    try
                    {
                        if (row.GetCell(0).ToString() == "Warranty")
                        {
                            tfProcess = true;
                            continue;
                        }

                        if (tfProcess)
                        {
                            var resultItem = new VmRfqPriceScheduleItem();
                            if (!string.IsNullOrEmpty(row.GetCell(1).ToString()) || !string.IsNullOrEmpty(row.GetCell(2).ToString()) || !string.IsNullOrEmpty(row.GetCell(3).ToString()))
                            {
                                resultItem.ItemName = row.GetCell(1).ToString();
                                resultItem.ItemDescription = row.GetCell(2).ToString();
                                //resultItem.InternalRefrenceCode = row.GetCell(3).ToString();
                                resultItem.QuantityRequired = row.GetCell(3).ToString();
                                resultItem.CategoryId = categoryId;
                                result.Add(resultItem);
                            }
                        }
                    }
                    catch
                    {
                        //nothing
                    }
                }
            }
            catch
            {

            }
            return result;
        }

        private List<VmRfqPriceScheduleItem> getPricingFromTemplate(ISheet sheet, int categoryId)
        {
            var result = new List<VmRfqPriceScheduleItem>();
            IRow headerRow = sheet.GetRow(0); //Get Header Row

            try
            {
                int cellCount = headerRow.LastCellNum;

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                {

                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    try
                    {
                        var resultItem = new VmRfqPriceScheduleItem();
                        resultItem.ItemName = row.GetCell(0).ToString();
                        resultItem.ItemDescription = row.GetCell(1).ToString();
                        resultItem.InternalRefrenceCode = row.GetCell(2).ToString();
                        resultItem.QuantityRequired = row.GetCell(3).ToString();
                        resultItem.CategoryId = categoryId;

                        result.Add(resultItem);
                    }
                    catch
                    {
                        //nothing
                    }
                }
            }
            catch
            {

            }

            return result;
        }

        private List<VmPenaltyItem> getPaneltyFromTemplate(ISheet sheet)
        {
            var result = new List<VmPenaltyItem>();
            IRow headerRow = sheet.GetRow(0); //Get Header Row

            try
            {
                int cellCount = headerRow.LastCellNum;

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                {

                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    try
                    {
                        if (!string.IsNullOrEmpty(row.GetCell(1).ToString()) || !string.IsNullOrEmpty(row.GetCell(2).ToString()))
                        {
                            var resultItem = new VmPenaltyItem();
                            resultItem.BreachOfServiceDefinition = row.GetCell(1).ToString();
                            resultItem.Description = row.GetCell(2).ToString();
                            //resultItem.PenaltyAmount = row.GetCell(2).ToString();

                            result.Add(resultItem);
                        }
                    }
                    catch
                    {
                        //nothing
                    }
                }
            }
            catch
            {

            }
            return result;
        }

        private List<VmSlaItem> getSLAFromTemplate(ISheet sheet)
        {
            var result = new List<VmSlaItem>();
            IRow headerRow = sheet.GetRow(0); //Get Header Row

            try
            {
                int cellCount = headerRow.LastCellNum;

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                {

                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    try
                    {
                        var resultItem = new VmSlaItem();
                        if (!string.IsNullOrEmpty(row.GetCell(1).ToString()) || !string.IsNullOrEmpty(row.GetCell(2).ToString()))
                        {
                            resultItem.Requirement = row.GetCell(1).ToString();
                            resultItem.Description = row.GetCell(2).ToString();

                            result.Add(resultItem);
                        }
                    }
                    catch
                    {
                        //nothing
                    }
                }
            }
            catch
            {

            }

            return result;
        }

        private List<VmRequirementItem> getRequirementsFromTemplate(ISheet sheet)
        {
            var result = new List<VmRequirementItem>();
            IRow headerRow = sheet.GetRow(0); //Get Header Row
            try
            {
                int cellCount = headerRow.LastCellNum;

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                {

                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    try
                    {
                        var resultItem = new VmRequirementItem();
                        if (!string.IsNullOrEmpty(row.GetCell(1).ToString()) || !string.IsNullOrEmpty(row.GetCell(2).ToString()))
                        {
                            resultItem.ServiceName = row.GetCell(1).ToString();
                            resultItem.Description = row.GetCell(2).ToString();

                            result.Add(resultItem);
                        }
                    }
                    catch
                    {
                        //nothing
                    }
                }
            }
            catch
            {

            }
            return result;
        }

        public ActionResult DownloadExcelTemplate()
        {
            string filePath = "wwwroot/Template/RFQ_Template.xlsx";
            string fileName = "RFQ_Template.xlsx";

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/force-download", fileName);

        }

        [HttpGet]
        public async Task<JsonResult> ResendEmail(string Email, string RfqId)
        {
            int rfqId = 0;
            if (!string.IsNullOrEmpty(RfqId))
            {
                rfqId = int.Parse(RfqId);
            }
            var result = await rfqService.ResendEmail(Email, rfqId);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> AddInvitationUser(string RfqId, string name, string email)
        {
            int rfqId = 0;
            if (!string.IsNullOrEmpty(RfqId))
            {
                rfqId = int.Parse(RfqId);
            }
            var result = await rfqService.AddInvitationUser(rfqId, name, email, HttpContext.Session.GetString("UserName"));

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpPost]
        public async Task<JsonResult> AddRatingByBuyerToSupplier(int DocumentId, int CompanyId, int ServiceRating, int SpeedOfQuotation, int SpeedOfDelivery, int PricingRating, string Title, string Description)
        {
            var User_Id = HttpContext.Session.GetString("User_Id");

            VmRatingItem item = new VmRatingItem() {
                Document_Id = DocumentId,
                Company_Id = CompanyId,
                ServiceQuality = ServiceRating,
                SpeedOfQuotation = SpeedOfQuotation,
                SpeedofDelivery = SpeedOfDelivery,
                Price = PricingRating,
                Title = Title,
                Description = Description,
                User_Id = Convert.ToInt32(User_Id),
            };            

            var result = await ratingService.Insert(item);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
    }
}