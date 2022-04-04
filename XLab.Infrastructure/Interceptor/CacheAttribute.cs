using AspectCore.DynamicProxy;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using XLab.Infrastructure.Cache;
using Newtonsoft.Json;
using XLab.Infrastructure.Configs;
using System.Linq;
using XLab.Common.Securitys;
using Microsoft.Extensions.Logging;

namespace XLab.Infrastructure.Interceptor
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class CachedAttribute : AbstractInterceptorAttribute
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo> _typeofTaskResultMethod = new ConcurrentDictionary<Type, MethodInfo>();
        public int ExpiryInSeconds { get; set; }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            ILogger<CachedAttribute> _logger=context.ServiceProvider.GetService<ILogger<CachedAttribute>>();
            var redisManager = context.ServiceProvider.GetService<IRedisManager>();//Microsoft.Extensions.DependencyInjection
            var settings = context.ServiceProvider.GetService<AppSetting>();
            int expiryInSeconds = GetExpirySeconds(settings);
            var methodReturnType = context.ServiceMethod.ReturnType;
            if (methodReturnType == typeof(void) || methodReturnType == typeof(Task) || context.ServiceMethod.GetParameters().Any(it => it.IsIn || it.IsOut))
            {
                await context.Invoke(next);
                return;
            }
            var isOriginalMethodThrowException = false;
            try
            {
                var isAsync = context.IsAsync();
                var cacheKey = GetCacheKey(context);
                var cachedValue = await redisManager.StringGetAsync(cacheKey);
                if (cachedValue == null)
                {
                    _logger.LogInformation($" RedisCache is not exist,expiryInSeconds={expiryInSeconds},key={cacheKey}");
                    try
                    {
                        await context.Invoke(next);
                    }
                    catch
                    {
                        isOriginalMethodThrowException = true;
                        throw;
                    }
                    dynamic returnValue = context.ReturnValue;
                    if (isAsync)
                    {
                        returnValue = returnValue.Result;
                    }
                    if (returnValue != null)
                    {
                        await redisManager.StringSetAsync(cacheKey, JsonConvert.SerializeObject(returnValue), TimeSpan.FromSeconds(expiryInSeconds));
                    }
                }
                else
                {
                    _logger.LogInformation($"call RedisCache,expiryInSecond={expiryInSeconds},key={cacheKey}");
                    if (isAsync)
                    {
                        var returnTypeForGenericTypeArgument = methodReturnType.GenericTypeArguments[0];
                        var objectPack = JsonConvert.DeserializeObject(cachedValue, returnTypeForGenericTypeArgument);
                        context.ReturnValue = ResultFactory(objectPack, returnTypeForGenericTypeArgument);
                    }
                    else
                    {
                        context.ReturnValue = JsonConvert.DeserializeObject(cachedValue, methodReturnType);
                    }
                }
            }
            catch
            {
                if (context.ReturnValue == null)
                {
                    if (!isOriginalMethodThrowException)
                    {
                        await context.Invoke(next);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        #region private helper
        /// <summary>
        /// get cache key
        /// </summary>
        /// <returns></returns>
        private string GetCacheKey(AspectContext context)
        {
            var typeName = context.Implementation.GetType().Name;
            var methodName = context.ServiceMethod.Name;
            var methodArguments = context.Parameters.Select(GetArgumentValue).Take(3);
            string key = $"{typeName}:{methodName}:";
            foreach (var param in methodArguments)
            {
                key = $"{key}{param}_";
            }
            return key.TrimEnd('_', ':');
        }

        /// <summary>
        /// object convert string
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private string GetArgumentValue(object arg)
        {
            if (arg is DateTime || arg is DateTime?)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");
            if (arg is string || arg is ValueType || IsNullable(arg))
                return arg.ToString();
            if (arg != null && arg.GetType().IsClass)
            {
                return Sha256Utils.HashString(JsonConvert.SerializeObject(arg));
            }
            return string.Empty;

            static bool IsNullable(object obj)
            {
                if (obj == null)
                    return false;
                var type = obj.GetType();
                return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
            }
        }

        private object ResultFactory(object result, Type returnType)
        {
            return _typeofTaskResultMethod.GetOrAdd(returnType, t => typeof(Task).GetMethods().First(p => p.Name == "FromResult" && p.ContainsGenericParameters).MakeGenericMethod(returnType))
                        .Invoke(null, new object[] { result });
        }

        private int GetExpirySeconds(AppSetting settings)
        {
            if (ExpiryInSeconds > 0)
            {
                return ExpiryInSeconds;
            }
            var expirySeconds = settings?.Redis?.DefaultExpiryInSeconds??0;
            if (expirySeconds > 0)
            {
                return expirySeconds;
            }
            return 30*60;
        }
        #endregion
    }
}
