using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DB.Entities
{
    public class Rating : GenericEntity
    {
       
        public uint CompanyId { get; set; }
        public uint UserId { get; set; }
        public uint DocumentId { get; set; }
        public int? SpeedOfQuotation { get; set; }
        public int? SpeedofDelivery { get; set; }
        public int? ServiceQuality { get; set; }
        public int? Price { get; set; }
        public int? SpeedofProcessing { get; set; }
        public int? Payment { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Ratingcol { get; set; }

        public Company Company { get; set; }
        public Document Document { get; set; }
        public User User { get; set; }
    }
}
