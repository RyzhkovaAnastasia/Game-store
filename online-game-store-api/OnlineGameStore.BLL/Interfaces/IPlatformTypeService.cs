using OnlineGameStore.BLL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Interfaces
{
    public interface IPlatformTypeService
    {
        Task<IEnumerable<PlatformTypeModel>> GetAllAsync();
        Task<PlatformTypeModel> GetByIdAsync(Guid id);
        Task<PlatformTypeModel> CreateAsync(PlatformTypeModel newPlatformType);
        Task<PlatformTypeModel> EditAsync(PlatformTypeModel updatedPlatformType);
        Task DeleteAsync(Guid id);
    }
}
