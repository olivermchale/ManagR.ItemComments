using System;
using System.Collections.Generic;
using System.Text;

namespace ItemComments.Models.ViewModels
{
    public class CommentVm
    {
        public Guid Id { get; set; }
        public Guid CommenterId { get; set; }
        public string CommenterName { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
