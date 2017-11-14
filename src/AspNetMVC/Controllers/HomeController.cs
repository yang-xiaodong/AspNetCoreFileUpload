using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetMVC.ViewModels;
using AspNetMVC.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AspNetMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index([FromServices]AppDbContext dbContext) {
            var model = dbContext.Set<User>()
                .Select(x => new UserViewModel {
                    Name = x.Name,
                    IdNum = x.IdCardNum,
                    IdCardImgName = x.IdCardImgName
                }).ToList();
            return View(model);
        }
      
        public IActionResult New() {
            return View();
        }
    
        [HttpPost]
        public IActionResult New([FromServices]IHostingEnvironment env, [FromServices]AppDbContext dbContext, UserViewModel user) {

            var fileName = Path.Combine("upload", DateTime.Now.ToString("MMddHHmmss") + ".jpg");
            using (var stream = new FileStream(Path.Combine(env.WebRootPath, fileName), FileMode.CreateNew)) {
                user.IdCardImg.CopyTo(stream);
            }

            var users = dbContext.Set<User>();
            var dbUser = new User() {
                Name = user.Name,
                IdCardNum = user.IdNum,
                IdCardImgName = fileName
            };
            users.Add(dbUser);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult About() {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact() {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error() {
            return View();
        }
    }
}
