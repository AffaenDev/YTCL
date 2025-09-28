using System;
using System.Collections.Generic;

namespace YTCL.Core.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Abstract { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
        public VideoStatus Status { get; set; }
        public int PublisherId { get; set; }
        public User Publisher { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;
        public ICollection<VideoTag> VideoTags { get; set; } = new List<VideoTag>();
    }

    public enum VideoStatus
    {
        Editing,
        Published
    }
}