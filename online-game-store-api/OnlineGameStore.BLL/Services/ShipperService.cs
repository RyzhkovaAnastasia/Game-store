using AutoMapper;
using OnlineGameStore.BLL.CustomExceptions;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Services
{
    public class ShipperService : IShipperService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ShipperService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ShipperModel>> GetAllAsync()
        {
            var shipperEntities = await _unitOfWork.ShipperRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ShipperModel>>(shipperEntities);
        }

        public async Task<ShipperModel> GetByIdAsync(Guid id)
        {
            DAL.Entities.Shipper shipperEntities = await _unitOfWork.ShipperRepository.FindAsync(x => x.Id == id);
            if (shipperEntities == null)
            {
                throw new NotFoundException();
            }
            return _mapper.Map<ShipperModel>(shipperEntities);
        }
    }
}
