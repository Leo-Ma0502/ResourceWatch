using System;
using System.ComponentModel.DataAnnotations;

namespace ResourceWatcher.Models
{
    public class ImageModel
    {
        public int Id { get; set; }

        [Required]
        [Url]
        public string? Type { get; set; }

        [Required]
        public string? Timestamp { get; set; }

        [Required]
        [Url]
        public string? Url { get; set; }

    }
}
