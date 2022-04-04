using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XLab.Infrastructure.Cache
{
    public interface IRedisManager
    {
        string StringGet(string redisKey);
        Task<string> StringGetAsync(string redisKey);
        bool StringSet(string redisKey, string redisValue, TimeSpan? expiry = null);
        Task<bool> StringSetAsync(string redisKey, string redisValue, TimeSpan? expiry = null);
    }
}
