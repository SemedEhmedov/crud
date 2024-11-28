using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.DAL;

namespace WebApplication4.Areas.AdminPanel.Controllers
{
    public class DashBoard : Controller
    {
        AppDBContext context;
        public DashBoard(AppDBContext appDBcontext)
        {
            context = appDBcontext;
        }
        [Area("AdminPanel")]
        public async Task<IActionResult> Index()
        {
            var products = await context.Products.Include(x => x.ProductImages).ToListAsync();
            return View(products);
        }
    }
}
