using System;

namespace ItemComments.Models
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid AgileItemId { get; set; }
        public Guid CommenterId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
