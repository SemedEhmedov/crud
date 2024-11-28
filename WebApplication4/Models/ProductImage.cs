using WebApplication4.Models.Base;

namespace WebApplication4.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public bool Primary { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
