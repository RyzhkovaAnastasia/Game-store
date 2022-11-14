using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OnlineGameStore.DAL.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineGameStore.DAL.Interfaces
{
    public interface IGameStoreDBContext : IDisposable
    {
        DatabaseFacade Database { get; }
        DbSet<Game> Games { get; set; }
        DbSet<Genre> Genres { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<PlatformType> PlatformTypes { get; set; }
        DbSet<Publisher> Publishers { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderDetail> OrderDetails { get; set; }
        DbSet<GamePlatformType> GamePlatformTypes { get; set; }
        DbSet<GameGenre> GameGenres { get; set; }

        int SaveChanges();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        EntityEntry Entry(object entity);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
    }
}
