using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DB.Entities
{
    public class Company : GenericEntity
    {
        public Company()
        {
            Document = new HashSet<Document>();
            InvitedSupplier = new HashSet<InvitedSupplier>();
            Rating = new HashSet<Rating>();
            SupplierServiceTags = new HashSet<SupplierServiceTags>();
            User = new HashSet<User>();
        }

       
        public string Name { get; set; }
        public string Address { get; set; }
        public string Domain { get; set; }
        public string RegNo { get; set; }
        public bool? IsVerified { get; set; }
        public string About { get; set; }
        public decimal? SupplierAvgRating { get; set; }
        public decimal? BuyerAvgRating { get; set; }
        public int? AwardedQuotation { get; set; }
        public int? SubmittedQuotation { get; set; }
       

        public ICollection<Document> Document { get; set; }
        public ICollection<InvitedSupplier> InvitedSupplier { get; set; }
        public ICollection<Rating> Rating { get; set; }
        public ICollection<SupplierServiceTags> SupplierServiceTags { get; set; }
        public ICollection<User> User { get; set; }
    }
}
