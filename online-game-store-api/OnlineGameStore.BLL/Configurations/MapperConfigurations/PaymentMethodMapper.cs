using AutoMapper;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Configurations.MapperConfigurations
{
    public class PaymentMethodMapper : Profile
    {
        public PaymentMethodMapper()
        {
            CreateMap<PaymentMethod, PaymentMethodModel>().ReverseMap();
        }
    }
}
