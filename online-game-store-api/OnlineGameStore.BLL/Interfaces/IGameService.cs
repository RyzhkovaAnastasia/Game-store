using OnlineGameStore.BLL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<GameModel>> GetAllAsync();
        Task<FilteredGameModel> GetByFilter(string gameFilter);
        Task<GameModel> GetByIdAsync(Guid id);
        Task<GameModel> GetByKeyAsync(string key);
        Task<int> GetGameNumberAsync();
        Task<GameModel> CreateAsync(GameModel newGame);
        Task<GameModel> EditAsync(GameModel updatedGame);
        Task EditViewNumberAsync(string gamekey);
        Task DeleteAsync(Guid id);
    }
}
