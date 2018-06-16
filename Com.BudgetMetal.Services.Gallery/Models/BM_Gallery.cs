using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Gallery.Models
{
    public class bm_gallery
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ThumbnailImage { get; set; }

        public string DetailImage { get; set; }

        public string DownloadableImage { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsActive { get; set; }

        public string Version { get; set; }
    }
}
