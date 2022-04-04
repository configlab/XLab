using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XLab.Infrastructure.Configs;

namespace XLab.Infrastructure.Logs
{
    public class DefaultFileLogger : ILogger
    {
        private readonly string _name;
        private readonly AppSetting _config;
        private LogLevel _logLevel;

        public DefaultFileLogger(string name, AppSetting config)
        {
            _name = name;
            _config = config;
        }
        public bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel < LogLevel.Information)
                return false;
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            _logLevel = logLevel;
            FileLog($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:fff")} - {logLevel.ToString()} - {_name} - {formatter(state, exception)}");
        }

        private async void FileLog(string strLog)
        {
            try
            {
                string fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                string filePath = System.AppDomain.CurrentDomain.BaseDirectory + _config.Logging.FileLogPath.LogPath + "\\" + _logLevel.ToString();
                //filePath = System.Environment.CurrentDirectory +"\\"+ filePath;
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                string fileFulleName = filePath + "\\" + fileName;
                await File.AppendAllLinesAsync(fileFulleName, new string[] { strLog });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }
    }
}
