using System.ComponentModel.DataAnnotations;

namespace ResourceWatcher.Models
{
    public class ImageViewModel
    {
        [Required]
        [Display(Name = "Images")]
        public List<ImageModel> Images { get; set; } = new List<ImageModel>();
    }
}