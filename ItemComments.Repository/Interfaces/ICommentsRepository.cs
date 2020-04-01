using ItemComments.Models;
using ItemComments.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ItemComments.Repository.Interfaces
{
    public interface ICommentsRepository
    {
        public Task<bool> CreateComment(CommentDto comment);

        public Task<List<CommentVm>> GetComments(Guid itemId);
    }
}
