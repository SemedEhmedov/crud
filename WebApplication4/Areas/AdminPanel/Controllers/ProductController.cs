using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Areas.AdminPanel.ViewModels.Product;
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
            var products = await context.Products.Include(x=>x.Category).Include(x=>x.TagProducts).ThenInclude(x=>x.Tag).ToListAsync();
            return View(products);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Tags = context.Tags.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProduct vm)
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Tags = context.Tags.ToList();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (vm.CategoryId != null)
            {
                if (!context.Categories.Any(x=>x.Id==vm.CategoryId))
                {
                    ModelState.AddModelError("CategoryId",$"{vm.CategoryId} li category yoxdu");
                    return View();
                }
            }
            Product product = new Product()
            {
                Name = vm.Name,
                CategoryId = vm.CategoryId,
                Description = vm.Description,
                Price = vm.Price
            };
            if(vm.TagIds != null)
            {
                foreach(var tagId in vm.TagIds)
                {
                    if(!await context.Tags.AnyAsync(x => x.Id == tagId))
                    {
                        ModelState.AddModelError("TagIds", $"{tagId} id li tag yoxdur");
                        return View();
                    }
                    TagProduct tagProduct = new TagProduct()
                    {
                        TagId = tagId,
                        ProductId = product.Id
                    };
                    context.TagProducts.Add(tagProduct);
                }
            }
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
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
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var product = await context.Products.Include(x => x.Category).Include(x => x.TagProducts).ThenInclude(x => x.Tag).FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Tags = context.Tags.ToList();
            if (product == null)
            {
                return BadRequest();
            }

            UpdateProductVM vm = new UpdateProductVM()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId

            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductVM vm)
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Tags = context.Tags.ToList();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (vm.Id== null || !(context.Products.Any(x=>x.Id==vm.Id)) )
            {
                return BadRequest();
            }
            if (vm.CategoryId != null)
            {
                if (!context.Categories.Any(x => x.Id == vm.CategoryId))
                {
                    ModelState.AddModelError("CategoryId", $"{vm.CategoryId} li category yoxdu");
                    return View();
                }
            }
            Product oldproduct = context.Products.FirstOrDefault(x=>x.Id==vm.Id);
            if (vm.TagIds != null)
            {
                foreach (var tagId in vm.TagIds)
                {
                    if (!await context.Tags.AnyAsync(x => x.Id == tagId))
                    {
                        ModelState.AddModelError("TagIds", $"{tagId} id li tag yoxdur");
                        return View();
                    }
                    TagProduct tagProduct = new TagProduct()
                    {
                        TagId = tagId,
                        ProductId = oldproduct.Id
                    };
                    context.TagProducts.Add(tagProduct);
                }
            }
            if (oldproduct == null)
            {
                return NotFound();
            }
            oldproduct.Name = vm.Name;
            oldproduct.Description = vm.Description;
            oldproduct.Price = vm.Price;
            oldproduct.CategoryId = vm.CategoryId;

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
