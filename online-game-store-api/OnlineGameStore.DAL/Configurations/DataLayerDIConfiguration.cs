using LightInject;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Migration.Migrations.Adapters;
using Mongo.Migration.Startup;
using Mongo.Migration.Startup.Static;
using MongoDB.Driver;
using OnlineGameStore.DAL.Configurations.EntitiesBsonConfigurations;
using OnlineGameStore.DAL.Context;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Repositories;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace OnlineGameStore.DAL.Configurations
{
    [ExcludeFromCodeCoverage]
    public static class DataLayerConfiguration
    {
        public static void SetDbContextSQL(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<GameStoreDBContext>(options =>
            options.UseSqlServer(connectionString).ConfigureWarnings(builder =>
                    builder.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning)));

            DataLayerDIConfigure(services);
        }

        public static void SetDbContextMongo(IServiceCollection services, string connectionString, string dbName)
        {
            NorthwindDBContext northwindDb = new NorthwindDBContext(connectionString, dbName);
            services.AddScoped<INorthwindDBContext, NorthwindDBContext>(
                x => northwindDb);

            ConfigureBson();

            services.AddSingleton<IMongoClient>(s => new MongoClient(connectionString));

            ServiceContainer container = new ServiceContainer();
            container.Register<IUnitOfWork, UnitOfWork>();
            container.Register(x => services.BuildServiceProvider().GetService<IGameStoreDBContext>());
            container.Register<INorthwindDBContext>(x => new NorthwindDBContext(connectionString, dbName));
            container.Register<ILogger>(_ => null);

            MongoMigrationClient.Initialize(
                new MongoClient(connectionString),
                new MongoMigrationSettings()
                {
                    ConnectionString = connectionString,
                    Database = dbName
                },
                new LightInjectAdapter(container));

        }

        private static void DataLayerDIConfigure(IServiceCollection services)
        {
            services.AddScoped<IGameStoreDBContext, GameStoreDBContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();
            ResolveIdentity(services);
            ResolveSQLRepositories(services);
            ResolveRepositories(services);
        }

        private static void ResolveIdentity(IServiceCollection services)
        {
            services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<GameStoreDBContext>()
            .AddDefaultTokenProviders();
        }

        private static void ResolveRepositories(IServiceCollection services)
        {
            services.AddScoped<IRepository<Game>, Repository<Game>>();
            services.AddScoped<IRepository<Genre>, Repository<Genre>>();
            services.AddScoped<IRepository<Comment>, Repository<Comment>>();
            services.AddScoped<IRepository<PlatformType>, Repository<PlatformType>>();
            services.AddScoped<IRepository<Publisher>, Repository<Publisher>>();
            services.AddScoped<IRepository<Order>, Repository<Order>>();
            services.AddScoped<IRepository<OrderDetail>, Repository<OrderDetail>>();
            services.AddScoped<IRepository<PaymentMethod>, Repository<PaymentMethod>>();
        }

        private static void ResolveSQLRepositories(IServiceCollection services)
        {
            services.AddScoped<ISQLRepository<Order>, SQLRepository<Order>>();
            services.AddScoped<ISQLRepository<OrderDetail>, SQLRepository<OrderDetail>>();
        }

        private static void ConfigureBson()
        {
            new OrderBsonConfiguration();
            new OrderDetailBsonConfiguration();
            new GameBsonConfiguration();
            new PublisherBsonConfiguration();
            new BaseEntityBsonConfiguration();
            new ShipperBsonConfiguration();
        }
    }
}
