using AutoMapper;
using OnlineGameStore.BLL.Models.AuthModels;
using OnlineGameStore.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OnlineGameStore.BLL.Configurations.MapperConfigurations
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserModel>()
                .ForMember(x => x.Role, opt => opt.MapFrom(x => x.Roles.FirstOrDefault()));

            CreateMap<UserModel, User>()
                .ForMember(x => x.Roles, opt => opt.MapFrom(x => new List<string>() { x.Role }));

            CreateMap<Role, RoleModel>().ReverseMap();
        }
    }
}
