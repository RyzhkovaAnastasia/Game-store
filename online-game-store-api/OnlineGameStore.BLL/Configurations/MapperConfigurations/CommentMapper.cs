using AutoMapper;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Configurations.MapperConfigurations
{
    public class CommentMapper : Profile
    {
        public CommentMapper()
        {
            CreateMap<Comment, CommentModel>()
                .ForMember(c => c.ParentCommentName, opt => opt.MapFrom(c => c.ParentComment.Name))
                .ForMember(c => c.ParentCommentBody, opt => opt.MapFrom(c => c.ParentComment.IsDeleted ? null : c.ParentComment.Body))
                .ForMember(c => c.Body, opt => opt.MapFrom(c => c.IsDeleted ? null : c.Body));

            CreateMap<CommentModel, Comment>()
                .ForMember(c => c.ParentCommentId, opt => opt.MapFrom(c => c.ParentCommentId))
                .AfterMap((src, dest) => dest.ParentComment = null)
                .AfterMap((src, dest) => dest.ChildComments = null);
        }
    }
}
