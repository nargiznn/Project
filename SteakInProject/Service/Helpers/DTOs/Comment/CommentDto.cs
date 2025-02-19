using System;
using Domain.Entities;
using Domain.Enum;
using System.Text.Json.Serialization;
using Domain.Common;

namespace Service.Helpers.DTOs.Comment
{
	public class CommentDto:BaseEntity
	{
        public int EventId { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public CommentStatus Status { get; set; }
        public List<CommentReplyDto> Replies { get; set; }

    }
}
