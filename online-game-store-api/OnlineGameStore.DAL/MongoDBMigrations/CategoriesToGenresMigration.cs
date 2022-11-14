using Mongo.Migration.Migrations.Database;
using MongoDB.Driver;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using System.Linq;

namespace OnlineGameStore.DAL.MongoDBMigrations
{
    public class CategoriesToGenresMigration : DatabaseMigration
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoriesToGenresMigration(IUnitOfWork unitOfWork) : base("0.0.1")
        {
            _unitOfWork = unitOfWork;
        }

        public override void Down(IMongoDatabase db)
        {
            FilterDefinition<Genre> filter = Builders<Genre>.Filter.Empty;
            IMongoCollection<Genre> collection = db.GetCollection<Genre>("categories");
            System.Collections.Generic.List<Genre> entities = collection.Find(filter).ToList();
            entities.ForEach(e => _unitOfWork.GenreRepository.DeleteAsync(e.Id).Wait());
        }

        public override void Up(IMongoDatabase db)
        {
            FilterDefinition<Genre> filter = Builders<Genre>.Filter.Empty;
            IMongoCollection<Genre> collection = db.GetCollection<Genre>("categories");
            System.Collections.Generic.List<Genre> entities = collection.Find(filter).ToList();
            entities.ForEach(e => _unitOfWork.GenreRepository.CreateAsync(e).Wait());
        }
    }
}
