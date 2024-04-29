using System.Net;
using System.Text.Json;
using TalabatAPIs.Error;

namespace TalabatAPIs.MiddleWares
{
	public class ExceptionMiddleWare
	{
		private readonly RequestDelegate _next;
		public ILogger<ExceptionMiddleWare> _logger { get; }
		private readonly IHostEnvironment _env;

		public ExceptionMiddleWare(RequestDelegate next, ILogger<ExceptionMiddleWare> logger, IHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
		}
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next.Invoke(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);

				context.Response.ContentType = "applicatin/json";
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

				///if (_env.IsDevelopment())
				///{
				///	var respone = new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString());
				///}
				///else
				///{
				///	var response = new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
				///}

				var response = _env.IsDevelopment() ? new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString()) : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

				var options = new JsonSerializerOptions()
				{
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase
				};
				var JsonResponse = JsonSerializer.Serialize(response, options);
				await context.Response.WriteAsync(JsonResponse);
			}
		}

	}
}
