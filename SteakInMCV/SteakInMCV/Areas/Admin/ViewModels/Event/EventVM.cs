using System;
using SteakInMCV.Areas.Admin.ViewModels.Comment;

namespace SteakInMCV.Areas.Admin.ViewModels.Event
{
	public class EventVM
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string ImgUrl { get; set; }
        public string Image { get; set; }
        public string Info { get; set; }
        public List<string> Tags { get; set; }
        public List<CommentVM> Comments { get; set; }

    }
}

