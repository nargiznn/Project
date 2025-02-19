using System;
using SteakInMCV.Models.Enum;

namespace SteakInMCV.Areas.Admin.ViewModels.Comment
{
	public class CommentReplyVM
	{
        public int Id { get; set; }
        public int CommentId { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public ReplyStatus Status { get; set; }
    }
}

