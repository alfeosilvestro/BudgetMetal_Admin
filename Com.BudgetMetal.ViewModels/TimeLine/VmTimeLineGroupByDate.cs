using Com.BudgetMetal.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.TimeLine
{
    public class VmTimeLineGroupByDate
    {
       public DateTime GroupDate { get; set; }

       public List<VmTimeLineItemForPage> Records { get; set; }
        
    }
}
