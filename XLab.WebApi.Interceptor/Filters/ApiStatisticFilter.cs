using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;

namespace XLab.WebApi.Interceptor.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ApiStatisticFilter: ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //return base.OnActionExecutionAsync(context, next);
            ILogger<ApiStatisticFilter> _logger = context.HttpContext.RequestServices.GetService<ILogger<ApiStatisticFilter>>();
            //操作Request.Body之前加上EnableBuffering即可
            context.HttpContext.Request.EnableBuffering();
            var request = context.HttpContext.Request;
            if (request.Method.ToLower().Equals("post"))
            {
                var method = request.Method;
                var requestPath = request.Path;
                if (!request.Body.CanRead)
                    return;
                //request.Body.Seek(0, SeekOrigin.Begin);
                request.Body.Position = 0;
                using (var reader = new StreamReader(request.Body))
                {
                    var param = await reader.ReadToEndAsync();
                    _logger.LogInformation($"ApiStatisticFilter-Log:[Method:{method} ; Path:{requestPath} ; bodyString:{param}]");
                }
            }
            await next();
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
