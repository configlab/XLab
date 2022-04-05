using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLab.Infrastructure.Configs;
using XLab.Infrastructure.Logs;
using XLab.WebApi.Global;
using XLab.WebApi.Interceptor.Filters;
using System.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using static XLab.WebApi.Interceptor.Filters.XLabAthenticationAttr;
using XLab.Common.Securitys;

namespace XLab.WebApi
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public AppSetting AppSettings { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            AppSettings = configuration.Get<AppSetting>();//configuration.GetSection("AppConfig").Get<AppSetting>();
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(
                  options => {
                      options.Filters.Add(new XLabApiStatisticFilter());//xlab:filter
                  }
                );
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,//是否验证失效时间
                    ClockSkew = TimeSpan.FromSeconds(10),
                    ValidateAudience = true,//是否验证Audience
                    //ValidAudience = Const.GetValidudience(),//Audience
                    //采用动态验证的方式，在重新登陆时，刷新token，旧token就强制失效了
                    AudienceValidator = (m, n, z) =>
                    {
                        //m:audience,  n:token,z:TokenValidationParameters,这里时为了模拟如何支持动态让一个用户的jwttoken失效
                        //需要将audience存储到redis等，虽然破坏了jwt无状态性，但依然保留了分布式的特性.
                        var valideAudience= m != null && m.FirstOrDefault().Equals("audience_user1_random0001");
                        return valideAudience;
                    },
                    ValidateIssuer = true,//是否验证Issuer
                    ValidIssuer = AppSettings.Security.Jwt.Default.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.Security.Jwt.Default.SecurityKey))//拿到SecurityKey
                })
                 .AddJwtBearer(AuthSchema.OpenAPI, options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters()
                     {
                         ValidateLifetime =false,
                         ValidateAudience = false,
                         ValidateIssuer = true,
                         ValidIssuer = AppSettings.Security.Jwt.OpenAPI.Issuer,
                         ValidAudience = "config.net.cn",
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new ECDsaSecurityKey(EccUtils.LoadPublicKey(AppSettings.Security.Jwt.OpenAPI.SecurityKey))
                     };
                 });

            services.ConfigureServiceCollection(AppSettings);//xlab:reg
            services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
                .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);//xlab:allow filter to read post body content
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();//启用身份验证
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            loggerFactory.AddProvider(new DefaultFileLoggerProvider(AppSettings));//xlab:add logging
            //app.Use(next => context =>
            //{
            //    context.Request.EnableBuffering();//xlab:allow filter to read post body content
            //    return next(context);
            //});
        }
    }
}
