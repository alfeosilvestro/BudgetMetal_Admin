﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Common.FoundationClasses;

namespace Com.EazyTender.WebApp.Configurations
{
    public class AppSettings : BaseSettings
    {
        public int TotalRecordPerPage
        {
            get;
            set;
        }

        public int FirstPage
        {
            get;
            set;
        }
    }
}
