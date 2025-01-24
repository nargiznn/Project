using System;
using Domain.Common;
using Domain.Enum;

namespace Service.Helpers.DTOs.Comment
{
	public class ReplyDto
	{
        public int Id { get; set; }
        public int CommentId { get; set; }  
        public string AuthorName { get; set; } 
        public string Content { get; set; } 
        public ReplyStatus Status { get; set; }
    }
}

