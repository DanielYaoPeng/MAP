using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using MD.ApkMAP.Common;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MD.ApkMAP
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // 生成承载 web 应用程序的 Microsoft.AspNetCore.Hosting.IWebHost。Build是WebHostBuilder最终的目的，将返回一个构造的WebHost，最终生成宿主。
           var host= CreateHostBuilder(args).Build();

            // 创建可用于解析作用域服务的新 Microsoft.Extensions.DependencyInjection.IServiceScope。 启动之前，直接加载数据库相关服务
            //using (var scope = host.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            //    try
            //    {
            //        // 从 system.IServicec提供程序获取 T 类型的服务。
            //        // 为了大家的数据安全，这里先注释掉了，大家自己先测试玩一玩吧。
            //        // 数据库连接字符串是在 Model 层的 Seed 文件夹下的 MyContext.cs 中
            //        var configuration = services.GetRequiredService<IConfiguration>();
            //        if (configuration.GetSection("AppSettings")["SeedDBEnabled"].ObjToBool())
            //        {
            //            var myContext = services.GetRequiredService<MyContext>();
            //            DBSeed.SeedAsync(myContext).Wait();
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        var logger = loggerFactory.CreateLogger<Program>();
            //        logger.LogError(e, "Error occured seeding the Database.");
            //        throw;
            //    }
            //}
            host.Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
              .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 });
    }
}
