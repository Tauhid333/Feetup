using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Feetup.Models;

namespace Feetup.Controllers
{
    public class CustomersController : Controller
    {
        private DBfeetEntities db = new DBfeetEntities();

        List<ShoppingCartModel> li = new List<ShoppingCartModel>();

        public CustomersController()
        {
            db = new DBfeetEntities();

        }

        // GET: Customers
        public ActionResult Index()
        {
            if (Session["cart"] != null)
            {
                ShoppingCartModel s = new ShoppingCartModel();
                s.TotalPrice = 0;
                List<ShoppingCartModel> li2 = Session["cart"] as List<ShoppingCartModel>;

                foreach (var item in li2)
                {


                    s.TotalPrice += item.TotalPrice;

                }
                Session["total"] = s.TotalPrice;
                Session["item_count"] = li2.Count();
            }
            return RedirectToAction("ViewCat");
        }

        public ActionResult ViewCat()
        {
            //int pagesize = 9, pageindex = 1;
            //pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Category.ToList();
            //IPagedList<tbl_category> stu = list.ToPagedList(pageindex, pagesize);


            return View(list);

        }


        public ActionResult CatDetails(int? id)
        {

            //int pagesize = 9, pageindex = 1;
            // pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Product.Where(x => x.CategoryID == id).ToList();
            //IPagedList<tbl_product> stu = list.ToPagedList(pageindex, pagesize);


            return View(list);




        }

        public ActionResult AddCart(int? id)
        {
            Product p = db.Product.Where(x => x.ProductID == id).SingleOrDefault();
            return View(p);

        }


        [HttpPost]
        public ActionResult AddCart(int id, int qty)
        {
            Product p = db.Product.Where(x => x.ProductID == id).SingleOrDefault();
            Cart q = db.Cart.Where(x => x.Quantity == qty).SingleOrDefault();

            ShoppingCartModel shoppingCartModel = new ShoppingCartModel();
            shoppingCartModel.ProductID = id;
            shoppingCartModel.ProductName = p.ProductName;
            shoppingCartModel.ProductPrice = p.ProductPrice;
            shoppingCartModel.Quantity = qty;
            shoppingCartModel.TotalPrice = p.ProductPrice * qty;

            if (Session["cart"] == null)
            {
                li.Add(shoppingCartModel);
                Session["cart"] = li;

            }
            else
            {
                List<ShoppingCartModel> li2 = Session["cart"] as List<ShoppingCartModel>;
                int flag = 0;
                foreach (var item in li2)
                {
                    if (item.ProductID == shoppingCartModel.ProductID)
                    {
                        item.Quantity += shoppingCartModel.Quantity;
                        item.TotalPrice += shoppingCartModel.TotalPrice;
                        flag = 1;
                    }
                }
                if (flag == 0)
                {
                    li2.Add(shoppingCartModel);
                }
            }

            return RedirectToAction("Index");

        }

        public ActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Checkout(string CustomerPhn, string address)
        {

            List<ShoppingCartModel> li2 = Session["cart"] as List<ShoppingCartModel>;
            Ordered ord = new Ordered();
            Customer cus = new Customer();


            if (Session["ad_id"] != null)
            {
                foreach (var item in li2)
                {
                    ord.CustomerAddress = address;
                    ord.CustomerID = Convert.ToInt32(Session["ad_id"].ToString());
                    cus.CustomerPhone = CustomerPhn;
                    db.Customer.Add(cus);
                    db.Ordered.Add(ord);
                    db.SaveChanges();




                }
                Session.Remove("total");
                Session.Remove("cart");
                // TempData["msg"] = "Order Book Successfully!!";
                return RedirectToAction("../Home/HomePage");
            }
            else
            {
                return RedirectToAction("login");
            }


            //return View();
        }



        public ActionResult remove(int? id)
        {
            if (Session["cart"] == null)
            {
                Session.Remove("total");
                Session.Remove("cart");

            }
            else
            {
                List<ShoppingCartModel> li2 = Session["cart"] as List<ShoppingCartModel>;
                ShoppingCartModel s = li2.Where(x => x.ProductID == id).SingleOrDefault();
                li2.Remove(s);
                s.TotalPrice = 0;
                foreach (var item in li2)
                {
                    s.TotalPrice += item.TotalPrice;

                }
                Session["total"] = s.TotalPrice;
            }
            return RedirectToAction("Index");
        }



        public ActionResult ViewCart(int? id)
        {





            return View();




        }
        public ActionResult ViewItem(int? id)
        {

            ViewItem ad = new ViewItem();
            Product p = db.Product.Where(x => x.ProductID == id).SingleOrDefault();
            ad.ProductID = p.ProductID;
            ad.ProductName = p.ProductName;
            ad.ProductImage = p.ProductImage;
            ad.ProductPrice = p.ProductPrice;
            ad.ProductBrand = p.ProductBrand;
            ad.ProductColor = p.ProductColor;
            ad.ProductDescription = p.ProductDescription;
            Category cat = db.Category.Where(x => x.CategoryID == p.CategoryID).SingleOrDefault();
            ad.CategoryName = cat.CategoryName;
            Seller u = db.Seller.Where(x => x.SellerID == p.SellerID).SingleOrDefault();
            ad.SellerName = u.SellerName;
            ad.SellerEmail = u.SellerEmail;
            ad.SellerPhone = u.SellerPhone;
            ad.SellerID = u.SellerID;




            return View(ad);







        }




        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Registration()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Include = "CustomerID,CustomerName,CustomerPassword,CustomerPhone,CustomerEmail")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customer.Add(customer);
                db.SaveChanges();
                return RedirectToAction("login");
            }

            return View(customer);
        }
        [HttpGet]
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(Customer avm)
        {

            Customer ad = db.Customer.Where(x => x.CustomerName == avm.CustomerName && x.CustomerPassword == avm.CustomerPassword).FirstOrDefault();

            if (ad != null)
            {


                Session["ad_id"] = ad.CustomerID.ToString();
                return RedirectToAction("../Home/HomePage");

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

            return RedirectToAction("../Home/HomePage");
        }


        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,CustomerName,CustomerPassword,CustomerPhone,CustomerEmail")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customer.Find(id);
            db.Customer.Remove(customer);
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
