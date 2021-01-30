
using EF_ORACLE.Model;
using EF_ORACLE.Model.TMES;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;
namespace EF_ORACLE
{
    #region snippet_Constructor
    public class MESContext : DbContext
    {
        public MESContext()
        {
        }
        public MESContext(DbContextOptions<MESContext> options) : base(options)
        {

        }
        //public MESContext(DbContextOptions options) : base(options)
        //{
        //    //Could not load assembly 'API_MES'.Ensure it is referenced by the startup project 'EF_ORACLE'.
        //    //确保它由启动项目的“Models”引用。查看地址 https://www.cnblogs.com/Onlooker/p/8047176.html
        //}
        //多个数据库应该使用这个构造函数，参数是上下文的集合 
        //public MESContext() :
        //       base("OracleDbContext")
        //{ } 
        // public DbSet<Student> Students { get; set; }
        // public DbSet<UserTable> UserTables { get; set; }
        public DbSet<FQA_MIDTIME> MidTimes { get; set; }
        public DbSet<LOCKMO>  lOCKMOs { get; set; }
        public DbSet<FQA_FIRSTARTICLEIMG>  fQA_FIRSTARTICLEIMGs { get; set; }
        public DbSet<ModelMO> ModelMOs { get; set; }
        public DbSet<FQA_FIRSTARTICLE>  fQA_FIRSTARTICLEs { get; set; }
        public DbSet<FQA_FIRSTARTICLELINE>  fQA_FIRSTARTICLELINEs { get; set; }
        public DbSet<FQA_FIRSTARTICLELINELOG>  fQA_FIRSTARTICLELINELOGs { get; set; }
        public DbSet<WORKPRODUCE>  wORKPRODUCEs { get; set; }
        //public DbSet<ODM_IMEILINKNETCODE> oDM_IMEILINKNETCODEs { get; set; }
        public DbSet<BARCODEREMP>  bARCODEREMPs { get; set; }
        public DbSet<EDI_SSCCINFOLOG> EDI_SSCCINFOLOG { get; set; }
        public DbSet<ODM_MODEL> ODM_MODEL { get; set; }
        public DbSet<ODM_TEMPERATUREBOARD> ODM_TEMPERATUREBOARD { get; set; }
        public DbSet<ODM_TEMPERATUREBOARD_DTL> ODM_TEMPERATUREBOARD_DTL { get; set; }
        public DbSet<FQA_LINETIMEOW> FQA_LINETIMEOW { get; set; }
        public DbSet<FQA_CK_SNLOG> FQA_CK_SNLOG { get; set; }
        public DbSet<FQA_CK_SN> FQA_CK_SN{ get; set; }
        public DbSet<ODM_SIDKEY> ODM_SIDKEY { get; set; }
        public DbSet<ODM_SIDKEYTEMP> ODM_SIDKEYTEMP { get; set; }
        public DbSet<ODM_SIDKEYLOG> ODM_SIDKEYLOG { get; set; }
        public DbSet<ODM_LOCKREWORK> ODM_LOCKREWORK { get; set; }
        public DbSet<MESTOOL> MESTOOL { get; set; }
        public DbSet<MTL_SUB_ATTEMPER> MTL_SUB_ATTEMPER { get; set; }
        public DbSet<FQA_OBAITEMTIME> FQA_OBAITEMTIME { get; set; } 
        public DbSet<FQA_OBARESHUOTIME> FQA_OBARESHUOTIME { get; set; }
        public DbSet<ODM_BEATE> ODM_BEATE { get; set; }
        public DbSet<RE_MODEL> RE_MODEL { get; set; }
        public DbSet<RE_BUG_TYPE> RE_BUG_TYPE { get; set; }
        public DbSet<QA_CLINT> QA_CLINT { get; set; }
        public DbSet<ODM_PRESSURETIME>  oDM_PRESSURETIMEs { get; set; }

        public DbSet<ODM_TESTFAILNUM>  oDM_TESTFAILNUMs { get; set; }
        //RE_BUG_TYPE QA_CLINT ODM_PRESSURETIME
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    IConfigurationRoot configuration = new ConfigurationBuilder()
        //    .SetBasePath(Directory.GetCurrentDirectory())
        //    .AddJsonFile("appsettings.json")
        //    .Build();
        //    var builder = new DbContextOptionsBuilder();
        //    var UrlOracle = configuration.GetConnectionString("OracleDba");
        //    optionsBuilder.UseOracle(Md5Encrypt.Decrypt(UrlOracle));
        //    base.OnConfiguring(optionsBuilder);
        //}
        #endregion 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //判断当前数据库是Oracle 需要手动添加Schema(DBA提供的数据库账号名称)
            if (this.Database.IsOracle())
            {
                modelBuilder.HasDefaultSchema("DMSNEW");
            }
            //https://blog.csdn.net/xwnxwn/article/details/90142330
            // Fluent API方式 主键设置
            //modelBuilder.Entity<Product>().HasKey(t => t.ProductID);
            modelBuilder.Entity<FQA_FIRSTARTICLE>().HasKey(t => new { t.MO,t.MTYPE });//联合主键
            modelBuilder.Entity<FQA_FIRSTARTICLELINE>().HasKey(t => new { t.MO,t.MTYPE,t.LINE});//联合主键
            modelBuilder.Entity<FQA_FIRSTARTICLELINELOG>().HasKey(t => new { t.MO, t.MTYPE, t.LINE,t.CREAT_DT });//联合主键
            modelBuilder.Entity<LOCKMO>().HasKey(t => new { t.MO,t.LINE,t.CRT_DATE });//联合主键
            modelBuilder.Entity<EDI_SSCCINFOLOG>().HasKey(t => new { t.SSCC, t.CRT_DATE });//联合主键
            modelBuilder.Entity<MTL_SUB_ATTEMPER>().HasKey(t => new { t.ATTEMPTER_CODE, t.SERIAL_NUMBER });//联合主键
            //modelBuilder.Entity<RE_BUG_TYPE>().HasKey(t => new { t.CODE, t.CODE2 });//联合主键
            //modelBuilder.Entity<ODM_IMEILINKNETCODE>().HasIndex(b => b.PHYSICSNO).HasName("P_PHYSICSNO_NETCODE_NEW");
            //modelBuilder.Entity<ODM_IMEILINKNETCODE>().HasIndex(b => b.SN).HasName("P_SNNETCODE");
            //modelBuilder.Entity<ODM_IMEILINKNETCODE>().HasIndex(b => b.WORKORDER).HasName("P_WORKORDERIMEI");
            //modelBuilder.Entity<ODM_IMEILINKNETCODE>().HasIndex(b => b.MEID).HasName("P_MEID_IDX");
            //modelBuilder.Entity<ODM_IMEILINKNETCODE>().HasIndex(b => b.IMEI2).HasName("P_IMEI2_IDX");
        }
    }
    public class Md5Encrypt
    {
        public static string Decrypt(string Text, string sKey = "enok")
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey).Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey).Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        private static string Md5Hash(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}