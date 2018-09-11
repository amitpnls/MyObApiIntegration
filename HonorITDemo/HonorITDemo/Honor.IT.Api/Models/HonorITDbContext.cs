using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Honor.IT.Api.Models
{
    public partial class HonorITDbContext : DbContext
    {
        public HonorITDbContext() : base("name=HonorITDbContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //DONT DO THIS ANYMORE
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Vote>().ToTable("Votes")
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Designation> Designations { get; set; }
    }
}
