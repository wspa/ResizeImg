using RecorteImg.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace RecorteImg.Controllers
{
    public class RecorteController : Controller
    {
        // GET: Recorte
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);

                var path = "";                
                System.Drawing.Image sourceimage = System.Drawing.Image.FromStream(file.InputStream);

                //original
                path = Path.Combine(Server.MapPath("~/App_Data/uploads"), "1_"+fileName);
                //ImageUtilities.SaveJpeg(path, sourceimage, 100);
                ImageUtilities.SavePng(path, sourceimage, 100);

                using (var resized = ImageUtilities.ResizeImage(sourceimage, (sourceimage.Width/2), (sourceimage.Height/2)))
                {
                    path = Path.Combine(Server.MapPath("~/App_Data/uploads"), "2_" + fileName);
                    //ImageUtilities.SaveJpeg(path, resized, 100);                    
                    ImageUtilities.SavePng(path, resized, 100);
                }

                using (var resized = ImageUtilities.ResizeImage(sourceimage, (sourceimage.Width / 4), (sourceimage.Height / 4)))
                {
                    path = Path.Combine(Server.MapPath("~/App_Data/uploads"), "3_" + fileName);
                    //ImageUtilities.SaveJpeg(path, resized, 100);
                    ImageUtilities.SavePng(path, resized, 100);
                }

                //file.SaveAs(path);
            }
            return RedirectToAction("Index");
        }
    }
}