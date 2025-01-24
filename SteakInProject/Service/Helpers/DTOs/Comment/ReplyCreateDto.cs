using System;
namespace Service.Helpers.DTOs.Comment
{
	public class ReplyCreateDto
	{
        public int CommentId { get; set; }  
        public string AuthorName { get; set; } 
        public string Content { get; set; } 
    }
}

