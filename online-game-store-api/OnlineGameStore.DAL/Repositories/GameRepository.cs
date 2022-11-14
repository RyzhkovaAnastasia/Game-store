using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using System.Threading.Tasks;

namespace OnlineGameStore.DAL.Repositories
{
    public class GameRepository : SQLRepository<Game>
    {
        public GameRepository(IGameStoreDBContext gameStoreDBContext) : base(gameStoreDBContext)
        {

        }

        public override async Task<Game> EditAsync(Game entity)
        {
            if (entity != null)
            {
                Game updatingEntity = await _gameStoreDBContext.Games.FirstOrDefaultAsync(x => x.Id == entity.Id);
                if (updatingEntity != null)
                {
                    updatingEntity.PlatformTypes?.Clear();
                    updatingEntity.PlatformTypes = entity.PlatformTypes;

                    updatingEntity.Genres?.Clear();
                    updatingEntity.Genres = entity.Genres;

                    updatingEntity.Publisher = null;

                    _gameStoreDBContext.Entry(updatingEntity).CurrentValues.SetValues(entity);
                    await _gameStoreDBContext.SaveChangesAsync(true);
                }
                return updatingEntity;
            }
            return entity;
        }
    }
}
