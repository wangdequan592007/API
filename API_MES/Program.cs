using API_MES.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;

namespace API_MES
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();//https://www.w3xue.com/exp/article/201910/61708.html
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                
                var services = scope.ServiceProvider;
                try
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogInformation( "开始运行api"); 
                }
                catch (Exception ex)
                {
                    //Do something
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, message: "失败");
                }
            }
            //using (var serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            //{
            //    var dbContext = serviceScope.ServiceProvider.GetService<RoutineDbContext>();
            //    dbContext.Database.EnsureDeleted();
            //    dbContext.Database.MigrateAsync();
            //    //init Database,you can add your init data here
            //    //var userManager = serviceScope.ServiceProvider.GetService<UserManager<RoutineDbContext>>();

            //    //var email = "weihanli@outlook.com";
            //    //if (userManager.FindByEmailAsync(email).GetAwaiter().GetResult() == null)
            //    //{
            //    //    userManager.CreateAsync(new UserAccount
            //    //    {
            //    //        UserName = "weihanli@outlook.com",
            //    //        Email = "weihanli@outlook.com"
            //    //    }, "Test1234").Wait();
            //    //}
            //}
            host.Run();
            //BuildWebHost(args).Run();
        }
        //设置MaxRequestBodySize属性为null，表示服务器不限制Http请求提交的最大数据量，其默认值为30000000（字节），也就是大约28.6MB
        //public static IWebHost BuildWebHost(string[] args) =>
        //WebHost.CreateDefaultBuilder(args)
        //    .UseStartup<Startup>()
        //    .UseHttpSys(options =>
        //    {
        //        options.MaxRequestBodySize = null;
        //    }) 
        //    .Build();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseNLog();
    }
}
