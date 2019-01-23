using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.BudgetMetal.ViewModels.Blogs
{
    public class VmBlogItem : ViewModelItemBase
    {
        
        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Note is required!")]
        public string Note { get; set; }

        public string Category { get; set; }

        public string RedirectLink { get; set; }
    }
}
