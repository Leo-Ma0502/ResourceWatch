using System;
using System.ComponentModel.DataAnnotations;

namespace ResourceWatcher.Models
{
    public class ImageModel
    {
        public int Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        [Url]
        public string? Url { get; set; }
    }
}