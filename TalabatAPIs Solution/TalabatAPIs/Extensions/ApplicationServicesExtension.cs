using Microsoft.AspNetCore.Mvc;
using TalabatAPIs.Error;
using TalabatAPIs.Helpers;
using TalabatCore;
using TalabatCore.Repositories;
using TalabatCore.Services;
using TalabatRebosatiory;
using TalabatServices;

namespace TalabatAPIs.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddSingleton<IResponseCacheService, ResponseCacheService>();

			services.AddScoped(typeof(IUnitofwork),typeof(Unitofwork));

			services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

			services.AddScoped<IorderService, OrderServices>();

			services.AddScoped(typeof(IPaymentService),typeof(PaymentService));

			services.AddAutoMapper(typeof(MappingProfiles));

			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
											  .SelectMany(p => p.Value.Errors)
											  .Select(E => E.ErrorMessage)
											  .ToArray();

					var ValidationErrorResponse = new ApiValidationErrorResponse()
					{
						Errors = errors
					};

					return new BadRequestObjectResult(ValidationErrorResponse);
				};
			});

			return services;
		}
	}
}
