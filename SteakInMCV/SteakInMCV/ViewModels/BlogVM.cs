using System;
using SteakInMCV.ViewModels.Events;

namespace SteakInMCV.ViewModels
{
	public class BlogVM
	{
        public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
        public IEnumerable<EventVM> EventVMs { get; set; } = new List<EventVM>();

        public EventVM EventVM { get; set; }
        //public CommentVM CommentVM { get; set; }
        public List<CommentVM> Comments { get; internal set; }
    }
}

