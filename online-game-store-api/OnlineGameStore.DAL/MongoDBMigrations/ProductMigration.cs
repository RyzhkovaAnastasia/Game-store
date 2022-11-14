using Mongo.Migration.Migrations.Database;
using MongoDB.Driver;
using OnlineGameStore.DAL.Entities;
using System.Text.RegularExpressions;

namespace OnlineGameStore.DAL.MongoDBMigrations
{
    public class ProductMigration : DatabaseMigration
    {
        public ProductMigration() : base("0.0.2")
        {
        }

        public override void Down(IMongoDatabase db)
        {
            FilterDefinition<Game> filter = Builders<Game>.Filter.Empty;
            UpdateDefinition<Game> update = Builders<Game>.Update.Unset(u => u.ViewsNumber);
            db.GetCollection<Game>("products").UpdateMany(filter, update);

            update = Builders<Game>.Update.Unset(u => u.Key);
            db.GetCollection<Game>("products").UpdateMany(filter, update);
        }

        public override void Up(IMongoDatabase db)
        {
            FilterDefinition<Game> filter = Builders<Game>.Filter.Empty;
            UpdateDefinition<Game> update = Builders<Game>.Update.Set(u => u.ViewsNumber, 0);
            db.GetCollection<Game>("products").UpdateMany(filter, update);

            foreach (Game game in db.GetCollection<Game>("products").Find(filter).ToList())
            {
                filter = Builders<Game>.Filter.Where(x => x.Name == game.Name);
                update = Builders<Game>.Update.Set(u => u.Key, Regex.Replace(game.Name, @"[^0-9a-zA-Z]+", "-").ToLower());
                db.GetCollection<Game>("products").UpdateOne(filter, update);
            }
        }
    }
}
