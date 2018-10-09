using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Com.EzTender.PublicPortal.Models;
using Com.BudgetMetal.Services.RFQ;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Com.BudgetMetal.Services.Industries;
using Com.BudgetMetal.Services.Quotation;
using Configurations;
using Com.BudgetMetal.Services.Company;
using Com.BudgetMetal.Services.ServiceTags;
using Com.BudgetMetal.Services.Users;
using Com.BudgetMetal.Services.Attachment;
using Microsoft.Extensions.Options;
using Com.BudgetMetal.ViewModels.User;
using System.Text;
using Com.BudgetMetal.Common;
using System.IO;

namespace Com.EzTender.PublicPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly IIndustryService industryService;
        private readonly IRFQService rfqService;
        private readonly IQuotationService quotationService;
        private readonly ICompanyService companyService;
        private readonly IServiceTagsService serviceTagsService;
        private readonly IUserService userService;
        private readonly IAttachmentService attachmentService;

        public HomeController(IRFQService rfqService, IIndustryService industryService, IQuotationService quotationService, ICompanyService companyService, IServiceTagsService serviceTagsService, IUserService userService, IOptions<AppSettings> appSettings, IAttachmentService attachmentService)
        {
            this.rfqService = rfqService;
            this.industryService = industryService;
            this.quotationService = quotationService;
            this.companyService = companyService;
            this.serviceTagsService = serviceTagsService;
            this.userService = userService;
            this._appSettings = appSettings.Value;
            this.attachmentService = attachmentService;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(VmUserItem user)
        {
            string[] serviceTags;
            serviceTags = Request.Form["serviceTags"].ToArray();

            var result = await userService.Register(user, serviceTags);
            if (result.IsSuccess)
            {
                string mailBody = "";
                string confirmationLink = "";

                var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");

                var url = location.AbsoluteUri;
                string encodeEmail = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.EmailAddress));
                string token = _appSettings.App_Identity.Identity;
                var byte_time = Encoding.UTF8.GetBytes(DateTime.Now.AddDays(2).ToString("yyyy-mm-dd"));
                string encodeTimeLimit = Convert.ToBase64String(byte_time);
                confirmationLink = url + "Home/ConfirmUser?token=" + token + "&e=" + encodeEmail + "&t=" + encodeTimeLimit;

                mailBody = "Please confirm your account by clicking the following link \n " + confirmationLink;
                SendingMail sm = new SendingMail();
                sm.SendMail(user.EmailAddress, "", "Confirmation for Registration", mailBody);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // GET: Rfq/Edit/5
        [HttpGet]
        public async Task<ActionResult> Detail(int id)
        {
            try
            {
                var result = await rfqService.GetPublicPortalSingleRfqById(id);

                return View(result);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }

        }

        public async Task<IActionResult> PublicRFQ()
        {
            ViewBag.Company_Id = HttpContext.Session.GetString("Company_Id");

            return View();
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<JsonResult> GetRFQForDashboard(int page, string status, string Company_Id, string skeyword)
        {

            var result = await rfqService.GetRfqByPage(Convert.ToInt32(Company_Id), page, 2, Convert.ToInt32(status),
                skeyword == null ? "" : skeyword);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetPublicRFQ(int page, string status, string skeyword)
        {
            var result = await rfqService.GetPublicRfqByPage(page, 2, Convert.ToInt32(status),
                skeyword == null ? "" : skeyword);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
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
        public async Task<JsonResult> GetQuotationByRfqId(int RfqId, int page, string keyword)
        {
            var result = await quotationService.GetQuotationByRfqId(RfqId, page, 10, 0, (keyword == null ? "" : keyword));

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> CheckUEN(string RegNo)
        {
            var result = await companyService.GetCompanyByUEN(RegNo);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> CheckEmail(string Email)
        {
            var result = await userService.CheckEmail(Email);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<string> LoadServiceTags()
        {
            var resultIndustry = await industryService.GetActiveIndustries();

            var resultServiceTags = await serviceTagsService.GetActiveVmServiceTags();

            string returnString = "";
            if (resultIndustry != null && resultServiceTags != null)
            {
                foreach (var itemIndustry in resultIndustry)
                {
                    returnString = returnString + "<optgroup label='" + itemIndustry.Name + "'>";
                    var resultServiceTagsByIndustry = resultServiceTags.Where(e => e.Industry_Id == itemIndustry.Id).ToList();
                    foreach (var itemServiceTag in resultServiceTagsByIndustry)
                    {
                        returnString = returnString + "<option value='" + itemServiceTag.Id.ToString() + "'>" + itemServiceTag.Name + "</option>";
                    }

                    returnString = returnString + "</optgroup>";
                }
            }


            return returnString;
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
