using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmCompany : ViewModelItemBase
    {
        //public Company()
        //{
        //    Document = new HashSet<Document>();
        //    InvitedSupplier = new HashSet<InvitedSupplier>();
        //    Rating = new HashSet<Rating>();
        //    SupplierServiceTags = new HashSet<SupplierServiceTags>();
        //    User = new HashSet<User>();
        //}

       
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
       

        public ICollection<VmDocument> Document { get; set; }
        public ICollection<VmInvitedSupplier> InvitedSupplier { get; set; }
        public ICollection<VmRating> Rating { get; set; }
        public ICollection<VmSupplierServiceTags> SupplierServiceTags { get; set; }
        public ICollection<VmUser> User { get; set; }
    }
}
