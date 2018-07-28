using Com.BudgetMetal.DBEntities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Com.BudgetMetal.DB
{
    public class DataContext_BM : DbContext
    {
        public DataContext_BM(DbContextOptions<DataContext_BM> options) : base(options)
        { }

        public virtual DbSet<single_sign_on> single_sign_on { get; set; }
    }
}
