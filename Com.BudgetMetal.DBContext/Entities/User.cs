﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.DB.Entities
{
    public class user : GenericEntity
    {
        public int UserTypeId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public int RoleId { get; set; }

        public string SiteAdmin { get; set; }

        public string Title { get; set; }

        public bool Status { get; set; }

        public bool Confirmed { get; set; }
    }
}