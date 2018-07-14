using System;

namespace Com.BudgetMetal.ViewModels
{
    public class ViewModelBase
    {
        public string RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public string ApplicationToken { get; set; }
        public string AuthenticationToken { get; set; }
    }
}
