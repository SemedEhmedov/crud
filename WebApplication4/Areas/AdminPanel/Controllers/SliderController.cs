using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.DAL;
using WebApplication4.Helpers.Extensions;
using WebApplication4.Models;

namespace WebApplication4.Areas.AdminPanel.Controllers
{
   
    [Area("AdminPanel")]
    public class SliderController : Controller
    {
        AppDBContext context;
        private readonly IWebHostEnvironment env;

        public SliderController(AppDBContext appDBContext,IWebHostEnvironment env)
        {
            context = appDBContext;
            this.env = env;
        }
        public async Task<IActionResult> Index()
        {
            var sliders = await context.Sliders.ToListAsync();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (slider.File == null)
            {
                ModelState.AddModelError("File", "Fayl secilmeyib.");
                return View();
            }
            if (!slider.File.ContentType.Contains("image"))
            {
                ModelState.AddModelError("File","fayl formati sehvdir");
                return View();
            }
            if(slider.File.Length > 2097152)
            {
                ModelState.AddModelError("File", "sekilin olcusu 2mb dan cox ola bilmez");
                return View();  
            }
            slider.ImgUrl = slider.File.Upload(env.WebRootPath,"Upload\\Slider");

            if (!ModelState.IsValid)
            {
                return View();
            }
            context.Sliders.Add(slider);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return View();
            }
            var slider = context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider == null)
            {
                return NotFound();
            }
            FileExtensions.DeleteFile(env.WebRootPath,"Upload\\Slider",slider.ImgUrl);
            context.Sliders.Remove(slider);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            if (id == null)
            {
                return View();
            }
            var slider = context.Sliders.FirstOrDefault(s=>s.Id == id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }
        [HttpPost]
        public IActionResult Update(Slider slider)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            var oldslider = context.Sliders.FirstOrDefault(s=>s.Id==slider.Id);
            if (oldslider == null)
            {
                return NotFound();
            }
            oldslider.Title = slider.Title;
            oldslider.SubTitle = slider.SubTitle;
            oldslider.Offer = slider.Offer;
            oldslider.ImgUrl =  slider.File.Upload(env.WebRootPath, "Upload\\Slider"); ;
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
