using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using TalabatCore.Repositories;

namespace TalabatAPIs.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CachedAttribute(int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var casheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>(); // Ask CLR To Inject object from class implement IResponseCacheService Explicitly

            var CasheKey = GenerateCasheKeyFromRequest(context.HttpContext.Request);

            var CacheResponse = await casheService.GetCacheResponseAsync(CasheKey);

            if (!string.IsNullOrEmpty(CacheResponse))
            {
                var ContentResult = new ContentResult
                {
                    Content = CacheResponse,
                    ContentType = "application/json",
                    StatusCode=200
                };

                context.Result = ContentResult;
                return;
            }

            var ExecutedEndpointContext = await next.Invoke(); // Will Excute endpoint - return context after Exuted endpoint

            if(ExecutedEndpointContext.Result  is OkObjectResult result )
            {
                await casheService.CacheResponseAsync(CasheKey,result.Value,TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }
        }

        private string GenerateCasheKeyFromRequest(HttpRequest request)
        {
            var KeyBuilder = new StringBuilder();

            KeyBuilder.Append(request.Path); // api/Controllername

            foreach (var (key,value) in request.Query.OrderBy(x=>x.Key))
            {
                KeyBuilder.Append($"|{key}-{value}");
            }

            return KeyBuilder.ToString();
        }
    }
}
