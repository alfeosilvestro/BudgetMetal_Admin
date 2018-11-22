using Microsoft.AspNetCore.Mvc;
using Com.BudgetMetal.Services.RFQ;
using Com.BudgetMetal.Services.Industries;
using Configurations;
using Com.BudgetMetal.Services.Company;
using Com.BudgetMetal.Services.ServiceTags;
using Com.BudgetMetal.Services.Users;
using Com.BudgetMetal.Services.Attachment;
using Microsoft.Extensions.Options;
using Com.BudgetMetal.ViewModels.User;
using System.Threading.Tasks;
using System;
using Com.BudgetMetal.Common;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Com.BudgetMetal.ViewModels.SupplierServiceTag;
using System.Collections.Generic;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Com.EzTender.PublicPortal.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly IIndustryService industryService;
        private readonly ICompanyService companyService;
        private readonly IServiceTagsService serviceTagsService;
        private readonly IUserService userService;

        public RegistrationController(IIndustryService industryService, 
                                      ICompanyService companyService, 
                                      IServiceTagsService serviceTagsService, 
                                      IUserService userService, 
                                      IOptions<AppSettings> appSettings)
        {
            this.industryService = industryService;
            this.companyService = companyService;
            this.serviceTagsService = serviceTagsService;
            this.userService = userService;
            this._appSettings = appSettings.Value;
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(VmUserItem user)
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
                confirmationLink = url + "Registration/ConfirmUser?token=" + token + "&e=" + encodeEmail + "&t=" + encodeTimeLimit;

                mailBody = "Please confirm your account by clicking the following link \n " + confirmationLink;
                SendingMail sm = new SendingMail();
                sm.SendMail(user.EmailAddress, "", "Confirmation for Registration", mailBody);
                return RedirectToAction("SuccessRegistration");
            }
            else
            {
                return View();
            }
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
        public async Task<JsonResult> CheckUEN(string RegNo)
        {
            var result = await companyService.GetCompanyByUEN(RegNo);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetServiceTagsByCompanyId(int companyId)
        {
            List<VmSupplierServiceTag> result = await companyService.GetCompanyServiceTagById(companyId);

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
        public IActionResult SuccessRegistration()
        {
            return View();
        }
    }
}
