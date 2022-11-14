using AutoMapper;
using OnlineGameStore.BLL.CustomExceptions;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PublisherService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PublisherModel> CreateAsync(PublisherModel newPublisher)
        {
            await CheckPublisherUnique(newPublisher);
            Publisher publisherEntity = _mapper.Map<Publisher>(newPublisher);
            publisherEntity = await _unitOfWork.PublisherRepository.CreateAsync(publisherEntity);
            return _mapper.Map<PublisherModel>(publisherEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.PublisherRepository.SoftDeleteAsync(id);
        }

        public async Task<PublisherModel> EditAsync(PublisherModel updatedPublisher)
        {
            await CheckPublisherUnique(updatedPublisher);

            Publisher publisherEntity = _mapper.Map<Publisher>(updatedPublisher);
            publisherEntity = await _unitOfWork.PublisherRepository.EditAsync(publisherEntity);

            if (publisherEntity == null)
            {
                throw new NotFoundException();
            }
            return _mapper.Map<PublisherModel>(publisherEntity);
        }

        public async Task<IEnumerable<PublisherModel>> GetAllAsync()
        {
            var publisherEntities = await _unitOfWork.PublisherRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PublisherModel>>(publisherEntities);
        }

        public async Task<PublisherModel> GetByIdAsync(Guid id)
        {
            Publisher publisherEntity = await _unitOfWork.PublisherRepository.FindAsync(x => x.Id == id);
            if (publisherEntity == null)
            {
                throw new NotFoundException();
            }
            return _mapper.Map<PublisherModel>(publisherEntity);
        }

        public async Task<PublisherModel> GetByCompanyNameAsync(string companyName)
        {
            Publisher publisherEntity = await _unitOfWork.PublisherRepository.FindAsync(g => g.CompanyName == companyName);
            if (publisherEntity == null)
            {
                throw new NotFoundException();
            }
            return _mapper.Map<PublisherModel>(publisherEntity);
        }

        private async Task CheckPublisherUnique(PublisherModel publisher)
        {
            var publishers = await _unitOfWork.PublisherRepository.GetWhereAsNoTrackingAsync(x => publisher.CompanyName.ToLower() == x.CompanyName.ToLower() && x.Id != publisher.Id);
            if (publishers.Any())
            {
                throw new ModelException("Publisher with current name is already exist");
            }
        }
    }
}
