using System.Collections.Generic;

namespace YTCL.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public ICollection<Video> Videos { get; set; } = new List<Video>();
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Video> Videos { get; set; } = new List<Video>();
    }

    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public LocationType Type { get; set; }
        public int? ParentId { get; set; }
        public Location? Parent { get; set; }
        public ICollection<Location> Children { get; set; } = new List<Location>();
        public ICollection<Video> Videos { get; set; } = new List<Video>();
    }

    public enum LocationType
    {
        Continent,
        Country,
        Region,
        City
    }

    public class VideoTag
    {
        public int VideoId { get; set; }
        public Video Video { get; set; } = null!;
        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;
    }

    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<VideoTag> VideoTags { get; set; } = new List<VideoTag>();
    }
}