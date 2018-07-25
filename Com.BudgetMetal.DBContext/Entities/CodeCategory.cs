using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DB.Entities
{
    public class CodeCategory : GenericEntity
    {
        public CodeCategory()
        {
            CodeTable = new HashSet<CodeTable>();
        }

        
        public string Name { get; set; }
       

        public ICollection<CodeTable> CodeTable { get; set; }
    }
}
