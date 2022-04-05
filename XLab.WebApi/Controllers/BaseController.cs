using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLab.WebApi.Interceptor.Filters;

namespace XLab.WebApi.Controllers
{
    /// <summary>
    /// 作者: http://config.net.cn
    /// 创建时间:2022-4-5
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [XLabAthenticationAttr]//[Authorize]
    public class BaseController : ControllerBase
    {
    }
}
