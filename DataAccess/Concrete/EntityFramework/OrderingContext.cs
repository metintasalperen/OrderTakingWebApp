using System;
using System.Collections.Generic;
using System.Text;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class OrderingContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb; Database=Ordering; Trusted_Connection=true");
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Table> Tables { get; set; }
    }
}
