using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace XLab.WebApi.Interceptor.Filters
{
    public class XLabAthenticationAttr: Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string reqAuthSchema = context.HttpContext?.Request?.Headers["AuthSchema"];
            var realAuthSchema = AuthSchema.GetAuthSchema(reqAuthSchema);
            var res = context.HttpContext.AuthenticateAsync(realAuthSchema).Result;
            if (!res.Succeeded&&!HasAllowAnonymous(context))//身份验证失败或者匿名api
            {
                context.Result = new ObjectResult(res?.Failure?.Message) { StatusCode = 401 };
            }
        }
        private bool HasAllowAnonymous(AuthorizationFilterContext context)
        {
            var filters = context.Filters;
            for (var i = 0; i < filters.Count; i++)
            {
                if (filters[i] is IAllowAnonymousFilter)
                {
                    return true;
                }
            }
            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return true;
            }
            return false;
        }
        public static class AuthSchema
        {
            public static readonly List<string> ListAuthSchema = new List<string>();
            static AuthSchema()
            {
                ListAuthSchema.AddRange(new string[] { OpenAPI });
            }
            public static string GetAuthSchema(string authSchema)
            {
                if (ListAuthSchema.Contains(authSchema))
                    return authSchema;
                return JwtBearerDefaults.AuthenticationScheme;
            }
            public const string OpenAPI = "OpenAPI";
        }
    }
}
