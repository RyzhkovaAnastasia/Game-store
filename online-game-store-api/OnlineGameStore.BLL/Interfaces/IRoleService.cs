using OnlineGameStore.BLL.Models.AuthModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Interfaces
{
    public interface IRoleService
    {
        IEnumerable<RoleModel> GetAll();
        Task<RoleModel> GetByIdAsync(Guid id);
    }
}
