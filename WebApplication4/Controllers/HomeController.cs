using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        AppDBContext context;
        public HomeController(AppDBContext appDBContext)
        {
            context = appDBContext;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
