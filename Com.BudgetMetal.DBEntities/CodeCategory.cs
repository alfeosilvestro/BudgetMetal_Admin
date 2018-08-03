﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DBEntities
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
