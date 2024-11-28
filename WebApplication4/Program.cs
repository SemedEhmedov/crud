using Microsoft.EntityFrameworkCore;
using WebApplication4.DAL;

namespace WebApplication4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDBContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("default"));
            });
            var app = builder.Build();




            app.MapControllerRoute(
               name: "areas",
               pattern: "{area:exists}/{controller=DashBoard}/{action=index}/{id?}"
               );
            app.MapControllerRoute(
                 name: "default",
                 pattern: "{controller=home}/{action=index}/{id?}"
                 );
            app.UseStaticFiles();
            app.Run();
        }
    }
}
