using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Gallery.Models
{
    public class PageResult
    {
        public int TotalRecords { get; set; }

        public int TotalPage { get; set; }

        public int CurrentPage { get; set; }

        public List<bm_gallery> Records { get; set; }
    }


    public class ResponseBase
    {
        public PageResult ResultObject { get; set; }
    }
}
