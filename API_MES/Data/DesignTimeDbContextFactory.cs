//using API_MES.Mappers;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//namespace API_MES
//{
//    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppContextDBA>
//    {
//        //ApplicationDbContext 代表的是你的创建失败的那个类型
//        public AppContextDBA CreateDbContext(string[] args)
//        {
//            IConfigurationRoot configuration = new ConfigurationBuilder()
//            .SetBasePath(Directory.GetCurrentDirectory())
//            .AddJsonFile("appsettings.json")
//            .Build();
//            var builder = new DbContextOptionsBuilder();
//            var UrlOracle = configuration.GetConnectionString("OracleConnection");
//            //var Ora = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=172.17.5.235)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=orcl)));User Id=dmsnew;Password=chpass";
//            //builder.UseOracle(Ora);
//            //return new AppContext(builder.Options);
//            var optionsBuilder = new DbContextOptionsBuilder<AppContextDBA>();
//            optionsBuilder.UseOracle(Md5Encrypt.Decrypt(UrlOracle));
//            return new AppContextDBA(optionsBuilder.Options);
//        }
//    }
//}
