using ItemComments.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ItemComments.Services.Interfaces
{
    public interface ICommenterService
    {
        Task<bool> GetCommenter(CommentVm comment);
    }
}
