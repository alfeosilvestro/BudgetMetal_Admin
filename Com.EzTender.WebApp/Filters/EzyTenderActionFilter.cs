


using Com.BudgetMetal.Common;
using Com.BudgetMetal.ViewModels.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.EzTender.WebApp.Filters
{
    public class EzyTenderActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.RouteValues["controller"];
            string actionName = filterContext.ActionDescriptor.RouteValues["action"];
            string controller_action = controllerName + "_" + actionName;


            var vrSession = filterContext.HttpContext.Session;


            byte[] b_UserId;
            vrSession.TryGetValue("User_Id", out b_UserId);
            if(actionName.ToLower() == "signin") { } else { 
            if (b_UserId == null)
            {
                // Redirect to login page action
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"controller", "User"},
                    {"action", "SignIn"}
                });
            }
            else
            {
                string userId = (b_UserId == null) ? "0" : System.Text.Encoding.Default.GetString(b_UserId);

                byte[] b_Roles;
                vrSession.TryGetValue("SelectedRoles", out b_Roles);
                bool isAccess = false;
                if (b_Roles != null)
                {
                    string strRole = System.Text.Encoding.Default.GetString(b_Roles);

                    var userRoles = JsonConvert.DeserializeObject<List<VmRoleItem>>(strRole);

                    Constants_RolePermission c_RolePermission = new Constants_RolePermission();
                    var rolePermissionMap = c_RolePermission.getRolePermissionMap();
                    foreach (var item in userRoles)
                    {
                        string currentRoleAction = item.Code + "_" + controller_action;
                        if (rolePermissionMap.ContainsKey(currentRoleAction))
                        {
                            var roleAccess = rolePermissionMap[currentRoleAction];
                            if (Convert.ToBoolean(roleAccess))
                            {
                                isAccess = true;
                                break;
                            }
                        }
                    }
                }
                if (isAccess)
                {

                }
                else
                {
                    // Redirect to controller action
                    //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    //{
                    //    {"controller", "Home"},
                    //    {"action", "ErrorForUser"},
                    //    {"page", "denied"}
                    //});
                }
                // Redirect to external url
                //filterContext.Result = new RedirectResult(url);
            }
            }
        }
    }
}
