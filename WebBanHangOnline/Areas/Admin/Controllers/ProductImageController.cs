using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductImageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ProductImage
        public ActionResult Index(int productId)
        {
            ViewBag.ProductId = productId;
            var items = db.ProductImages.Where(x => x.ProductId == productId).ToList();
            return View(items);
        }
        [HttpPost]
        public ActionResult AddImage(int productId, string url) 
        {
            db.ProductImages.Add(new ProductImage
            {
                ProductId = productId,
                Image = url,
                IsDefault = false
            });
            db.SaveChanges();
            return Json(new { success = true });
        }
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            var item = db.ProductImages.Find(Id);
            db.ProductImages.Remove(item);
            db.SaveChanges();
            return Json(new { success = true });
        }
    }
}