using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using XLab.Common.Securitys;
using XLab.Entity.Demo;
using XLab.Infrastructure.Interceptor;

namespace XLab.Service.Demo
{
    public class DataSignService:IDataSignService
    {
        public string GetSign(SignRequestModel data)
        {
            if (data == null)
                return string.Empty;
            return Sha256Utils.HashString(JsonConvert.SerializeObject(data));
        }
    }
}
