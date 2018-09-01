using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Services.Attachment;
using Com.BudgetMetal.Services.Quotation;
using Com.BudgetMetal.Services.Roles;
using Com.BudgetMetal.Services.Users;
using Com.BudgetMetal.ViewModels.Attachment;
using Com.BudgetMetal.ViewModels.DocumentUser;
using Com.BudgetMetal.ViewModels.Quotation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Com.EzTender.WebApp.Controllers
{
    public class QuotationController : Controller
    {
        private readonly IUserService userService;
       private readonly IRoleService roleService;
        private readonly IAttachmentService attachmentService;
        private readonly IQuotationService quotationService;

        public QuotationController( IQuotationService quotationService, IUserService userService, IRoleService roleService, IAttachmentService attachmentService)
        {
            this.quotationService = quotationService;
            this.userService = userService;
            this.roleService = roleService;
            this.attachmentService = attachmentService;
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
            var result = await quotationService.GetQuotationByPage(0, page, 10);
            return View(result);
        }

        // GET: Quotation/Edit/5
        [HttpGet]
        public async Task<ActionResult> View(int id)
        {
            try
            {
                var result = await quotationService.GetSingleQuotationById(id);

                return View(result);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }

        }

        // GET: Quotation/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var result = await quotationService.GetSingleQuotationById(id);

                return View(result);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public ActionResult Edit(VmQuotationItem quotationItem)
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

                string documentNo = quotationService.UpdateQuotation(quotationItem);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Quotation/Create/RefId
        [HttpGet]
        public async Task<ActionResult> Create(int id)
        {
            try
            {
                HttpContext.Session.SetString("User_Id", "1");
                HttpContext.Session.SetString("Company_Id", "1");
                HttpContext.Session.SetString("UserName", "Peter");
                HttpContext.Session.SetString("FullName", "Peter");

                ViewBag.User_Id = HttpContext.Session.GetString("User_Id");
                ViewBag.Company_Id = HttpContext.Session.GetString("Company_Id");
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                ViewBag.FullName = HttpContext.Session.GetString("FullName");

                var result = await quotationService.InitialLoadByRfqId(id);

                return View(result);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
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

                string documentNo = quotationService.SaveQuotation(quotationItem);

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
    }
}