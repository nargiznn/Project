using System;
namespace SteakInMCV.Areas.Admin.ViewModels.Faq
{
    public class FaqVM
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

