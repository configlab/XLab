using System;
using System.Collections.Generic;
using System.Text;

namespace XLab.Infrastructure.Configs
{
    public class AppSetting
    {
        public LoggingSettign Logging { get; set; }
        public RedisSetting Redis { get; set; }
        public SecuritySetting Security { get; set; }
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
    /// <summary>
    /// 这里需要放到secre.json中，也就是机密文件，否则将上传到代码仓库，不安全。
    /// </summary>
    public class SecuritySetting
    {
        public AuthenticationSetting Authentication { get; set; }
    }
    public class AuthenticationSetting
    {
        public JwtTokenSetting DefaultJwt { get; set; }
    }
    public class JwtTokenSetting
    {
        public string Issuer { get; set; }
        public string SecurityKey { get; set; }
    }
}
