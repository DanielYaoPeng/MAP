using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Engine;
using Autofac.Extensions.DependencyInjection;
using MD.ApkMAP.AOP;
using MD.ApkMAP.AuthHelper.OverWrite;
using MD.ApkMAP.AuthHelper.Policys;
using MD.ApkMAP.Common.GlobalVar;
using MD.ApkMAP.IRepository;
using MD.ApkMAP.IServices;
using MD.ApkMAP.IServices.Base;
using MD.ApkMAP.Repository;
using MD.ApkMAP.Services;
using MD.ApkMAP.Services.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace MD.ApkMAP
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private const string ApiName = "ApkMAP";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IMemoryCache>(factory =>
            {
                var cache = new MemoryCache(new MemoryCacheOptions());
                return cache;
            });

            //services.AddScoped<ICaching, ApkMapMemoryCache>();//系统缓存注入
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    Version = "V1",
                    Title = $"{ApiName} 接口文档——Netcore 3.0",
                    Description = "应用包MAP Api文档",
                    Contact = new OpenApiContact { Name = ApiName, Email = "yaopeng", Url = new Uri("https://www.mingdao.com") },
                    License = new OpenApiLicense { Name = ApiName, Url = new Uri("https://www.mingdao.com") }
                });
                c.OrderActionsBy(o => o.RelativePath);

                var xmlPath = Path.Combine(basePath, "MD.ApkMAP.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                #region 绑定token 到ConfigureServices 在swagger上显示认证信息


                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                c.OperationFilter<SecurityRequirementsOperationFilter>();

                //方案名称“Blog.Core”可自定义，上下一致即可
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}(注意两者之间是个空格)\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
                #endregion
            });

            #endregion

            #region JWT

            #region 1、 授权

            //简单版本  controller上打标签 这么写 [Authorize(Policy = "Admin")]
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
            //    options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
            //    options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));
            //    options.AddPolicy("A_S_O", policy => policy.RequireRole("Admin", "System", "Others"));
            //});

            //复杂版本

            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // 如果要数据库动态绑定，这里先留个空，后边处理器里动态赋值
            var permission = new List<PermissionItem>();

            // 角色与接口的权限要求参数
            var permissionRequirement = new PermissionRequirement(
                "/api/denied",// 拒绝授权的跳转地址（目前无用）
                permission,
                ClaimTypes.Role,//基于角色的授权
                audienceConfig["Issuer"],//发行人
                audienceConfig["Audience"],//听众
                signingCredentials,//签名凭据
                expiration: TimeSpan.FromSeconds(60 * 2)//接口的过期时间
                );

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Permissions.Name,
                         policy => policy.Requirements.Add(permissionRequirement));
            });
            #endregion


            #region 2、配置服务

            // 令牌验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = audienceConfig["Issuer"],//发行人,
                ValidAudience = audienceConfig["Audience"],//订阅人
                IssuerSigningKey = signingKey,
                //RequireSignedTokens = true,

                // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
                ValidateAudience = true,
                ValidateIssuer = true, //验证JWT身份
                ValidateIssuerSigningKey = true,
                // 是否要求Token的Claims中必须包含 Expires
                RequireExpirationTime = true,
                // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                ValidateLifetime = true,
                //注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
                ClockSkew = TimeSpan.Zero,
            };


            services.AddAuthentication("Bearer")
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = tokenValidationParameters;
                    o.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            // 如果过期，则把<是否过期>添加到，返回头信息中
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            // 注入权限处理器

            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton(permissionRequirement);

            #endregion

            #endregion
            

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            /*
             * 3.0把接口的注入放到这里了，
            ConfigureServices 不能是返回类型了，只能是 void 方法，那我们就不用 return 出去了，
            官方给我们提供了一个服务提供上工厂，我们从这个工厂里拿，而不是将服务配置 return 出去
            */
            //var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;

            //// builder.RegisterType<ApkMapMemoryCache>().As<ICaching>().InstancePerDependency();

            //var servicesDllFile = Path.Combine(basePath, "MD.ApkMAP.Services.dll");
            //var assemblysServices = Assembly.LoadFrom(servicesDllFile);//直接采用加载文件的方法  ※※★※※ 如果你是第一次下载项目，请先F6编译，然后再F5执行，※※★※※

            //var repositoryDllFile = Path.Combine(basePath, "MD.ApkMAP.Repository.dll");
            //var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);

            
            //Autofac.Engine.EngineContext.Initialize();

            //builder.RegisterAssemblyTypes(assemblysServices)
            //              .AsImplementedInterfaces()
            //              .InstancePerLifetimeScope();

            //builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();



            //***第二种注册方式，单个一一注册
            builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>().InstancePerDependency();
            builder.RegisterType<AdvertisementRepository>().As<IAdvertisementRepository>().InstancePerDependency();
            builder.RegisterType<SysUserInfoServices>().As<ISysUserInfoServices>().InstancePerDependency();
            builder.RegisterType<RoleModulePermissionServices>().As<IRoleModulePermissionServices>().InstancePerDependency();

            /*注入生命周期
             *   InstancePerDependency() 每个依赖一个实例。(每次都会返回一个新的实例，并且这是默认的生命周期。)
             *   SingleInstance() 单例，所有服务请求都将会返回同一个实例。(无论您请求何处，都始终获得相同的实例。)
             *   InstancePerLifetimeScope 每个匹配的生命周期作用域一个实例(在解决每个生命周期实例作用域组件时，每个嵌套作用域将获得一个实例（例如，每个工作单元）)
             */

            //builder.RegisterType<BaseServices>().As<IBaseServices>().InstancePerDependency();


            //将services填充到Autofac容器生成器中
            //containerBuilder.Populate(services);

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 使用静态文件
            app.UseStaticFiles();

            app.UseRouting();//路由中间件
                             // 短路中间件，配置Controller路由

            // 使用cookie
            app.UseCookiePolicy();
            // 返回错误码
            app.UseStatusCodePages();//把错误码返回前台，比如是404

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/V1/swagger.json", $"{ApiName} V1");

                //路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，去launchSettings.json把launchUrl去掉，如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "doc";
                c.RoutePrefix = "";
            });

            // app.UseMiniProfiler();






            // app.UseStaticFiles();//用于访问wwwroot下的文件 
        }
    }
}
