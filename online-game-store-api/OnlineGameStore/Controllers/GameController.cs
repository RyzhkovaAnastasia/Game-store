using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.Common.Enums;
using OnlineGameStore.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OnlineGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly UserClaims _userClaims;

        public GameController(IWebHostEnvironment appEnvironment, IGameService gameService, UserClaims userClaims)
        {
            _gameService = gameService;
            _appEnvironment = appEnvironment;
            _userClaims = userClaims;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _gameService.GetAllAsync());
        }

        [HttpGet("filter/{filter}")]
        public async Task<IActionResult> GetAllAsync(string filter)
        {
            return Ok(await _gameService.GetByFilter(filter));
        }

        [HttpGet("{gamekey}")]
        public async Task<IActionResult> GetGameByKeyAsync(string gamekey)
        {
            var game = await _gameService.GetByKeyAsync(gamekey);
            return Ok(game);
        }

        [HttpGet("gameNumber")]
        public async Task<IActionResult> GetGamesNumber()
        {
            int gameNumber = await _gameService.GetGameNumberAsync();
            return Ok(gameNumber);
        }

        [HttpPost]
        [RoleAuthorize(Role.Administrator, Role.Manager)]
        public async Task<IActionResult> CreateAsync(GameModel newGame)
        {
            var game = await _gameService.CreateAsync(newGame);
            return Ok(game);
        }

        [HttpPut]
        [RoleAuthorize(Role.Administrator, Role.Manager, Role.Publisher)]
        public async Task<IActionResult> EditAsync(GameModel updatedGame)
        {
            var oldGame = await _gameService.GetByIdAsync(updatedGame.Id);
            if (_userClaims.IsUserHasAccess(oldGame.PublisherId.GetValueOrDefault()))
            {
                GameModel game = await _gameService.EditAsync(updatedGame);
                return Ok(game);
            }
            return Unauthorized();
        }

        [HttpPut("incviewnumber/{gamekey}")]
        public async Task<IActionResult> EditViewNumberAsync([FromRoute] string gameKey)
        {
            await _gameService.EditViewNumberAsync(gameKey);
            return Ok();
        }

        [HttpDelete("{id}")]
        [RoleAuthorize(Role.Administrator, Role.Manager)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _gameService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("{gamekey}/download")]
        public IActionResult Download(string gamekey)
        {
            string filePath = Path.Combine(_appEnvironment.ContentRootPath, "Games/TestGameFile.bin");
            string fileType = "application/octet-stream";
            string fileName = "game.bin";

            return PhysicalFile(filePath, fileType, fileName);
        }
    }
}
