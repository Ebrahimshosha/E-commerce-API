using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using TalabatAPIs.Error;
using TalabatAPIs.Extensions;
using TalabatAPIs.Helpers;
using TalabatAPIs.MiddleWares;
using TalabatCore.Entities;
using TalabatCore.Entities.Identity;
using TalabatCore.Repositories;
using TalabatRebosatiory;
using TalabatRebosatiory.Data;
using TalabatRebosatiory.Identity;

namespace TalabatAPIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services

            // Add services to the container.
            builder.Services.AddControllers();  // Allow DI to Web API services

            builder.Services.AddSwaggerServices();

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var Connection = builder.Configuration.GetConnectionString("ResiasConnection");
                return ConnectionMultiplexer.Connect(Connection);
            });


            builder.Services.AddApplicationServices(); // Her We allow DI For any service

            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddCors(Options =>
            {
                Options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                    options.WithOrigins(builder.Configuration["FrontBaseUrl"]);
                });
            });

            #endregion

            var app = builder.Build();

            #region Update-Database

            using var scope = app.Services.CreateScope(); // Group of services lifetime scopped
            var services = scope.ServiceProvider; // Services It Self

            var LoggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var dbcontext = services.GetRequiredService<StoreContext>(); // Ask CLR for creating object from DBcontext Explicitly  == allow DI For DBcontext (StoreContext)
                await dbcontext.Database.MigrateAsync();

                var Identitydbcontext = services.GetRequiredService<AppIdentityDbContext>(); // Ask CLR for creating object from DBcontext Explicitly  == allow DI For DBcontext (AppIdentityDbContext)
                await Identitydbcontext.Database.MigrateAsync();

                await StoreContextseed.seedAsync(dbcontext); // Data seeding

                var UserManager = services.GetRequiredService<UserManager<AppUser>>(); // Ask CLR for creating object from UserManager service Explicitly == allow DI For UserManager service
                await AppIdentityDbContextSeed.SeedUserAsync(UserManager);
            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An error occured during updating database");
            }

            #endregion

            #region Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage(); // In .net 5, this statement should be written to show an internal server error, but in .net 6, this statement is there by default, and finally whether 5 or 6, we commented that out to create my own middleware

                app.UseMiddleware<ExceptionMiddleWare>(); //  Server Error (Exception) Handling
                app.UseSwaggerMiddleWare();

            }

            //app.UseStatusCodePagesWithRedirects("/errors/{0}"); // two requsets
            app.UseStatusCodePagesWithReExecute("/errors/{0}"); // one requset

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseCors("MyPolicy");

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllers();


            #endregion

            app.Run();
        }
    }
}