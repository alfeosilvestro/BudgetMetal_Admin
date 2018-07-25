﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DB.Entities
{
    [Table("code_category")]
    public class CodeCategory : GenericEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
