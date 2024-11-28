using WebApplication4.Models;

namespace WebApplication4.Areas.AdminPanel.ViewModels.Product
{
    public record UpdateProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int? CategoryId { get; set; }
        public List<int>? TagIds { get; set; }
    }
}
