using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetMetal_Admin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BudgetMetal_Admin.DB
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public virtual DbSet<bm_gallery> bm_gallery { get; set; }




    }
}
