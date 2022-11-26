using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Feetup.Models;

namespace Feetup.Controllers
{
    public class SellersController : Controller
    {
        private DBfeetEntities db = new DBfeetEntities();





        // GET: Sellers
        public ActionResult Index()
        {
            return View(db.Product.ToList());
        }

        // GET: Sellers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product seller = db.Product.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            return View(seller);
        }

        // GET: Sellers/Create
        public ActionResult Registration()
        {
            return View();
        }

        //// POST: Sellers/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Include = "SellerID,SellerName,SellerPassword,SellerPhone,SellerEmail,BrandName")] Seller seller)
        {
            if (ModelState.IsValid)
            {
                db.Seller.Add(seller);
                db.SaveChanges();
                return RedirectToAction("login");
            }

            return View(seller);
        }

        [HttpGet]
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(Seller avm)
        {

            Seller ad = db.Seller.Where(x => x.SellerName == avm.SellerName && x.SellerPassword == avm.SellerPassword).SingleOrDefault();

            if (ad != null)
            {

                Session["ad_id"] = ad.SellerID.ToString();
                return RedirectToAction("Create");

            }
            else
            {
                ViewBag.error = "Invalid Username or password";
            }

            return View();
        }
        public ActionResult LogOut()
        {
            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("../Home/Index");
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["ad_id"] == null)
            {
                return RedirectToAction("login");
            }
            List<Category> li = db.Category.ToList();
            ViewBag.categorylist = new SelectList(li, "CategoryID", "CategoryName");

            ViewBag.SellerID = new SelectList(db.Seller, "SellerID", "SellerName");

            return View();
        }

        [HttpPost]
        public ActionResult Create(Product cvm, HttpPostedFileBase imgfile)
        {
            Product cat = new Product();
            string path = uploadimagefile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded";
            }
            else
            {

                cat.ProductName = cvm.ProductName;
                cat.ProductDescription = cvm.ProductDescription;
                cat.ProductBrand = cvm.ProductBrand;
                cat.ProductColor = cvm.ProductColor;
                cat.ProductPrice = cvm.ProductPrice;
                cat.CategoryID = cvm.CategoryID;
                cat.SellerID = cvm.SellerID;
                cat.ProductImage = path;
                // cat.cat_fk_ad = Convert.ToInt32(Session["ad_id"].ToString());

                db.Product.Add(cat);

                db.SaveChanges();



                //return RedirectToAction("Create");
            }
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", cvm.CategoryID);
            ViewBag.SellerID = new SelectList(db.Seller, "SellerID", "SellerName", cvm.SellerID);
            return View(cvm);
        }


        public string uploadimagefile(HttpPostedFileBase file)
        {
            Random r = new Random();
            int random = r.Next();
            string path = "-1";
            if (file != null && file.ContentLength > 0)
            {
                string extention = Path.GetExtension(file.FileName);
                if (extention.ToLower().Equals(".jpg") || extention.ToLower().Equals(".jpeg") || extention.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                        Console.WriteLine(ex);
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jgb,jpeg and png formats are acceptable...'); </script>");

                }


            }
            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }

            return path;
        }

        // GET: Sellers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = db.Seller.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            return View(seller);
        }

        // POST: Sellers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SellerID,SellerName,SellerPassword,SellerPhone,SellerEmail,BrandName")] Seller seller)
        {
            if (ModelState.IsValid)
            {
                db.Entry(seller).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(seller);
        }

        // GET: Sellers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = db.Seller.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            return View(seller);
        }

        // POST: Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seller seller = db.Seller.Find(id);
            db.Seller.Remove(seller);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
