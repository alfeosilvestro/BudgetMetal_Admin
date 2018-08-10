using Com.BudgetMetal.DBEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;


namespace Com.BudgetMetal.DB
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        //public virtual DbSet<bm_gallery> bm_gallery { get; set; }

        //public virtual DbSet<roles> roles { get; set; }

        //public virtual DbSet<user>  user { get; set; }

        //public virtual DbSet<CodeCategory> codeCategorie { get; set; }

        public virtual DbSet<Attachment> Attachment { get; set; }

        public virtual DbSet<bm_gallery> bm_gallery { get; set; }

        public virtual DbSet<Clarification> Clarification { get; set; }
        public virtual DbSet<CodeCategory> CodeCategory { get; set; }
        public virtual DbSet<CodeTable> CodeTable { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<DocumentUser> DocumentUser { get; set; }
        public virtual DbSet<EmailLog> EmailLog { get; set; }
        public virtual DbSet<Industry> Industry { get; set; }
        public virtual DbSet<InvitedSupplier> InvitedSupplier { get; set; }
        public virtual DbSet<Penalty> Penalty { get; set; }
        public virtual DbSet<Quotation> Quotation { get; set; }
        public virtual DbSet<QuotationPriceSchedule> QuotationPriceSchedule { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }
        public virtual DbSet<Requirement> Requirement { get; set; }
        public virtual DbSet<Rfq> Rfq { get; set; }
        public virtual DbSet<RfqPriceSchedule> RfqPriceSchedule { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<ServiceTags> ServiceTags { get; set; }
        public virtual DbSet<Sla> Sla { get; set; }
        public virtual DbSet<SupplierServiceTags> SupplierServiceTags { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
    }
}
