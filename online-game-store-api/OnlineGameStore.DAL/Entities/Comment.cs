using System;
using System.Collections.Generic;

namespace OnlineGameStore.DAL.Entities
{
    public class Comment : BaseEntity
    {
        public string Name { get; set; }
        public string Body { get; set; }
        public bool IsQuoted { get; set; }

        public Guid GameId { get; set; }
        public virtual Game Game { get; set; }

        public Guid? ParentCommentId { get; set; }
        public virtual Comment ParentComment { get; set; }

        public virtual ICollection<Comment> ChildComments { get; set; }
    }
}