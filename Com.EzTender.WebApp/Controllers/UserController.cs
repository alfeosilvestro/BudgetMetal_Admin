using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Services.Users;
using Com.BudgetMetal.ViewModels.Sys_User;
using Com.BudgetMetal.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Com.GenericPlatform.WebApp.Controllers
{
    public class UserController : Controller
    {
      


        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        // GET: User
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(VM_Sys_User_Sign_In user)
        {
            try
            {
                var result = userService.ValidateUser(user);
                var resultObj = result.Result;
                if (resultObj.Id == 0)
                {
                    ViewBag.ErrorMessage = "Email or Password is invalid!";
                    return View(user);
                }
                else
                {
                    // TODO: Add insert logic here
                    
                    HttpContext.Session.SetString("User_Id", resultObj.Id.ToString());
                    HttpContext.Session.SetString("EmailAddress", resultObj.EmailAddress.ToString());
                    HttpContext.Session.SetString("Company_Id", resultObj.Company_Id.ToString());
                    HttpContext.Session.SetString("UserType", resultObj.UserType.ToString());
                    HttpContext.Session.SetString("ContactName", resultObj.ContactName.ToString());
                    HttpContext.Session.SetString("UserName", resultObj.UserName.ToString());

                    return RedirectToAction("Index", "Home");
                }

            }
            catch
            {
                return View();
            }
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }


    }
}