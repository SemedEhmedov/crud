using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.DAL;
using WebApplication4.Models;


namespace WebApplication4.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ProductController : Controller
    {
        AppDBContext context;
        public ProductController(AppDBContext appDBcontext)
        {
            context = appDBcontext;
        }
        public async Task<IActionResult> Index()
        {
            var products = await context.Products.Include(x=>x.ProductImages).ToListAsync();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
    
            context.Products.Add(product);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var product = context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return BadRequest();
            }
            context.Products.Remove(product);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var product = context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return BadRequest();
            }
            return View(product);
        }
        [HttpPost]
        public IActionResult Update(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(product);
            }
            if (product.Id== null)
            {
                return BadRequest();
            }
            var oldproduct = context.Products.FirstOrDefault(x=>x.Id==product.Id);
            if(oldproduct == null)
            {
                return BadRequest();
            }
            oldproduct.Name = product.Name;
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
