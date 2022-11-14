using OnlineGameStore.BLL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Interfaces
{
    public interface ICommentService
    {
        Task<CommentModel> GetByIdAsync(Guid id);
        Task<IEnumerable<CommentModel>> GetByGameKeyAsync(string key);
        Task<CommentModel> CreateAsync(CommentModel newComment);
        Task DeleteAsync(Guid id);
    }
}
