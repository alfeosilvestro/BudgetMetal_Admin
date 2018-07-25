using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DB.Entities
{
    public class bm_gallery : GenericEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ThumbnailImage { get; set; }

        public string DetailImage { get; set; }

        public string DownloadableImage { get; set; }

    }
}
