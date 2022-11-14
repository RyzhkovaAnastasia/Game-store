using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.Common.Enums;
using System;
using System.Threading.Tasks;

namespace OnlineGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [RoleAuthorize(Role.Administrator, Role.Manager)]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _genreService.GetAllAsync();
            return Ok(genres);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var genre = await _genreService.GetByIdAsync(id);
            return Ok(genre);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenreModel newGenre)
        {
            var genre = await _genreService.CreateAsync(newGenre);
            return Ok(genre);
        }

        [HttpPut]
        public async Task<IActionResult> EditAsync(GenreModel updatedGenre)
        {
            var genre = await _genreService.EditAsync(updatedGenre);
            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _genreService.DeleteAsync(id);
            return Ok();
        }
    }
}
