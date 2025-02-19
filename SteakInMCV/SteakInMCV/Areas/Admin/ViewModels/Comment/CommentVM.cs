using System;
using SteakInMCV.Models.Enum;

namespace SteakInMCV.Areas.Admin.ViewModels.Comment
{
	public class CommentVM
	{
        public int EventId { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public CommentStatus Status { get; set; }
        public List<CommentReplyVM> Replies { get; set; }
    }
}

