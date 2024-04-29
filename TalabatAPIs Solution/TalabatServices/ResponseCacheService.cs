using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TalabatCore.Repositories;

namespace TalabatServices
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;
        public ResponseCacheService(IConnectionMultiplexer redias)
        {
            _database = redias.GetDatabase();
        }

        public async Task CacheResponseAsync(string CasheKey, object response, TimeSpan TimeToLive)
        {
            if (response is null) return;

            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var Serializedresponse = JsonSerializer.Serialize(response);

            await _database.StringSetAsync(CasheKey, Serializedresponse,TimeToLive);
        }

        public async Task<string?> GetCacheResponseAsync(string CashKey)
        {
            var CachedResponse = await _database.StringGetAsync(CashKey);

            if (CachedResponse.IsNullOrEmpty) return null;

            return CachedResponse;
        }
    }
}
