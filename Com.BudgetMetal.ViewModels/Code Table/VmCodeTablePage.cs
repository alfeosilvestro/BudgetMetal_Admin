﻿using Com.BudgetMetal.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.Code_Table
{
    public class VmCodeTablePage : ViewModelBase
    {
        public PageResult<VmCodeTableItem> Result { get; set; }
    }
}
