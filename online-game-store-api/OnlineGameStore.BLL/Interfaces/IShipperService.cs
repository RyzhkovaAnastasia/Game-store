using OnlineGameStore.BLL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Interfaces
{
    public interface IShipperService
    {
        Task<IEnumerable<ShipperModel>> GetAllAsync();
        Task<ShipperModel> GetByIdAsync(Guid id);
    }
}
