using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTCL.Core.Models;
using YTCL.Infrastructure.Data;

namespace YTCL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VideosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Videos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VideoDto>>> GetVideos(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 12,
            [FromQuery] int? categoryId = null,
            [FromQuery] int? locationId = null,
            [FromQuery] string? searchQuery = null)
        {
            var query = _context.Videos
                .Include(v => v.Publisher)
                .Include(v => v.Category)
                .Include(v => v.Location)
                .Where(v => v.Status == VideoStatus.Published);

            if (categoryId.HasValue)
            {
                query = query.Where(v => v.CategoryId == categoryId.Value);
            }

            if (locationId.HasValue)
            {
                query = query.Where(v => v.LocationId == locationId.Value);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(v => 
                    v.Title.Contains(searchQuery) || 
                    v.Abstract.Contains(searchQuery));
            }

            var totalCount = await query.CountAsync();
            
            var videos = await query
                .OrderByDescending(v => v.UploadDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(v => new VideoDto
                {
                    Id = v.Id,
                    Title = v.Title,
                    Abstract = v.Abstract,
                    ThumbnailUrl = v.ThumbnailUrl,
                    VideoUrl = v.VideoUrl,
                    UploadDate = v.UploadDate,
                    PublisherId = v.PublisherId,
                    PublisherName = v.Publisher.Username,
                    CategoryId = v.CategoryId,
                    CategoryName = v.Category.Name,
                    LocationId = v.LocationId,
                    LocationName = v.Location.Name
                })
                .ToListAsync();

            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            Response.Headers.Add("X-Page-Count", ((int)Math.Ceiling(totalCount / (double)pageSize)).ToString());

            return videos;
        }

        // GET: api/Videos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoDto>> GetVideo(int id)
        {
            var video = await _context.Videos
                .Include(v => v.Publisher)
                .Include(v => v.Category)
                .Include(v => v.Location)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (video == null)
            {
                return NotFound();
            }

            return new VideoDto
            {
                Id = video.Id,
                Title = video.Title,
                Abstract = video.Abstract,
                ThumbnailUrl = video.ThumbnailUrl,
                VideoUrl = video.VideoUrl,
                UploadDate = video.UploadDate,
                PublisherId = video.PublisherId,
                PublisherName = video.Publisher.Username,
                CategoryId = video.CategoryId,
                CategoryName = video.Category.Name,
                LocationId = video.LocationId,
                LocationName = video.Location.Name
            };
        }

        // POST: api/Videos
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<VideoDto>> PostVideo(VideoCreateDto videoDto)
        {
            // In a real app, we would get the user ID from the authenticated user
            var userId = 1; // Mock user ID for demo

            var video = new Video
            {
                Title = videoDto.Title,
                Abstract = videoDto.Abstract,
                ThumbnailUrl = videoDto.ThumbnailUrl,
                VideoUrl = videoDto.VideoUrl,
                UploadDate = DateTime.UtcNow,
                Status = VideoStatus.Published,
                PublisherId = userId,
                CategoryId = videoDto.CategoryId,
                LocationId = videoDto.LocationId
            };

            _context.Videos.Add(video);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVideo), new { id = video.Id }, new VideoDto
            {
                Id = video.Id,
                Title = video.Title,
                Abstract = video.Abstract,
                ThumbnailUrl = video.ThumbnailUrl,
                VideoUrl = video.VideoUrl,
                UploadDate = video.UploadDate,
                PublisherId = video.PublisherId,
                CategoryId = video.CategoryId,
                LocationId = video.LocationId
            });
        }
    }

    public class VideoDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Abstract { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
        public int PublisherId { get; set; }
        public string? PublisherName { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
    }

    public class VideoCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Abstract { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int LocationId { get; set; }
    }
}