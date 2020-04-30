using ItemComments.Data;
using ItemComments.Models;
using ItemComments.Models.ViewModels;
using ItemComments.Repository.Interfaces;
using ItemComments.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemComments.Repository
{
    public class CommentsRepository : ICommentsRepository
    {
        private ItemCommentsDb _context;
        private ICommenterService _commenterService;
        private ILogger<CommentsRepository> _logger;
        public CommentsRepository(ItemCommentsDb context, ICommenterService commenterService, ILogger<CommentsRepository> logger)
        {
            _context = context;
            _commenterService = commenterService;
            _logger = logger;
        }

        public async Task<bool> CreateComment(CommentDto comment)
        {
            try
            {
                comment.CreatedAt = DateTime.Now;
                comment.Id = Guid.NewGuid();
                comment.IsActive = true;
                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception when creating comment, Exception:" + e + "Stack trace:" + e.StackTrace, "comment: " + comment);
            }
            return false;
        }

        public async Task<List<CommentVm>> GetComments(Guid itemId)
        {
            try
            {
                var comments = await _context.Comments.Where(a => a.AgileItemId == itemId && a.IsActive == true)
                    .Select(c => new CommentVm
                    {
                        Comment = c.Comment,
                        CommenterId = c.CommenterId,
                        CommenterName = null,
                        CreatedAt = c.CreatedAt,
                        Id = c.Id,
                        IsActive = c.IsActive
                    }).OrderByDescending(d => d.CreatedAt).ToListAsync();

                // Create a list of tasks (i.e a series of executions)
                // Then add all the tasks to a list
                var tasks = new List<Task<bool>>();
                foreach (var comment in comments)
                {
                    tasks.Add(_commenterService.GetCommenter(comment));
                }
              
                // Asynchronously fire and wait for all tasks to complete, when this has happened all comments have a commenter
                // From communicating with the ManagR identity/authentication microservice
                Task.WhenAll(tasks).Wait();
                return comments;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception when getting comments, Exception:" + e + "Stack trace:" + e.StackTrace, "item: " + itemId);
            }
            return null;
        }
    }
}
