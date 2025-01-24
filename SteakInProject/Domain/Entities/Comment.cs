using System;
using System.Text.Json.Serialization;
using Domain.Common;
using Domain.Entities;
using Domain.Enum;

namespace Domain.Entities
{
    public class Comment :BaseEntity
	{
        public int EventId { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; } 
        public CommentStatus Status { get; set; } = CommentStatus.Pending;
        public List<CommentReply> CommentReplies { get; set; } 
        public Event Event { get; set; }
    }
}

