using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLab.Infrastructure.Cache;
using XLab.Infrastructure.Configs;
using System.Reflection;
using XLab.Service.Demo;

namespace XLab.WebApi.Global
{
    /// <summary>
    /// 作者: http://config.net.cn
    /// 创建时间:2022-4-5
    /// </summary>
    public static class ServiceCollectionExtension
    {
        public static void ConfigureServiceCollection(this IServiceCollection services, AppSetting settings)
        {
            services.AddSingleton<IRedisManager>(t => new RedisManager(settings.Redis.ConnectStr,"XLab",0));
            services.AddSingleton<AppSetting>(settings);
            //service.Scan :Scrutor
            services.Scan(scan => scan.FromAssemblyOf<IDataSignService>()
            .AddClasses(t => t.Where(type => type.Name.EndsWith("Service")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        }
    }
}
