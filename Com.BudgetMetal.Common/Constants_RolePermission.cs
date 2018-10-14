using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.Common
{
    public class Constants_RolePermission
    {
        //public static Dictionary<string, bool> role_permission_map;

        public Constants_RolePermission()
        {
            //role_permission_map = new Dictionary<string, bool>();
            
        }

        public Dictionary<string, bool> getRolePermissionMap()
        {
            Dictionary<string, bool> role_permission_map = new Dictionary<string, bool>();
            role_permission_map.Add("C_Admin_Home_Index", true);
            role_permission_map.Add("C_Admin_Rfq_Index", true);
            role_permission_map.Add("C_Admin_Rfq_View", true);
            role_permission_map.Add("C_Admin_Rfq_Edit", true);
            role_permission_map.Add("C_Admin_Rfq_Create", true);

            return role_permission_map;
        }


    }
}
