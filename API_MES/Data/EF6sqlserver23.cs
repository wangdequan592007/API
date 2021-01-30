using System;
using API_MES.Model; 
using Microsoft.EntityFrameworkCore;
namespace API_MES.Data
{
    public class EF6sqlserver23 : DbContext
    {
        public EF6sqlserver23(DbContextOptions<EF6sqlserver23> options) : base(options)
        {

        }
        public DbSet<Employee23>  employee23s { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
