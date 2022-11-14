using OnlineGameStore.BLL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Interfaces
{
    public interface IPublisherService
    {
        Task<IEnumerable<PublisherModel>> GetAllAsync();
        Task<PublisherModel> GetByIdAsync(Guid id);
        Task<PublisherModel> GetByCompanyNameAsync(string companyName);
        Task<PublisherModel> CreateAsync(PublisherModel newPublisher);
        Task<PublisherModel> EditAsync(PublisherModel updatedPublisher);
        Task DeleteAsync(Guid id);
    }
}
