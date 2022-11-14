using AutoMapper;
using Newtonsoft.Json;
using OnlineGameStore.BLL.CustomExceptions;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.DAL.DTOs;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Pipelines.PaginationPipelineSteps;
using OnlineGameStore.DAL.Pipelines.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GameService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GameModel> CreateAsync(GameModel newGame)
        {
            await CheckIsGameUnique(newGame);
            newGame.Id = Guid.NewGuid();
            Game gameEntity = _mapper.Map<Game>(newGame);

            gameEntity.MongoGenreId = null;
            gameEntity.DownloadPath = newGame.IsDownloaded ? "Resources/Games/" + gameEntity.Name + gameEntity.Id : null;

            gameEntity = await _unitOfWork.GameRepository.CreateAsync(gameEntity);
            return _mapper.Map<GameModel>(gameEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.GameRepository.SoftDeleteAsync(id);
        }

        public async Task EditViewNumberAsync(string gamekey)
        {
            Game game = await _unitOfWork.GameRepository.FindAsync(x => x.Key == gamekey);

            if (game != null)
            {
                game.ViewsNumber++;
                await _unitOfWork.GameRepository.EditFieldAsync(game, nameof(game.ViewsNumber));
            }
        }

        public async Task<GameModel> EditAsync(GameModel updatedGame)
        {
            await CheckIsGameUnique(updatedGame);

            Game gameEntity = _mapper.Map<Game>(updatedGame);

            Game updatedGameEntity = await _unitOfWork.GameRepository.EditAsync(gameEntity);

            if (updatedGameEntity == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<GameModel>(updatedGameEntity);
        }

        public async Task<IEnumerable<GameModel>> GetAllAsync()
        {
            var games = await _unitOfWork.GameRepository.GetAllAsync();
            var gamesWithUniqueKey = games.GroupBy(x => x.Key).Select(x => x.First());
            return _mapper.Map<IEnumerable<GameModel>>(gamesWithUniqueKey);
        }

        public async Task<FilteredGameModel> GetByFilter(string gameFilter)
        {
            if (!string.IsNullOrEmpty(gameFilter))
            {
                var filter = JsonConvert.DeserializeObject<GamesFilterComponents>(gameFilter);

                var games = (await _unitOfWork.GameRepository.GetAllAsync()).GroupBy(x => x.Key).Select(x => x.First());

                foreach (var game in games)
                {
                    if (game.PublisherId.HasValue)
                    {
                        game.Publisher = await _unitOfWork.PublisherRepository.FindAsync(x => x.Id == game.PublisherId.Value);
                    }
                    if (game.MongoGenreId.HasValue && game.MongoGenreId != Guid.Empty)
                    {
                        game.Genres = new List<GameGenre>() { new GameGenre() { GameId = game.Id, GenreId = game.MongoGenreId.Value } };
                    }
                }

                List<Game> allEntites = new GamePipeline(filter).Process(games.ToList());

                return new FilteredGameModel()
                {
                    Games = _mapper.Map<List<GameModel>>(new PaginationPipeline(filter.Pagination).Process(allEntites)),
                    AllGameNumber = allEntites.Count()
                };
            }

            return new FilteredGameModel()
            {
                Games = await GetAllAsync(),
                AllGameNumber = await GetGameNumberAsync()
            };
        }

        public async Task<GameModel> GetByKeyAsync(string key)
        {
            Game gameEntity = await _unitOfWork.GameRepository.FindAsync(g => g.Key == key);
            if (gameEntity == null)
            {
                throw new NotFoundException("Cannot find game");
            }

            if (gameEntity.PublisherId.HasValue)
            {
                gameEntity.Publisher = await _unitOfWork.PublisherRepository.FindAsync(x => x.Id == gameEntity.PublisherId.Value);
            }

            GameModel gameModel = _mapper.Map<GameModel>(gameEntity);

            if (gameEntity.MongoGenreId.HasValue && gameEntity.MongoGenreId != Guid.Empty)
            {
                gameModel.Genres = _mapper.Map<List<GenreModel>>((await _unitOfWork.GenreRepository.GetAllAsync(x => x.Id == gameEntity.MongoGenreId.Value)).ToList());
            }

            return gameModel;
        }

        public async Task<GameModel> GetByIdAsync(Guid id)
        {
            Game gameEntity = await _unitOfWork.GameRepository.FindAsync(x => x.Id == id);
            if (gameEntity == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<GameModel>(gameEntity);
        }

        public async Task<int> GetGameNumberAsync()
        {
            var games = await GetAllAsync();
            return games.Count();
        }

        private async Task CheckIsGameUnique(GameModel game)
        {
            var games = await _unitOfWork.GameRepository.GetWhereAsNoTrackingAsync(x => game.Key.ToLower() == x.Key.ToLower() && game.Id != x.Id);
            if (games.Any())
            {
                throw new ModelException("Game with current key is already exist");
            }
        }
    }
}
