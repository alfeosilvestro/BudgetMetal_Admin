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
using Com.BudgetMetal.Services.Company;
using Com.BudgetMetal.Services.ServiceTags;
using Com.BudgetMetal.Services.Users;
using Com.BudgetMetal.ViewModels.User;
using System.Net.Mail;
using System.Net;
using Com.BudgetMetal.Common;
using System.Text;
using Configurations;
using Microsoft.Extensions.Options;

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

        public HomeController(IRFQService rfqService, IIndustryService industryService, IQuotationService quotationService, ICompanyService companyService, IServiceTagsService serviceTagsService, IUserService userService, IOptions<AppSettings> appSettings)
        {
            this.rfqService = rfqService;
            this.industryService = industryService;
            this.quotationService = quotationService;
            this.companyService = companyService;
            this.serviceTagsService = serviceTagsService;
            this.userService = userService;
            this._appSettings = appSettings.Value;


        }

        public IActionResult Index()
        {

            return View();
        }

        // GET: User
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            HttpContext.Session.SetString("WebAppUrl", _appSettings.App_Identity.WebAppUrl);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmUser()
        {
            HttpContext.Session.SetString("WebAppUrl", _appSettings.App_Identity.WebAppUrl);
            string token = _appSettings.App_Identity.Identity;
            string _token = Request.Query["token"];
            //HttpContext.Session.SetString("WebAppUrl", _appSettings.App_Identity.WebAppUrl);
            if (token == _token)
            {
                string email = Request.Query["e"];
                string timeLimit = Request.Query["t"];
                if (timeLimit != null && email != null)
                {
                    email = email.DecodeString(); // Encoding.UTF8.GetString(Convert.FromBase64String(email));
                    timeLimit = timeLimit.DecodeString(); // Encoding.UTF8.GetString(Convert.FromBase64String(timeLimit));

                    if (DateTime.Now < Convert.ToDateTime(timeLimit))
                    {
                        var result = await userService.ConfirmEmail(email);

                        if (result.IsSuccess)
                        {
                            TempData["IsSuccess"] = true;
                            TempData["Message"] = "You have successfully confirmed your account.";
                        }
                        else
                        {
                            TempData["IsSuccess"] = false;
                            TempData["Message"] = "Fail to confrim your account. " + result.MessageToUser;
                        }
                    }
                    else
                    {
                        TempData["IsSuccess"] = false;
                        TempData["Message"] = "Link Expired.";
                    }
                }
                else
                {
                    TempData["IsSuccess"] = false;
                    TempData["Message"] = "Invalid link.";
                }
            }
            else
            {
                TempData["IsSuccess"] = false;
                TempData["Message"] = "Invalid link.";
            }
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

                //var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
                var location = new Uri($"{Request.Scheme}://{Request.Host}");
                var url = location.AbsoluteUri;
                string encodeEmail = user.EmailAddress.EncodeString(); // Convert.ToBase64String(Encoding.UTF8.GetBytes(user.EmailAddress));
                string token = _appSettings.App_Identity.Identity;
                //var byte_time = Encoding.UTF8.GetBytes(DateTime.Now.AddDays(2).ToString());
                string encodeTimeLimit = DateTime.Now.AddDays(2).ToString().EncodeString(); // Convert.ToBase64String(byte_time);
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
        public async Task<JsonResult> ResetPassword(string Email)
        {
            string newPassword = CreateRandomPassword(6);
            var result = await userService.ResetPassword(Email, newPassword);

            if (result.IsSuccess)
            {
                string mailBody = "";
                mailBody = "Your new password for EzyTender is " + newPassword;
                SendingMail sm = new SendingMail();
                sm.SendMail(Email, "", "Reset Password", mailBody);
            }

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        public string CreateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "0123456789asdfghjklqwertyuiopzxcvbnm";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
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
    }
}
