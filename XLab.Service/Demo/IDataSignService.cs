using System;
using System.Collections.Generic;
using System.Text;
using XLab.Entity.Demo;
using XLab.Infrastructure.Interceptor;

namespace XLab.Service.Demo
{
    [Cached]
    public interface IDataSignService
    {
        public string GetSign(SignRequestModel data);
    }
}
