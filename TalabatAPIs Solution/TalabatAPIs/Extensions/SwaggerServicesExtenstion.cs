namespace TalabatAPIs.Extensions
{
	public static class SwaggerServicesExtenstion
	{
		public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
		{

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer(); //  Allow DI to Swagger Services
            services.AddSwaggerGen();           //  Allow DI to Swagger Services

            return services;
		}

		public static WebApplication UseSwaggerMiddleWare(this WebApplication app)
		{
			app.UseSwagger();
			app.UseSwaggerUI();

			return app;
		}
	}
}
