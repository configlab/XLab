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
                      options.Filters.Add(new ApiStatisticFilter());//xlab:filter
                  }
                );
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
