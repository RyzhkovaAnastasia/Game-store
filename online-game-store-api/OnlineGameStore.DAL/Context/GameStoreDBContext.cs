using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OnlineGameStore.DAL.Contex.EntitiesConfiguration;
using OnlineGameStore.DAL.Context.EntitiesConfiguration;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace OnlineGameStore.DAL.Context
{
    public class GameStoreDBContext : IdentityDbContext<User, Role, Guid>, IGameStoreDBContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GameGenre> GameGenres { get; set; }
        public DbSet<GamePlatformType> GamePlatformTypes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PlatformType> PlatformTypes { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public GameStoreDBContext(DbContextOptions<GameStoreDBContext> contextOptions) : base(contextOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder
                .UseLazyLoadingProxies()
                .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.DetachedLazyLoadingWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameStoreDBContext).Assembly);

            List<Publisher> publishers = new PublisherConfiguration().Publishers;
            modelBuilder.Entity<Publisher>().HasData(publishers);

            List<Game> games = new GameConfiguration(publishers).Games;
            modelBuilder.Entity<Game>().HasData(games);

            games.ForEach(x => modelBuilder.Entity<GameGenre>().HasData(new GameGenreConfiguration().GenerateGenres(x.Id)));
            games.ForEach(x => modelBuilder.Entity<Comment>().HasData(new CommentConfiguration().GenerateComments(x.Id)));
            games.ForEach(x => modelBuilder.Entity<GamePlatformType>().HasData(new GamePlatformTypeConfiguration().GeneratePlatforms(x.Id)));
        }
    }
}
