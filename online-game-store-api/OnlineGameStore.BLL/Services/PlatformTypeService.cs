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
    public class PlatformTypeService : IPlatformTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PlatformTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PlatformTypeModel> CreateAsync(PlatformTypeModel newPlatformType)
        {
            await CheckPlatformTypeUnique(newPlatformType);

            PlatformType platfornTypeEntity = _mapper.Map<PlatformType>(newPlatformType);
            PlatformType newPlatformEntity = await _unitOfWork.PlatformTypeRepository.CreateAsync(platfornTypeEntity);
            return _mapper.Map<PlatformTypeModel>(newPlatformEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.PlatformTypeRepository.SoftDeleteAsync(id);
        }

        public async Task<PlatformTypeModel> EditAsync(PlatformTypeModel updatedPlatformType)
        {
            await CheckPlatformTypeUnique(updatedPlatformType);

            var platformTypeEntity = _mapper.Map<PlatformType>(updatedPlatformType);
            platformTypeEntity = await _unitOfWork.PlatformTypeRepository.EditAsync(platformTypeEntity);

            if (platformTypeEntity == null)
            {
                throw new NotFoundException();
            }
            return _mapper.Map<PlatformTypeModel>(platformTypeEntity);
        }

        public async Task<IEnumerable<PlatformTypeModel>> GetAllAsync()
        {
            var platformEntities = await _unitOfWork.PlatformTypeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PlatformTypeModel>>(platformEntities);
        }

        public async Task<PlatformTypeModel> GetByIdAsync(Guid id)
        {
            var platformTypeEntity = await _unitOfWork.PlatformTypeRepository.FindAsync(x => x.Id == id);
            if (platformTypeEntity == null)
            {
                throw new NotFoundException();
            }
            return _mapper.Map<PlatformTypeModel>(platformTypeEntity);
        }

        private async Task CheckPlatformTypeUnique(PlatformTypeModel platform)
        {
            var platforms = await _unitOfWork.PlatformTypeRepository.GetWhereAsNoTrackingAsync(x => x.Id != platform.Id && x.Type.ToLower() == platform.Type.ToLower());
            if (platforms.Any())
            {
                throw new ModelException("Type must be unique");
            }
        }
    }
}
