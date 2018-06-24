using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.Common
{
    public class PageResult<T> where T : class
    {
        public int TotalRecords { get; set; }

        public int TotalPage { get; set; }

        public int CurrentPage { get; set; }

        public List<T> Records { get; set; }
    }
}
