using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmCodeCategory : ViewModelItemBase
    {
        //public CodeCategory()
        //{
        //    CodeTable = new HashSet<CodeTable>();
        //}

        
        public string Name { get; set; }
       

        public ICollection<VmCodeTable> CodeTable { get; set; }
    }
}


//namespace Com.BudgetMetal.ViewModels.EzyTender
//{
//    [Table("code_category")]
//    public class CodeCategory : ViewModelItemBase
//    {
//        public string Name { get; set; }

//        public string Description { get; set; }
//    }
//}
