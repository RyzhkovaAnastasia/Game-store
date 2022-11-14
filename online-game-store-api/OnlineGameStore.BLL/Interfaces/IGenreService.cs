using OnlineGameStore.BLL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreModel>> GetAllAsync();
        Task<GenreModel> GetByIdAsync(Guid id);
        Task<GenreModel> CreateAsync(GenreModel newGenre);
        Task<GenreModel> EditAsync(GenreModel updatedGenre);
        Task DeleteAsync(Guid id);

    }
}
