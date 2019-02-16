using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Common;
using Com.BudgetMetal.Services.Attachment;
using Com.BudgetMetal.Services.Quotation;
using Com.BudgetMetal.Services.Roles;
using Com.BudgetMetal.Services.Users;
using Com.BudgetMetal.ViewModels.Attachment;
using Com.BudgetMetal.ViewModels.Document;
using Com.BudgetMetal.ViewModels.DocumentActivity;
using Com.BudgetMetal.ViewModels.DocumentUser;
using Com.BudgetMetal.ViewModels.Quotation;
using Com.BudgetMetal.ViewModels.Role;
using Com.EazyTender.WebApp.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Com.EzTender.WebApp.Controllers
{
    public class QuotationController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly IAttachmentService attachmentService;
        private readonly IQuotationService quotationService;

        public QuotationController(IQuotationService quotationService, IUserService userService, IRoleService roleService, IAttachmentService attachmentService, IOptions<AppSettings> appSettings)
        {
            this.quotationService = quotationService;
            this.userService = userService;
            this.roleService = roleService;
            this.attachmentService = attachmentService;
            this._appSettings = appSettings.Value;
        }

        // GET: Quotation
        [HttpGet]
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
            var result = await quotationService.GetQuotationForBuyerByPage(Convert.ToInt32(User_Id), Convert.ToInt32(Company_Id), page, 10, isCompanyAdmin);
            return View(result);
        }

        // GET: Quotation
        [HttpGet]
        public async Task<ActionResult> Listing()
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

            var result = await quotationService.GetQuotationByPage(Convert.ToInt32(User_Id), Convert.ToInt32(Company_Id), page, 10, isCompanyAdmin);
            return View(result);
        }

        // GET: Quotation/Edit/5
        [HttpGet]
        public async Task<ActionResult> View(int id)
        {
            try
            {
                ViewBag.User_Id = HttpContext.Session.GetString("User_Id");
                ViewBag.Company_Id = HttpContext.Session.GetString("Company_Id");
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                ViewBag.FullName = HttpContext.Session.GetString("ContactName");

                var userRoles = JsonConvert.DeserializeObject<List<VmRoleItem>>(HttpContext.Session.GetString("SelectedRoles"));
                bool isCompanyAdmin = false;
                if (userRoles.Where(e => e.Id == Constants.C_Admin_Role).ToList().Count > 0)
                {
                    isCompanyAdmin = true;
                }
                ViewBag.CompanyAdmin = isCompanyAdmin;

                string User_Id = HttpContext.Session.GetString("User_Id");
                string Company_Id = HttpContext.Session.GetString("Company_Id");
                string currentCompanyType = HttpContext.Session.GetString("C_BusinessType");

                var checkPermissionResult = await quotationService.CheckPermissionForQuotation(Convert.ToInt32(Company_Id), Convert.ToInt32(currentCompanyType), Convert.ToInt32(User_Id), id, isCompanyAdmin);

                if (checkPermissionResult.IsSuccess)
                {
                    var result = await quotationService.GetSingleQuotationById(id);
                    int userId = Convert.ToInt32(HttpContext.Session.GetString("User_Id"));
                    int roleId = Convert.ToInt32(Constants.QuotationDefaultRoleId);

                    ViewBag.DocumentOwner = true;
                    if (result.Document.DocumentUserDisplay.Where(e => e.User_Id == userId && e.Roles.Contains(Constants.QuotationDefaultRole)).ToList().Count > 0)
                    {
                        ViewBag.DocumentOwner = true;
                    }
                    return View(result);
                }
                else
                {
                    TempData["ErrorMessage"] = "You are not authorized to access this Quotation.";
                    return RedirectToAction("ErrorForUser", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ErrorForUser", "Home");
            }

        }

        // GET: Quotation/Edit/5
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

                var checkPermissionResult = await quotationService.CheckPermissionForQuotation(Convert.ToInt32(Company_Id), Convert.ToInt32(currentCompanyType), Convert.ToInt32(User_Id), id, isCompanyAdmin);
                if (checkPermissionResult.IsSuccess)
                {
                    var result = await quotationService.GetSingleQuotationById(id);

                    return View(result);
                }
                else
                {
                    TempData["ErrorMessage"] = "You are not authorized to access this Quotation.";
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
        public async Task<JsonResult> CancelQuotation(int documentId)
        {

            var result = await quotationService.CancelQuotation(documentId, Convert.ToInt32(HttpContext.Session.GetString("User_Id")), HttpContext.Session.GetString("UserName"));

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> DecideQuotation(int documentId, int isAccept)
        {

            var result = await quotationService.DecideQuotation(documentId, Convert.ToInt32(HttpContext.Session.GetString("User_Id")), HttpContext.Session.GetString("UserName"), ((isAccept == 1) ? true : false));

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
        [HttpPost]
        public ActionResult Edit(VmQuotationItem quotationItem)
        {
            try
            {
                var submitType = Request.Form["btnType"];
                string documentAction = "";
                if (submitType.ToString().ToLower() == "save as draft")
                {
                    quotationItem.Document.DocumentStatus_Id = Constants_CodeTable.Code_Quotation_Draft;
                    documentAction = "Save as Draft";
                }
                else
                {
                    quotationItem.Document.DocumentStatus_Id = Constants_CodeTable.Code_Quotation_Submitted;
                    documentAction = "Submitted";
                }

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
                            Description = Request.Form["fileDescriptionQuotation[]"].ToArray()[i].ToString(),
                            CreatedBy = quotationItem.CreatedBy,
                            UpdatedBy = quotationItem.UpdatedBy
                        };
                        listAttachment.Add(att);
                    }
                    i++;
                }

                quotationItem.Document.Attachment = listAttachment;

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
                quotationItem.Document.DocumentUser = listDocumentUser;

                var listDocumentActivity = new List<VmDocumentActivityItem>();
                var DocumentActivity = new VmDocumentActivityItem()
                {
                    User_Id = Convert.ToInt32(HttpContext.Session.GetString("User_Id")),
                    IsRfq = false,
                    Action = documentAction,
                };
                listDocumentActivity.Add(DocumentActivity);
                quotationItem.Document.DocumentActivityList = listDocumentActivity;

                //string documentNo = quotationService.UpdateQuotation(quotationItem);

                //return RedirectToAction("Index");

                var result = quotationService.UpdateQuotation(quotationItem);
                if (result.IsSuccess)
                {
                    return RedirectToAction("Listing");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error on updating quotation.";
                    return RedirectToAction("ErrorForUser", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ErrorForUser", "Home");
            }
        }

        // GET: Quotation/Create/RefId
        [HttpGet]
        public async Task<ActionResult> Create(int id)
        {
            try
            {
                ViewBag.User_Id = HttpContext.Session.GetString("User_Id");
                ViewBag.Company_Id = HttpContext.Session.GetString("Company_Id");
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                ViewBag.FullName = HttpContext.Session.GetString("ContactName");
                if (quotationService.CheckQuotationLimit(Convert.ToInt32(HttpContext.Session.GetString("Company_Id"))) == false)
                {
                    TempData["message"] = "Quotation Limitation per week is exceed. please contact admin to upgrade your account.";
                    return RedirectToAction("Index");
                }
                var result = await quotationService.InitialLoadByRfqId(id);
                result.Document = new VmDocumentItem();
                result.Document.ContactPersonName = HttpContext.Session.GetString("UserName");
                result.Document.DocumentStatus_Id = Constants_CodeTable.Code_Quotation_Draft;
                result.Document.DocumentType_Id = Constants_CodeTable.Code_Quotation;
                result.Document.Company_Id = Convert.ToInt32(HttpContext.Session.GetString("Company_Id"));
                result.Document.ContactPersonName = HttpContext.Session.GetString("ContactName");
                result.Document.CreatedBy = result.Document.UpdatedBy = result.CreatedBy = result.UpdatedBy = HttpContext.Session.GetString("UserName");
                result.Document.DocumentUser = new List<VmDocumentUserItem>();
                var documentUser = new VmDocumentUserItem
                {
                    User_Id = Convert.ToInt32(HttpContext.Session.GetString("User_Id")),
                    Role_Id = Convert.ToInt32(Constants.QuotationDefaultRoleId)
                };
                result.Document.DocumentUser.Add(documentUser);

                result.Document.DocumentActivityList = new List<VmDocumentActivityItem>();
                var DocumentActivity = new VmDocumentActivityItem()
                {
                    User_Id = Convert.ToInt32(HttpContext.Session.GetString("User_Id")),
                    IsRfq = false,
                    Action = "Create",

                };
                result.Document.DocumentActivityList.Add(DocumentActivity);

                var resultSaving = quotationService.SaveQuotation(result);

                if (resultSaving.IsSuccess)
                {
                    return RedirectToAction("Edit", new { id = resultSaving.MessageToUser });
                }
                else
                {
                    TempData["ErrorMessage"] = "Error on updating quotation.";
                    return RedirectToAction("ErrorForUser", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ErrorForUser", "Home");
            }
        }

        // POST: Quotation/Create
        [HttpPost]
        public ActionResult Create(VmQuotationItem quotationItem)
        {
            try
            {

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
                            Description = Request.Form["fileDescriptionQuotation[]"].ToArray()[i].ToString(),
                            CreatedBy = quotationItem.CreatedBy,
                            UpdatedBy = quotationItem.UpdatedBy
                        };

                        listAttachment.Add(att);

                    }
                    i++;
                }

                quotationItem.Document.Attachment = listAttachment;

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
                quotationItem.Document.DocumentUser = listDocumentUser;

                var listDocumentActivity = new List<VmDocumentActivityItem>();
                var DocumentActivity = new VmDocumentActivityItem()
                {
                    User_Id = Convert.ToInt32(HttpContext.Session.GetString("User_Id")),
                    IsRfq = true,
                    Action = "Create",

                };
                listDocumentActivity.Add(DocumentActivity);
                quotationItem.Document.DocumentActivityList = listDocumentActivity;

                var result = quotationService.SaveQuotation(quotationItem);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetActiveRoles()
        {
            var result = await roleService.GetActiveRoles("quotation");

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
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
        public async Task<JsonResult> AddComment(int documentId, string clarification)
        {

            var result = await quotationService.AddClarification(documentId, Convert.ToInt32(HttpContext.Session.GetString("User_Id")), HttpContext.Session.GetString("UserName"), clarification, 0);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> ReplyComment(int documentId, string clarification, int commentId)
        {

            var result = await quotationService.AddClarification(documentId, Convert.ToInt32(HttpContext.Session.GetString("User_Id")), HttpContext.Session.GetString("UserName"), clarification, commentId);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

    }
}