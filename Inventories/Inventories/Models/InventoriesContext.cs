using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Inventories.Models
{
    public class InventoriesContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public InventoriesContext() : base("name=InventoriesContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }


        public System.Data.Entity.DbSet<Inventories.Models.Department> Departments { get; set; }

        public System.Data.Entity.DbSet<Inventories.Models.City> Cities { get; set; }

        public System.Data.Entity.DbSet<Inventories.Models.Company> Companies { get; set; }

        public System.Data.Entity.DbSet<Inventories.Models.User> Users { get; set; }
    }
}
