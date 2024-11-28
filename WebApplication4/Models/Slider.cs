using System.ComponentModel.DataAnnotations.Schema;
using WebApplication4.Models.Base;

namespace WebApplication4.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string  SubTitle { get; set; }
        public string Offer {  get; set; }
        public string? ImgUrl { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }

    }
}
