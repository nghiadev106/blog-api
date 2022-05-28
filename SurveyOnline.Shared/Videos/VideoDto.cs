using System;

namespace SurveyOnline.Shared.Videos
{
    public class VideoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CategoryId { get; set; }
        public string Link { get; set; }
        public string CategoryName { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsHot { get; set; }
        public string CreateBy { get; set; }
        public string Url { get; set; }
    }
}
