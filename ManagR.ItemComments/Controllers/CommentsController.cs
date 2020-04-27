using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItemComments.Models;
using ItemComments.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManagR.ItemComments.Controllers
{
    [EnableCors("ManagRAppServices")]
    [Authorize(Policy = "spectator")]
    public class CommentsController : ControllerBase
    {
        private ICommentsRepository _commentsRepository;

        public CommentsController(ICommentsRepository commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentDto comment)
        {
            var success = await _commentsRepository.CreateComment(comment);
            return Ok(success);
        }

        [HttpGet]
        public async Task<IActionResult> GetComments (Guid id)
        {
            var comments = await _commentsRepository.GetComments(id);
            return Ok(comments);
        }
    }
}