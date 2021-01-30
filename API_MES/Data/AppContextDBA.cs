//using API_MES.Model;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;

//namespace API_MES
//{
//    public class AppContextDBA : DbContext
//    {
//        public AppContextDBA(DbContextOptions<AppContextDBA> options) : base(options)
//        {

//        }

//        //public AppContext([NotNullAttribute] DbContextOptions options) : base(options)
//        //{
//        //} 
//        //该处定义你要映射到数据库中的表
//        //格式固定 
//        public DbSet<Student> Students { get; set; }
//        public DbSet<UserTable> UserTables { get; set; }
//        public DbSet<FQA_MIDTIME> MidTimes { get; set; }
//        public DbSet<SYSTUDEN> Systudens { get; set; }
//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            //判断当前数据库是Oracle 需要手动添加Schema(DBA提供的数据库账号名称)
//            if (this.Database.IsOracle())
//            {
//                modelBuilder.HasDefaultSchema("DMSNEW");
//            }

//            //modelBuilder.Configurations.Add(new CourseMapper()); 
//            base.OnModelCreating(modelBuilder);
//            //var mi = builder.GetType().GetMethod("ApplyConfiguration"); 未升级前只有一个ApplyConfiguration方法
//            var mi = modelBuilder.GetType().GetMethods()
//                .Single(
//                e => e.Name == "ApplyConfiguration"
//                && e.ContainsGenericParameters
//                && e.GetParameters().SingleOrDefault()?.ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));
//            //EntityFrameworkCore\Update-Database
//            //EntityFrameworkCore\Add-Migration
//        }
//        //public DbSet<Order> Orders { get; set; }

//        /// <summary>
//        /// OnModelCreating
//        /// </summary>
//        /// <param name="modelBuilder"></param>
//        //protected override void OnModelCreating(ModelBuilder modelBuilder)
//        //{
//        //    modelBuilder.HasDefaultSchema("ITEM");
//        //    //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
//        //    //modelBuilder.Conventions.Remove<DatabaseGeneratedAttributeConvention>();

//        //    modelBuilder.Entity<Order>().ToTable("ORDERS");
//        //    //自增列需要在数据库建序列+触发器配合生成
//        //    modelBuilder.Entity<Order>().HasKey(o => o.OrderId);
//        //    modelBuilder.Entity<Order>().Property(o => o.OrderId).HasColumnName("ORDERID");
//        //    modelBuilder.Entity<Order>().Property(o => o.OrderNo).HasColumnName("ORDERNO");
//        //    modelBuilder.Entity<Order>().Property(o => o.StoreId).HasColumnName("STOREID");
//        //    modelBuilder.Entity<Order>().Property(o => o.StoreOwnerId).HasColumnName("STOREOWNERID");
//        //    modelBuilder.Entity<Order>().Property(o => o.CustomerId).HasColumnName("CUSTOMERID");
//        //    modelBuilder.Entity<Order>().Property(o => o.OrderType).HasColumnName("ORDERTYPE");
//        //    modelBuilder.Entity<Order>().Property(o => o.OrderStatus).HasColumnName("ORDERSTATUS");
//        //}
//    }
//}
