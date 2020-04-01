using ItemComments.Data;
using ItemComments.Models;
using ItemComments.Models.ViewModels;
using ItemComments.Repository.Interfaces;
using ItemComments.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        public CommentsRepository(ItemCommentsDb context, ICommenterService commenterService)
        {
            _context = context;
            _commenterService = commenterService;
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
                // exception creating comment
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
                    }).ToListAsync();

                var tasks = new List<Task<bool>>();
                foreach (var comment in comments)
                {
                    tasks.Add(_commenterService.GetCommenter(comment));
                }
                Task.WhenAll(tasks).Wait();
                return comments;
            }
            catch (Exception e)
            {
                // excpetion getting comments
            }
            return null;
        }
    }
}
