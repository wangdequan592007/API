using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EF_SQL
{
    public class SQLContextFactory : IDesignTimeDbContextFactory<SqlDbContext>
    {
        //ApplicationDbContext 代表的是你的创建失败的那个类型
        public SqlDbContext CreateDbContext(string[] args)
        {

            //IConfigurationRoot configuration = new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile("appsettings.json")
            //.Build();
            var builder = new DbContextOptionsBuilder();
            //var Sqlcontext = configuration.GetConnectionString("OracleDba");
            var Sqlcontext = "server=192.168.12.105;database=WebTestDBA; uid=kings;pwd=kings;";
            //var Ora = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=172.17.5.235)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=orcl)));User Id=dmsnew;Password=chpass";
            //builder.UseOracle(Ora);
            //return new AppContext(builder.Options);
            var optionsBuilder = new DbContextOptionsBuilder<SqlDbContext>();
            optionsBuilder.UseSqlServer(Sqlcontext);
            return new SqlDbContext(optionsBuilder.Options);
        }
    } 
}
