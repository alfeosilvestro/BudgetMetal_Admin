﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels
{
    public class VmGalleryItem : ViewModelItemBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ThumbnailImage { get; set; }
    }
}
