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
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GenreModel> CreateAsync(GenreModel newGenre)
        {
            await CheckGenreUnique(newGenre);

            Genre platfornTypeEntity = _mapper.Map<Genre>(newGenre);
            Genre newPlatformEntity = await _unitOfWork.GenreRepository.CreateAsync(platfornTypeEntity);
            return _mapper.Map<GenreModel>(newPlatformEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.GenreRepository.SoftDeleteAsync(id);
        }

        public async Task<GenreModel> EditAsync(GenreModel updatedGenre)
        {
            await CheckGenreUnique(updatedGenre);

            Genre genreEntity = _mapper.Map<Genre>(updatedGenre);
            genreEntity = await _unitOfWork.GenreRepository.EditAsync(genreEntity);

            if (genreEntity == null)
            {
                throw new NotFoundException();
            }

            return updatedGenre;
        }

        public async Task<IEnumerable<GenreModel>> GetAllAsync()
        {
            IEnumerable<Genre> genreEntities = await _unitOfWork.GenreRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GenreModel>>(genreEntities);
        }

        public async Task<GenreModel> GetByIdAsync(Guid id)
        {
            Genre genreEntity = await _unitOfWork.GenreRepository.FindAsync(x => x.Id == id);
            if (genreEntity == null)
            {
                throw new NotFoundException();
            }
            return _mapper.Map<GenreModel>(genreEntity);
        }

        private async Task CheckGenreUnique(GenreModel genre)
        {
            var genres = await _unitOfWork.GenreRepository.GetWhereAsNoTrackingAsync(x => x.Id != genre.Id && x.Name.ToLower() == genre.Name.ToLower());

            if (genres.Any())
            {
                throw new ModelException("Genre name must be unique");
            }
        }
    }
}
