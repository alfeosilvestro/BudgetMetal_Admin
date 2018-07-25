using Com.BudgetMetal.DB.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Com.BudgetMetal.DB
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public virtual DbSet<bm_gallery> bm_gallery { get; set; }

        public virtual DbSet<roles> roles { get; set; }

        public virtual DbSet<user>  user { get; set; }

        public virtual DbSet<CodeCategory> codeCategorie { get; set; }
    }
}
