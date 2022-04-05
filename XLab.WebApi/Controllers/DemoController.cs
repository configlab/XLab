using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XLab.Entity.Demo;
using XLab.Infrastructure.Configs;
using XLab.Infrastructure.Interceptor;
using XLab.Service.Demo;

namespace XLab.WebApi.Controllers
{
    public class DemoController : BaseController
    {

        private readonly ILogger<DemoController> _logger;
        private readonly IDataSignService _dataSignService;
        private readonly IRandomService _randomService;
        private readonly AppSetting _appSetting;

        public DemoController(IDataSignService dataSignService,
            IRandomService randomService,
            AppSetting appSetting,
            ILogger<DemoController> logger)
        {
            _dataSignService = dataSignService;
            _randomService = randomService;
            _appSetting = appSetting;
            _logger = logger;
            _logger.LogInformation("log......", null);
        }
        [HttpGet("random")]
        public string GetRandom()
        {
            return _randomService.GetRandom();
        }
        [HttpPost("sign")]
        public string GetDataSign(SignRequestModel data)
        {
            return _dataSignService.GetSign(data);
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public string GetJwtToken(LoginRequestModel loginData)
        {
            if (!(loginData.UserName == "user1" && loginData.Pwd == "config.net.cn"))
            {
                Response.StatusCode = HttpStatusCode.Unauthorized.GetHashCode();
                Response.Headers["erromsg"] = "invalide account";
                Response.CompleteAsync();
                return string.Empty;
            }
            DateTime dTimeExpire = DateTime.Now.AddSeconds(60*60);
            var claims = new Claim[]
            {
                    new Claim(ClaimTypes.Name,"user1"),
                    new Claim(ClaimTypes.DateOfBirth, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),//token生效时间
                    new Claim(ClaimTypes.Expiration, $"{new DateTimeOffset(dTimeExpire).ToUnixTimeSeconds()}")//到期时间，按秒数计算
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSetting.Security.Jwt.Default.SecurityKey));//key至少是16位
            var token = new JwtSecurityToken(
            issuer: _appSetting.Security.Jwt.Default.Issuer,
            audience: $"audience_user1_random0001",
            claims: claims,
            notBefore: DateTime.Now,
            expires: dTimeExpire,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
