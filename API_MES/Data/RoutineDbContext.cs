using System;
using API_MES.Model;
using Microsoft.EntityFrameworkCore;

namespace API_MES.Data
{
    public class RoutineDbContext : DbContext
    {
        public RoutineDbContext(DbContextOptions<RoutineDbContext> options) : base(options)
        {

        }
       // public DbSet<Company> Companies { get; set; }
       // public DbSet<Employee> Employees { get; set; }
        public DbSet<IMGFIRST> IMGFIRSTs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
