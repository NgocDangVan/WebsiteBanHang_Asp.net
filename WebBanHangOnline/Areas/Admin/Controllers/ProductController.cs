using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Product
        public ActionResult Index(int? page)
        {
            IEnumerable<Product> items = db.Products.OrderByDescending(x => x.Id);
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        public ActionResult Add()
        {
            Product item = new Product();
            ViewBag.ProductCategoryID = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product model,List<String> Images, List<int> rDefault)
        {
            if (ModelState.IsValid)
            {
                if(Images != null && Images.Count > 0)
                {
                    for(int i = 0; i < Images.Count; i++)
                    {
                        if(i + 1 == rDefault[0])
                        {
                            model.Image = Images[0];
                            model.ProductImage.Add(new ProductImage
                            {
                                ProductId = model.Id,
                                Image = Images[i].ToString(),
                                IsDefault = true,
                            });
                        }
                        else
                        {
                            model.ProductImage.Add(new ProductImage
                            {
                                ProductId = model.Id,
                                Image = Images[i].ToString(),
                                IsDefault = false,
                            });
                        }
                    }
                }
                model.CreatedDate = DateTime.Now;
                model.ModifierDate = DateTime.Now;
                if(!string.IsNullOrEmpty(model.SeoTitle))
                    model.SeoTitle = model.Title;
                if (string.IsNullOrEmpty(model.Alias))
                {
                    model.Alias = WebBanHangOnline.Models.Common.Fillter.FilterChar(model.Title);
                }
                db.Products.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategoryID = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.ProductCategoryID = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            var item = db.Products.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                model.ModifierDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Common.Fillter.FilterChar(model.Title);
                db.Products.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                var checkImg = item.ProductImage.Where(x => x.ProductId == item.Id);
                if(checkImg != null)
                {
                    foreach (var img in checkImg)
                    {
                        db.ProductImages.Remove(img);
                        db.SaveChanges();
                    }
                }
                db.Products.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}