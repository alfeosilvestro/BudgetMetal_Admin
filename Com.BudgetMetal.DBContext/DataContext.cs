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
    }
}
