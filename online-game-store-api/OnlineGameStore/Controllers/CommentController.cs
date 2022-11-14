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
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{gamekey}")]
        public async Task<IActionResult> GetCommentsByGameKeyAsync(string gamekey)
        {
            var comments = await _commentService.GetByGameKeyAsync(gamekey);
            return Ok(comments);
        }

        [HttpGet("{gamekey}/{id}")]
        public async Task<IActionResult> GetCommentByIdAsync(string gamekey, Guid id)
        {
            CommentModel comment = await _commentService.GetByIdAsync(id);
            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CommentModel newComment)
        {
            CommentModel comment = await _commentService.CreateAsync(newComment);
            return Ok(comment);
        }


        [HttpDelete("{id}")]
        [RoleAuthorize(Role.Administrator, Role.Manager, Role.Moderator)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _commentService.DeleteAsync(id);
            return Ok();
        }
    }
}
