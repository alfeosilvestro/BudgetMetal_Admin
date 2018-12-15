using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Services.Users;
using Com.BudgetMetal.ViewModels.Sys_User;
using Com.BudgetMetal.ViewModels.User;
using Com.EazyTender.WebApp.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Com.EzTender.WebApp.Filters;
using Newtonsoft.Json;

namespace Com.GenericPlatform.WebApp.Controllers
{
    [EzyTenderActionFilter]
    public class UserController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly IUserService userService;
        public UserController(IUserService userService, IOptions<AppSettings> appSettings)
        {
            this.userService = userService;
            this._appSettings = appSettings.Value;
        }

        // GET: User
        public ActionResult SignOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn", "User");
        }

        // GET: User
        
        public ActionResult SignIn()
        {
            TempData["ForgotPasswordUrl"] = _appSettings.App_Identity.WebAppUrl + "Public/ForgotPassword";
            TempData["RegisterUrl"] = _appSettings.App_Identity.WebAppUrl + "Public/Registration";
            TempData["PublicSiteUrl"] = _appSettings.App_Identity.PublicSiteUrl;

            var user_Id = HttpContext.Session.GetString("User_Id");
            if (user_Id != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(VM_Sys_User_Sign_In user)
        {
            try
            {
                TempData["ForgotPasswordUrl"] = _appSettings.App_Identity.WebAppUrl + "Public/ForgotPassword";
                TempData["RegisterUrl"] = _appSettings.App_Identity.WebAppUrl + "Public/Registration";
                TempData["PublicSiteUrl"] = _appSettings.App_Identity.PublicSiteUrl;

                var result = userService.ValidateUser(user);

                var resultObj = result.Result;
                if (resultObj.Id == 0)
                {
                    ViewBag.ErrorMessage = "Email or Password is invalid!";
                    return View(user);
                }
                else
                {
                    if (resultObj.IsActive && resultObj.IsConfirmed)
                    {
                        // TODO: Add insert logic here
                        HttpContext.Session.SetString("User_Id", resultObj.Id.ToString());
                        HttpContext.Session.SetString("EmailAddress", resultObj.EmailAddress.ToString());
                        HttpContext.Session.SetString("Company_Id", resultObj.Company_Id.ToString());
                        HttpContext.Session.SetString("UserType", resultObj.UserType.ToString());
                        HttpContext.Session.SetString("ContactName", resultObj.ContactName.ToString());
                        HttpContext.Session.SetString("UserName", resultObj.UserName.ToString());
                        HttpContext.Session.SetString("C_BusinessType", resultObj.Company.C_BusinessType.ToString());
                        string strSelectedRoles = JsonConvert.SerializeObject(resultObj.SelectedRoles);
                        HttpContext.Session.SetString("SelectedRoles", strSelectedRoles.ToString());
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Email not confiremed";
                        return View(user);
                    }
                    return RedirectToAction("Index", "Home");
                }

            }
            catch
            {
                return View();
            }
        }


        // GET: User
        public ActionResult Register()
        {
            return View();
        }

        // GET: User
        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> ChangesPassword(string Password)
        {
            var User_Id = HttpContext.Session.GetString("User_Id");

            var result = await userService.ChangePassword(int.Parse(User_Id), Password);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet]
        public async Task<JsonResult> CheckCurrentPassword(string CurrentPassword)
        {
            var User_Id = HttpContext.Session.GetString("User_Id");
            
            var result = await userService.CheckCurrentPassword(int.Parse(User_Id), CurrentPassword);

            return new JsonResult(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }


    }
}