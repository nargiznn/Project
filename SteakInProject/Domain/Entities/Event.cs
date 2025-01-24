using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Event:BaseEntity
	{
		public string Title { get; set; }
		public string Desc { get; set; }
		public string ImgUrl { get; set; }
        public string Info { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Comment> Comments { get; set; }
        public List<CommentReply> CommentReplies { get; set; }
    }
}

