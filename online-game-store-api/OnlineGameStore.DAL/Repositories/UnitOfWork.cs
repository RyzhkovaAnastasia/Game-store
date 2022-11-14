using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using Serilog;
using System;
using System.Data;
using System.Threading.Tasks;

namespace OnlineGameStore.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IGameStoreDBContext _sqlDB;
        private readonly INorthwindDBContext _mongoDB;
        private readonly ILogger _logger;

        #region Repositories
        public IRepository<Game> GameRepository =>
            new Repository<Game>(
                new GameRepository(_sqlDB),
                new MongoRepository<Game>(_mongoDB, "products"),
                _logger);

        public IRepository<Genre> GenreRepository =>
            new Repository<Genre>(
                new SQLRepository<Genre>(_sqlDB),
                null,
                _logger);

        public IRepository<Comment> CommentRepository =>
            new Repository<Comment>(
                new SQLRepository<Comment>(_sqlDB),
                null,
                _logger);

        public IRepository<PlatformType> PlatformTypeRepository =>
            new Repository<PlatformType>(
                new SQLRepository<PlatformType>(_sqlDB),
                null,
                _logger);

        public IRepository<Publisher> PublisherRepository =>
            new Repository<Publisher>(
                new SQLRepository<Publisher>(_sqlDB),
                new MongoRepository<Publisher>(_mongoDB, "suppliers"),
                _logger);

        public IRepository<Order> OrderRepository =>
            new Repository<Order>(
                new SQLRepository<Order>(_sqlDB),
                new MongoRepository<Order>(_mongoDB, "orders"),
                _logger);

        public IRepository<OrderDetail> OrderDetailRepository =>
             new Repository<OrderDetail>(
                new SQLRepository<OrderDetail>(_sqlDB),
                new MongoRepository<OrderDetail>(_mongoDB, "order-details"),
                _logger);

        public IRepository<PaymentMethod> PaymentMethodRepository =>
            new Repository<PaymentMethod>(
                new SQLRepository<PaymentMethod>(_sqlDB),
                null,
                _logger);

        public IRepository<Shipper> ShipperRepository =>
            new Repository<Shipper>(
                null,
                new MongoRepository<Shipper>(_mongoDB, "shippers"),
                _logger);
        #endregion

        public UnitOfWork(IGameStoreDBContext sqlDB, INorthwindDBContext mongoDB, ILogger logger = null)
        {
            _sqlDB = sqlDB;
            _mongoDB = mongoDB;
            _logger = logger;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel)
        {
            return await _sqlDB.Database.BeginTransactionAsync(isolationLevel);
        }

        public async Task CommitTransaction(IDbContextTransaction transaction)
        {
            await transaction.CommitAsync();
        }

        public async Task RollbackTransaction(IDbContextTransaction transaction)
        {
            await transaction.RollbackAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _sqlDB.Dispose();
        }
    }
}
