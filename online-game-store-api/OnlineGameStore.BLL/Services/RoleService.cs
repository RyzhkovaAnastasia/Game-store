using AutoMapper;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models.AuthModels;
using OnlineGameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IAuthUnitOfWork _authUnitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IAuthUnitOfWork authUnitOfWork, IMapper mapper)
        {
            _authUnitOfWork = authUnitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<RoleModel> GetAll()
        {
            var roles = _authUnitOfWork.RoleManager.Roles;
            return _mapper.Map<IEnumerable<RoleModel>>(roles);
        }

        public async Task<RoleModel> GetByIdAsync(Guid id)
        {
            var role = await _authUnitOfWork.RoleManager.FindByIdAsync(id.ToString());
            return _mapper.Map<RoleModel>(role);
        }
    }
}
