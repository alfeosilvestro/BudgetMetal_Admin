using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.BudgetMetal.Common.FoundationClasses;

namespace Com.BudgetMetal.App.Configurations
{
    public class AppSettings : BaseSettings
    {
        public string APIURL
        {
            get;
            set;
        }

        public string MainSiteURL
        {
            get;
            set;
        }

        public string DefaultUEN
        {
            get;
            set;
        }
        
            public string DefaultCreator
        {
            get;
            set;
        }
    }
}
