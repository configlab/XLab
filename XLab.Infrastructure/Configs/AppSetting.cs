using System;
using System.Collections.Generic;
using System.Text;

namespace XLab.Infrastructure.Configs
{
    public class AppSetting
    {
        public LoggingSettign Logging { get; set; }
        public RedisSetting Redis { get; set; }
    }
    public class RedisSetting
    {
        public string ConnectStr { get; set; }

        public int DefaultExpiryInSeconds { get; set; }
    }
    public class LoggingSettign
    {
        public LogLevelSetting LogLevel { get; set; }
        public FileLogPathSetting FileLogPath { get; set; }
    }
    public class FileLogPathSetting
    {
        public string LogPath { get; set; }
        public int MinLogLevel { get; set; }
    }
    public class LogLevelSetting
    {
        public string Default { get; set; }

        public string Microsoft { get; set; }
    }
}
