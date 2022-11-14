using AutoMapper;

namespace OnlineGameStore.BLL.Configurations.MapperConfigurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile(IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<CommentMapper>();
            mapperConfigurationExpression.AddProfile<GameMapper>();
            mapperConfigurationExpression.AddProfile<GenreMapper>();
            mapperConfigurationExpression.AddProfile<OrderDetailMapper>();
            mapperConfigurationExpression.AddProfile<OrderMapper>();
            mapperConfigurationExpression.AddProfile<PlatformTypeMapper>();
            mapperConfigurationExpression.AddProfile<PublisherMapper>();
            mapperConfigurationExpression.AddProfile<PaymentMethodMapper>();
            mapperConfigurationExpression.AddProfile<ShipperMapper>();
            mapperConfigurationExpression.AddProfile<UserMapper>();
        }
    }
}
