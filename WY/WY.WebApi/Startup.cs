using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using SqlSugar.IOC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WY.Data;
using WY.Data.Utility._AutoMapper;
using WY.Data.Utility.Caching;
using WY.IService;
using WY.Service;

namespace WY.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddControllersAsServices();

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WY.WebApi", Version = "v1" });
                #region swagger使用鉴权组件
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "直接在下框中输入Bearer {token}(注意两者之间是一个空格)",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });
                #endregion
            });
            #endregion
            #region SqlSugar IOC
            services.AddSqlSugar(new IocConfig()
            {
                ConnectionString = this.Configuration["SqlConn"],
                DbType = IocDbType.MySql,
                IsAutoCloseConnection = true//自动释放
            });
            #endregion
            #region AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfile));
            #endregion
            #region EasyCaching
            services.AddEasyCaching(option =>
            {
                //use memory cache
                option.UseInMemory("default");
            });
            #endregion
            #region JWT鉴权
            services.AddCustomJWT();
            #endregion
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<AdminService>().As<IAdminService>().InstancePerLifetimeScope();
            builder.RegisterType<StudentService>().As<IStudentService>().InstancePerLifetimeScope();
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().InstancePerLifetimeScope();
            builder.RegisterType<MemoryCacheManager>().As<ILocker>().As<IStaticCacheManager>().SingleInstance();
            var webAssemblytype = typeof(Program).Assembly;
            builder.RegisterAssemblyTypes(webAssemblytype).PropertiesAutowired();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WY.WebApi v1"));
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute
                        (name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute
                        ("areaRoute", "{area:exists}/{controller=Admin}/{action=Index}/{id?}");
            });
        }
    }
    public static class IOCExtend
    {
        public static IServiceCollection AddCustomJWT(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SDMC-CJAS1-SAD-DFSFA-SADHJVF-VF")),
                        ValidateIssuer = true,
                        ValidIssuer = "http://localhost:6060",
                        ValidateAudience = true,
                        ValidAudience = "http://localhost:5000",
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(60)
                    };
                });
            return services;
        }
    }
}
