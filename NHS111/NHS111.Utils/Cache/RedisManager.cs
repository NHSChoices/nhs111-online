using System.Threading.Tasks;
using StackExchange.Redis;

namespace NHS111.Utils.Cache
{
    public class RedisManager : ICacheManager<string, string>
    {
        private readonly IDatabase _database;

        public RedisManager(string connString)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connString);
            _database = redis.GetDatabase();
        }

        //public async void Set(string key, string value)
        //{
        //    await _database.StringSetAsync(key, value);
        //}

        //public async Task<string> Read(string key)
        //{
        //    return await _database.StringGetAsync(key);
        //}



        public void Set(string key, string value)
        {
            _database.StringSetAsync(key, value);
        }

        public async Task<string> Read(string key)
        {
            return await _database.StringGetAsync(key);
        }
    }
}