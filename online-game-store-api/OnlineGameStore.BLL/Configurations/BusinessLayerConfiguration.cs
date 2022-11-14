using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OnlineGameStore.BLL.Configurations.JWT;
using OnlineGameStore.BLL.Configurations.MapperConfigurations;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.DAL.Configurations;

namespace OnlineGameStore.BLL.Configurations
{

    public static class BusinessLayerConfiguration
    {
        public static void SetDbContextSQL(IServiceCollection services, string connectionString)
        {
            DataLayerConfiguration.SetDbContextSQL(services, connectionString);
            services.AddHangfire(x =>
            {
                x.UseSqlServerStorage(connectionString);
                x.UseSerializerSettings(new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            });
            services.AddHangfireServer();
        }
        public static void SetDbContextMongo(IServiceCollection services, string connectionString, string dbName)
        {
            DataLayerConfiguration.SetDbContextMongo(services, connectionString, dbName);
        }

        public static void BusinessLayerDIConfigure(IServiceCollection services)
        {
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IPlatformTypeService, PlatformTypeService>();
            services.AddScoped<IPublisherService, PublisherService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IShipperService, ShipperService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
        }
        public static void MapperConfigure(IServiceCollection services)
        {
            MapperConfiguration mapConfig = new MapperConfiguration(mc => new MappingProfile(mc));
            services.AddSingleton(mapConfig.CreateMapper());
        }

        public static void AuthConfigure(IServiceCollection services, AuthOptions authOptions)
        {
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = authOptions.SymmetricSecurityKey,

                    ValidateAudience = true,
                    ValidAudience = authOptions.Audience,

                    ValidateIssuer = true,
                    ValidIssuer = authOptions.Issuer,

                    ValidateLifetime = true

                };
            });
        }
    }
}
