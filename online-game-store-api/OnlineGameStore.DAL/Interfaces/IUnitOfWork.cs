using Microsoft.EntityFrameworkCore.Storage;
using OnlineGameStore.DAL.Entities;
using System;
using System.Data;
using System.Threading.Tasks;

namespace OnlineGameStore.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Game> GameRepository { get; }
        IRepository<Genre> GenreRepository { get; }
        IRepository<Comment> CommentRepository { get; }
        IRepository<PlatformType> PlatformTypeRepository { get; }
        IRepository<Publisher> PublisherRepository { get; }
        IRepository<Order> OrderRepository { get; }
        IRepository<OrderDetail> OrderDetailRepository { get; }
        IRepository<PaymentMethod> PaymentMethodRepository { get; }
        IRepository<Shipper> ShipperRepository { get; }
        Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel);
        Task CommitTransaction(IDbContextTransaction transaction);
        Task RollbackTransaction(IDbContextTransaction transaction);
    }
}
