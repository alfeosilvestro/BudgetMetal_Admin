using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Com.BudgetMetal.Services.RFQ;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Com.BudgetMetal.Services.Industries;
using Com.BudgetMetal.Services.Quotation;
using Com.BudgetMetal.Services.Company;
using Com.BudgetMetal.Services.ServiceTags;
using Com.BudgetMetal.Services.Users;
using Com.BudgetMetal.Services.Attachment;
using Microsoft.Extensions.Options;
using Com.BudgetMetal.ViewModels.User;
using System.Text;
using Com.BudgetMetal.Common;
using System.IO;
using Com.EazyTender.WebApp.Configurations;
using Com.BudgetMetal.Services.Blogs;

namespace Com.EzTender.WebApp.Controllers
{
    public class PublicController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly IIndustryService industryService;
        private readonly IRFQService rfqService;
        private readonly ICompanyService companyService;
        private readonly IServiceTagsService serviceTagsService;
        private readonly IUserService userService;
        private readonly IAttachmentService attachmentService;
        private readonly IBlogService blogService;        

        public PublicController(IRFQService rfqService,
                              ICompanyService companyService,
                              IServiceTagsService serviceTagsService,
                              IIndustryService industryService,
                              IOptions<AppSettings> appSettings,
                              IUserService userService,                              
                              IAttachmentService attachmentService, IBlogService blogService)
        {
            this.rfqService = rfqService;
            this.industryService = industryService;
            this.companyService = companyService;
            this.serviceTagsService = serviceTagsService;
            this.userService = userService;
            this._appSettings = appSettings.Value;
            this.attachmentService = attachmentService;
            this.blogService = blogService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Blog(string keyword, int page, int totalRecords)
        {
            var result = await blogService.GetBlogsByPage(keyword, page, _appSettings.TotalRecordPerPage);

            return View(result);
        }

        public IActionResult RFQ()
        {
            return View();
        }

        public IActionResult TenderBoard()
        {
            return View();
        }

        public IActionResult CompanyPortal()
        {
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult ResetPass()
        {
            string username = Request.Query["u"];
            ViewBag.u = System.Net.WebUtility.UrlEncode(username);
            return View();
        }

        public IActionResult ConfirmUser()
        {
            string _token = Request.Query["token"];
            string username = Request.Query["e"];
            string timeLimit = Request.Query["t"];
            ViewBag.token = _token;
            ViewBag.e = username;
            ViewBag.t = timeLimit;
            return View();
        }

        public IActionResult SignIn()
        {
            string _cul = Request.Query["culture"];
            
            if(_cul != null)
            {
                ViewBag.culture = _cul;
            }
            else
            {
                ViewBag.culture = "";
            }
            return View();
        }

        public IActionResult CompletedSignIn()
        {
            return View();
        }

        public IActionResult RfqAccess()
        {
            return View();
        }

        public IActionResult SingleRFQ(int id)
        {
            string code = Request.Query["c"];
            ViewBag.Id = id;
            ViewBag.Code = code;
            return View();
        }
        // GET: Rfq/Edit/5
        [HttpGet]
        public async Task<ActionResult> Detail(int id)
        {            
            try
            {
                string code = Request.Query["code"];
                ViewBag.Code = code;
                var result = await rfqService.GetPublicPortalSingleRfqById(id);
                TempData["LoginUrl"] = _appSettings.App_Identity.WebAppUrl + "Public/SignIn";
                return View(result);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }

        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult FAQ()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Policy()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetPublicRFQ(int page, string status, string skeyword)
        {
            var result = await rfqService.GetPublicRfqByPage(page, 12, Convert.ToInt32(status),
                skeyword == null ? "" : skeyword);

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

        [HttpGet]
        public async Task<JsonResult> GetLoadTenderNoticBoardPublicRFQ(int count)
        {
            var result = await rfqService.GetLoadTenderNoticBoardPublicRFQ(count);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpPost]
        public JsonResult SendEmailFromContactUs(string senderEmail, string senderName, string subject, string content)
        {
            var result = new Com.BudgetMetal.ViewModels.VmGenericServiceResult();

            try
            {
                SendingMail sm = new SendingMail();
                string email = _appSettings.App_Identity.fromMail;
                string emailSubject = "Message From Contact Page";
                string emailBody = "";

                emailBody = "From : " + senderEmail + "<br>";
                emailBody = emailBody + "Name : " + senderName + "<br>";
                emailBody = emailBody + "Subject : " + subject + "<br><br>";
                emailBody = emailBody + "Body : <br><br>" + content.Replace("\n", "<br>") + "<br>";
                sm.SendMail(email, "", emailSubject, emailBody);

                result.IsSuccess = true;
                result.MessageToUser = "OK";
            }
            catch
            {
                result.IsSuccess = false;
                result.MessageToUser = "Error while sending email.";
            }
           


            
            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

    }
}