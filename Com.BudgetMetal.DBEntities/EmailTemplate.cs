using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DBEntities
{
    public class EmailTemplate : GenericEntity
    {       
        public string Purpose { get; set; }
        public string EmailSubject { get; set; }
        public string EmailContent { get; set; }
        
    }
}
