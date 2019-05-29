using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MD.ApkMAP.Common;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MD.ApkMAP
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // 生成承载 web 应用程序的 Microsoft.AspNetCore.Hosting.IWebHost。Build是WebHostBuilder最终的目的，将返回一个构造的WebHost，最终生成宿主。
           var host=  CreateWebHostBuilder(args).Build();

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


        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            //使用预配置的默认值初始化 Microsoft.AspNetCore.Hosting.WebHostBuilder 类的新实例。
            WebHost.CreateDefaultBuilder(args)
                //指定要由 web 主机使用的启动类型。相当于注册了一个IStartup服务。可以自定义启动服务，比如.UseStartup(typeof(StartupDevelopment).GetTypeInfo().Assembly.FullName)
                //.UseUrls("http://localhost:5012")
                .UseStartup<Startup>();

        //public static void Main(string[] args)
        //{
        //    BuildWebHost(args).Run();
        //}

        //public static IWebHost BuildWebHost(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>()
        //        .Build();
    }
}
