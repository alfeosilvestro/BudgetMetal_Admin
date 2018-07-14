using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.Common.FoundationClasses
{
    public class BaseSettings
    {
        public AppIdentity App_Identity { get; set; }
        public DbSettings DB_Settings { get; set; }
    }
}
