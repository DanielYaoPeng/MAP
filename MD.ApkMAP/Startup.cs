using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MD.ApkMAP.AOP;
using MD.ApkMAP.AuthHelper.OverWrite;
using MD.ApkMAP.IRepository;
using MD.ApkMAP.IServices;
using MD.ApkMAP.Repository;
using MD.ApkMAP.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.AddScoped<ICaching, ApkMapMemoryCache>();//系统缓存注入

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v0.1.0",
                    Title = "MD.ApkMAP API",
                    Description = "应用包MAP Api文档",
                    TermsOfService = "None",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact { Name = "ApkMAP", Url = "https://www.mingdao.com" }
                });

                //就是这里

                var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "MD.ApkMAP.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                #region 绑定token 到ConfigureServices 在swagger上显示认证信息
                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "MD.ApkMAP", new string[] { } }, };
                c.AddSecurityRequirement(security);
                //方案名称“Blog.Core”可自定义，上下一致即可
                c.AddSecurityDefinition("MD.ApkMAP", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}(注意两者之间是个空格)\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
                #endregion
            });

            #endregion

            #region JWT
            //无身份注册
            //services.AddAuthentication().AddJwtBearer(jwtOptions =>
            //{
            //    jwtOptions.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtHelper.secretKey)),
            //        //ValidAudience = "1",
            //        //ValidIssuer = "1",
            //        ValidateLifetime = true
            //    };
            //});


            #region 认证，第二种验证方法

            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
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
                });
            #endregion


            #region token服务注册
            services.AddSingleton<IMemoryCache>(factory =>
            {
                var cache = new MemoryCache(new MemoryCacheOptions());
                return cache;
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("AdminOrClient", policy => policy.RequireRole("Admin,Client").Build());
            });
            #endregion
            #endregion

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<ApkMapMemoryCache>().As<ICaching>().InstancePerDependency();
            containerBuilder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>().InstancePerDependency();
            containerBuilder.RegisterType<AdvertisementRepository>().As<IAdvertisementRepository>().InstancePerDependency();
            //将services填充到Autofac容器生成器中
            containerBuilder.Populate(services);

            //使用已进行的组件登记创建新容器
            var ApplicationContainer = containerBuilder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
            });
            #endregion

            // app.UseMiddleware<JwtTokenAuth>();//注意此授权方法已经放弃，请使用下边的官方授权方法。这里仅仅是授权方法的替换
            app.UseAuthentication();
            //app.UseJwtBearerAuthentication(new JwtBearerOptions
            //{
            //    TokenValidationParameters = new TokenValidationParameters
            //    {
            //        IssuerSigningKey = _tokenOptions.Key,
            //        ValidAudience = _tokenOptions.Audience, // 设置接收者必须是 TestAudience
            //        ValidIssuer = _tokenOptions.Issuer, // 设置签发者必须是 TestIssuer
            //        ValidateLifetime = true
            //    }
            //});
            app.UseMvc();

            // app.UseStaticFiles();//用于访问wwwroot下的文件 
        }
    }
}
