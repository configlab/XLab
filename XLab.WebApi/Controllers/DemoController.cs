using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLab.Entity.Demo;
using XLab.Infrastructure.Interceptor;
using XLab.Service.Demo;

namespace XLab.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {

        private readonly ILogger<DemoController> _logger;
        private readonly IDataSignService _dataSignService;

        public DemoController(IDataSignService dataSignService,
            ILogger<DemoController> logger)
        {
            _dataSignService = dataSignService;
            _logger = logger;
            _logger.LogInformation("log......", null);
        }
        [HttpGet("random")]
        public string GetRandom()
        {
            return new Random().Next(0, int.MaxValue).ToString();
        }
        [HttpPost("sign")]
        public string GetDataSign(RequestData data)
        {
            return _dataSignService.GetSign(data);
        }
    }
}
