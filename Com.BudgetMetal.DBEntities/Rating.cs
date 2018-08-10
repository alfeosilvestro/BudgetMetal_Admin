using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class Rating : GenericEntity
    {
       
        
        public int? SpeedOfQuotation { get; set; }
        public int? SpeedofDelivery { get; set; }
        public int? ServiceQuality { get; set; }
        public int? Price { get; set; }
        public int? SpeedofProcessing { get; set; }
        public int? Payment { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Ratingcol { get; set; }

        [ForeignKey("Company")]
        public int Company_Id { get; set; }
        public virtual Company Company { get; set; }

        [ForeignKey("Document")]
        public int Document_Id { get; set; }
        public virtual Document Document { get; set; }

        [ForeignKey("User")]
        public int User_Id { get; set; }
        public virtual User User { get; set; }
    }
}
