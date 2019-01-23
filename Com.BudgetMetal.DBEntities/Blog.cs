using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DBEntities
{
    public class Blog : GenericEntity
    {       
        

        public string Title { get; set; }
        public string Note { get; set; }
        public string Category { get; set; }
        public string RedirectLink { get; set; }
        

    }
}
