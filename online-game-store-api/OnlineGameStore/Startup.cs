using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OnlineGameStore.BLL.Configurations;
using OnlineGameStore.BLL.Configurations.JWT;
using OnlineGameStore.Extensions;
using OnlineGameStore.Middleware;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace OnlineGameStore
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
              .AddNewtonsoftJson(options =>
              {
                  options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                  options.SerializerSettings.Converters.Add(new StringEnumConverter());
              });


            string frontOptions = Configuration["FrontAdress"];
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials()
                      .WithExposedHeaders("Content-Disposition")
                      .WithOrigins(frontOptions);
                });
            });

            services.TryAddSingleton<IBackgroundJobClient, BackgroundJobClient>();

            string connectionGameStoreSQL = Configuration["GameStoreDb"];
            string connectionNorthwindMongo = Configuration["NorthwindDb"];
            string nameNorthwindDb = Configuration["NorthwindDbName"];
            string connectionLoggerSQL = Configuration["DatabaseLoggingConnectionString"];

            //Databases connection
            BusinessLayerConfiguration.SetDbContextSQL(services, connectionGameStoreSQL);
            BusinessLayerConfiguration.SetDbContextMongo(services, connectionNorthwindMongo, nameNorthwindDb);

            //BLL dependencies
            BusinessLayerConfiguration.BusinessLayerDIConfigure(services);
            BusinessLayerConfiguration.MapperConfigure(services);

            //Logger
            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(Configuration)
               .CreateLogger();

            services.AddSingleton(_ => Log.Logger);

            //Auth
            var authOptionsSection = Configuration.GetSection("Auth");
            services.Configure<AuthOptions>(authOptionsSection);
            var authOptions = authOptionsSection.Get<AuthOptions>();
            BusinessLayerConfiguration.AuthConfigure(services, authOptions);
            services.AddSingleton<UserClaims>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHangfireDashboard("/mydashboard");
            }

            loggerFactory.AddSerilog();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                Secure = CookieSecurePolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.None
            });


            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}
