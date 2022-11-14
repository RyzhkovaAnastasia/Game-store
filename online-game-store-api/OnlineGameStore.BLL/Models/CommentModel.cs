using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.Models
{
    public class CommentModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Body { get; set; }
        public bool IsQuoted { get; set; }

        public Guid GameId { get; set; }

        public Guid? ParentCommentId { get; set; }
        public string ParentCommentName { get; set; }
        public string ParentCommentBody { get; set; }

        public IEnumerable<CommentModel> ChildComments { get; set; }
    }
}
