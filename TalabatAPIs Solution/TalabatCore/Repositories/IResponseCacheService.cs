using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalabatCore.Repositories
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string CashKey,object response, TimeSpan TimeToLive);

        Task<string?> GetCacheResponseAsync(string CashKey);
    }
}
