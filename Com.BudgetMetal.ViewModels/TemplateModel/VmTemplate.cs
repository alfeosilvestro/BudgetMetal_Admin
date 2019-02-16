using Com.BudgetMetal.ViewModels.Requirement;
using Com.BudgetMetal.ViewModels.RfqPenalty;
using Com.BudgetMetal.ViewModels.RfqPriceSchedule;
using Com.BudgetMetal.ViewModels.Sla;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.TemplateModel
{
    public class VmTemplate
    {
        public virtual VmGenericServiceResult result { get; set; }

        public List<VmRequirementItem> List_Requirement { get; set; }

        public List<VmSlaItem> List_SLA { get; set; }

        public List<VmPenaltyItem> List_Panalty { get; set; }

        public List<VmRfqPriceScheduleItem> List_Pricing { get; set; }

        public List<VmRfqPriceScheduleItem> List_Service_Pricing { get; set; }

        public List<VmRfqPriceScheduleItem> List_Waranty_Pricing { get; set; }
    }
}
