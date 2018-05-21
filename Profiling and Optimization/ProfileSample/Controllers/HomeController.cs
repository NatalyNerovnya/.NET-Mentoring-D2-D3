using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ProfileSample.DAL;
using ProfileSample.Models;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProfileSampleEntities _context;

        public HomeController()
        {
            _context = new ProfileSampleEntities();
        }
        
        public ActionResult Index()
        {
            var model = new List<ImageModel>();
            var sources = _context.ImgSources.Take(20);

            foreach (var item in sources.ToList())
            {
                model.Add(new ImageModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Data = item.Data
                });
            }

            return View(model);
        }

        public ActionResult Convert()
        {
            var files = Directory.GetFiles(Server.MapPath("~/Content/Img"), "*.jpg");

            using (_context)
            {
                foreach (var file in files)
                {
                    using (var stream = new FileStream(file, FileMode.Open))
                    {
                        byte[] buff = new byte[stream.Length];

                        stream.Read(buff, 0, (int)stream.Length);

                        _context.ImgSources.Add(new ImgSource()
                        {
                            Name = Path.GetFileName(file),
                            Data = buff
                        });
                    }
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [OutputCache(Duration = 30, VaryByParam = "id")]
        public ActionResult GetImageById(int id)
        {
            var item = _context.ImgSources.First(s => s.Id == id);

            return File(item.Data, "image");

            //var img = new WebImage(item.Data)
            //    .Resize(300, 150, false, true);
            //return File(new MemoryStream(img.GetBytes()), "binary/octet-stream");
        }
    }
}