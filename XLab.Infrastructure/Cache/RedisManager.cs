using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XLab.Infrastructure.Logs;

namespace XLab.Infrastructure.Cache
{
    /// <summary>
    /// 作者: http://config.net.cn
    /// 创建时间:2022-4-5
    /// </summary>
    public class RedisManager: IRedisManager
    {
        private readonly IConnectionMultiplexer _connMultiplexer;

        //private readonly ILogger logger =Log4Logger.Instance;

        private readonly string DefaultPrefix;

        private readonly IDatabase _db;
        public RedisManager(string connectionString,string defaultPrefix,int redisDbIndex=0)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                ConfigurationOptions option = new ConfigurationOptions
                {
                    AbortOnConnectFail = false,
                    EndPoints = { connectionString }
                };
                _connMultiplexer = ConnectionMultiplexer.Connect(option);
                AddRegisterEvent();
                _db = _connMultiplexer.GetDatabase(redisDbIndex);
                DefaultPrefix = defaultPrefix;
            }
        }
        private string BuildKeyPrefix(string key)
        {
            if (string.IsNullOrEmpty(DefaultPrefix))
            {
                return key;
            }
            return $"{DefaultPrefix}:{key}";
        }
        public bool StringSet(string redisKey, string redisValue, TimeSpan? expiry = null)
        {
            redisKey = BuildKeyPrefix(redisKey);
            return _db.StringSet(redisKey, redisValue, expiry);
        }

        public string StringGet(string redisKey)
        {
            redisKey = BuildKeyPrefix(redisKey);
            return _db.StringGet(redisKey);
        }
        public async Task<bool> StringSetAsync(string redisKey, string redisValue, TimeSpan? expiry = null)
        {
            redisKey = BuildKeyPrefix(redisKey);
            return await _db.StringSetAsync(redisKey, redisValue, expiry);
        }

        public async Task<string> StringGetAsync(string redisKey)
        {
            redisKey = BuildKeyPrefix(redisKey);
            return await _db.StringGetAsync(redisKey);
        }
        private void AddRegisterEvent()
        {
            _connMultiplexer.ConnectionRestored += _connMultiplexer_ConnectionRestored;
            _connMultiplexer.ConnectionFailed += _connMultiplexer_ConnectionFailed;
            _connMultiplexer.ErrorMessage += _connMultiplexer_ErrorMessage; ;
            _connMultiplexer.ConfigurationChanged += _connMultiplexer_ConfigurationChanged; ;
            _connMultiplexer.HashSlotMoved += _connMultiplexer_HashSlotMoved; ;
            _connMultiplexer.InternalError += _connMultiplexer_InternalError; ;
            _connMultiplexer.ConfigurationChangedBroadcast += _connMultiplexer_ConfigurationChangedBroadcast; ;
        }


        private void _connMultiplexer_ConfigurationChangedBroadcast(object sender, EndPointEventArgs e)
        {
            //logger.Debug($"{nameof(_connMultiplexer_ConfigurationChangedBroadcast)}--ex:{e.EndPoint}");
        }

        private void _connMultiplexer_InternalError(object sender, InternalErrorEventArgs e)
        {
            //logger.Debug($"{nameof(_connMultiplexer_InternalError)}--ex.endpoint={e.EndPoint},ex.msg={e.Exception?.Message??""},ex.connectType={e.ConnectionType},ex.Orgin={e.Origin}");
        }

        private void _connMultiplexer_HashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            //logger.Debug($"{nameof(_connMultiplexer_HashSlotMoved)}--ex.HashSlot={e.HashSlot},ex.NewEndPoint={e.NewEndPoint},ex.OldEndPoint={e.OldEndPoint}");
        }

        private void _connMultiplexer_ConfigurationChanged(object sender, EndPointEventArgs e)
        {
            //logger.Debug($"{nameof(_connMultiplexer_ConfigurationChanged)}--ex.EndPoint={e.EndPoint}");
        }

        private void _connMultiplexer_ErrorMessage(object sender, RedisErrorEventArgs e)
        {
            //logger.Debug($"{nameof(_connMultiplexer_ErrorMessage)}--ex.EndPoint={e.EndPoint},ex.Message={e.Message??""}");
        }

        private void _connMultiplexer_ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            //logger.Debug($"{nameof(_connMultiplexer_ConnectionFailed)}--ex.EndPoint={e.EndPoint},ex.Message={e.Exception?.Message ?? ""}");
        }

        private void _connMultiplexer_ConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            //logger.Debug($"{nameof(_connMultiplexer_ConnectionRestored)}--ex.EndPoint={e.EndPoint},ex.Message={e.Exception?.Message ?? ""}");
        }
    }
}
