using System;
using SteakInMCV.Models;
using SteakInMCV.Models.Enum;

namespace SteakInMCV.ViewModels
{
	public class CommentVM:BaseEntity
	{
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public CommentStatus Status { get; set; }
        public List<ReplyVM> Replies { get; set; }
    }
}

