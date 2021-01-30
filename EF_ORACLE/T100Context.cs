using System; 
using EF_ORACLE.Model;
using EF_ORACLE.Model.T100;
using Microsoft.EntityFrameworkCore;
namespace EF_ORACLE
{
    public class T100Context : DbContext
    {
        public T100Context()
        {
        }
        //public T100Context(DbContextOptions options) : base(options)
        //{
        //    //Could not load assembly 'API_MES'.Ensure it is referenced by the startup project 'EF_ORACLE'.
        //    //确保它由启动项目的“Models”引用。查看地址 https://www.cnblogs.com/Onlooker/p/8047176.html
        //}
        //多个数据库应该使用这个构造函数，参数是上下文的集合
        public T100Context(DbContextOptions<T100Context> options) : base(options)
        {

        }

        //public MESContext() :
        //       base("OracleDbContext")
        //{ } DbContextOptions<firstContext> options
        //public DbSet<Student>  students { get; set; }
        //public DbSet<UserTable>  userTables { get; set; }
        public DbSet<DJ_T>   dJ_Ts { get; set; }
        public DbSet<IMAA_T>  iMAA_Ts { get; set; }
        public DbSet<SFBA_T>  sFBA_Ts { get; set; }
        public DbSet<IMAAL_T> iMAAL_Ts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //判断当前数据库是Oracle 需要手动添加Schema(DBA提供的数据库账号名称)
            if (this.Database.IsOracle())
            {
                modelBuilder.HasDefaultSchema("DSDATA");
            }
            //https://blog.csdn.net/xwnxwn/article/details/90142330
            // Fluent API方式 主键设置
            //modelBuilder.Entity<Product>().HasKey(t => t.ProductID);
             modelBuilder.Entity<IMAA_T>().HasKey(t => new { t.IMAA001, t.IMAAENT });//联合主键
             modelBuilder.Entity<SFBA_T>().HasKey(t => new { t.SFBA006, t.SFBADOCNO, t.SFBASEQ });//联合主键
             modelBuilder.Entity<IMAAL_T>().HasKey(t => new { t.IMAAL001, t.IMAAL002 });//联合主键
        }
    }
}
