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
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CommentModel> CreateAsync(CommentModel newComment)
        {
            Comment commentEntity = _mapper.Map<Comment>(newComment);
            var result = await _unitOfWork.CommentRepository.CreateAsync(commentEntity);
            return _mapper.Map<CommentModel>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.CommentRepository.SoftDeleteAsync(id);
        }

        public async Task<CommentModel> GetByIdAsync(Guid id)
        {
            Comment commentEntity = await _unitOfWork.CommentRepository.FindAsync(comment => comment.Id == id);
            return _mapper.Map<CommentModel>(commentEntity);
        }

        public async Task<IEnumerable<CommentModel>> GetByGameKeyAsync(string key)
        {
            Game game = await _unitOfWork.GameRepository.FindAsync(game => game.Key == key);
            IEnumerable<CommentModel> commentModels =
                _mapper.Map<IEnumerable<CommentModel>>(game.Comments.Where(x => x.ParentCommentId == null));

            if (!_mapper.Map<GameModel>(game).IsCommented)
            {
                throw new ForrbidenException("Cannot load comments");
            }
            return commentModels;
        }
    }
}
