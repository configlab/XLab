using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using XLab.Infrastructure.Configs;

namespace XLab.Infrastructure.Logs
{
    public class DefaultFileLoggerProvider: ILoggerProvider
    {
        private readonly AppSetting _config;

        public DefaultFileLoggerProvider(AppSetting config)
        {
            this._config = config;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DefaultFileLogger(categoryName, _config);
        }

        public void Dispose()
        {

        }
    }
}
