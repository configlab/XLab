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
    [Route("api/[controller]")]
    [ApiController]
    [XLabAthenticationAttr]//[Authorize]
    public class BaseController : ControllerBase
    {
    }
}
