using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Feetup.Models;

using FeetUp.Repository;
using Newtonsoft.Json;

namespace FeetUp.Controllers
{
    public class AdminsController : Controller
    {
        private DBfeetEntities db = new DBfeetEntities();
        public GenericUnitOfWork unitOfWork = new GenericUnitOfWork();

        // GET: Admins
        public ActionResult Index()
        {
            return View(db.Admin.ToList());
        }

        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Authorize(Feetup.Models.Admin admin)
        {
            var admindetails = db.Admin.Where(x => x.AdminName == admin.AdminName && x.AdminPassword == admin.AdminPassword).FirstOrDefault();
            if (admindetails == null)
            {
                admin.LoginErrorMessage = "Wrong username or password";
                return View("AdminLogin", admin);
            }
            else
            {
                Session["adminID"] = admindetails.AdminID;
                return RedirectToAction("Categories", "Admins");
            }


        }
        public ActionResult LogOut()
        {
            // Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            //FormsAuthentication.SignOut();
            return RedirectToAction("AdminLogin");
        }

        public ActionResult Dashboard()
        {
            if (Session["adminID"] != null)
            {
                return View(db.Admin.ToList());
            }
            return RedirectToAction("AdminLogin");
        }



        public ActionResult Categories()
        {
            if (Session["adminID"] != null)
            {
                List<Category> allcategories = unitOfWork.GetRepositoryInstance<Category>().GetAllRecordsIQueryable().ToList();
                return View(allcategories);
            }
            return RedirectToAction("AdminLogin");
        }

        public ActionResult Details(int? id)
        {
            if (Session["adminID"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Admin admin = db.Admin.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }
                return View(admin);
            }
            return RedirectToAction("AdminLogin");
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            if (Session["adminID"] != null)
            {
                return View();
            }
            return RedirectToAction("AdminLogin");
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdminID,AdminName,AdminPassword")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admin.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["adminID"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Admin admin = db.Admin.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }
                return View(admin);
            }
            return RedirectToAction("AdminLogin");

        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdminID,AdminName,AdminPassword")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["adminID"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Admin admin = db.Admin.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }
                return View(admin);
            }
            return RedirectToAction("AdminLogin");
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admin.Find(id);
            db.Admin.Remove(admin);
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
