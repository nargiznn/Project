using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SteakInMCV.Areas.Admin.ViewModels.Event
{
	public class EventCreateVM
	{
        public string Title { get; set; }
        public string Desc { get; set; }
        public string ImgUrl { get; set; }
        public IFormFile Image { get; set; }
        public string Info { get; set; }
        public List<int> SelectedTags { get; set; }
        public List<SelectListItem> AvailableTags { get; set; }

    }
}



