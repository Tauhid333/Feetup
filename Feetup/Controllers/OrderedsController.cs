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
    public class OrderedsController : Controller
    {
        private DBfeetEntities db = new DBfeetEntities();

        // GET: Ordereds
        public ActionResult Index()
        {
            var ordered = db.Ordered.Include(o => o.Customer);
            return View(ordered.ToList());
        }

        // GET: Ordereds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordered ordered = db.Ordered.Find(id);
            if (ordered == null)
            {
                return HttpNotFound();
            }
            return View(ordered);
        }

        // GET: Ordereds/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerID", "CustomerName");
            return View();
        }

        // POST: Ordereds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,CustomerID,CustomerAddress")] Ordered ordered)
        {
            if (ModelState.IsValid)
            {
                db.Ordered.Add(ordered);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerID", "CustomerName", ordered.CustomerID);
            return View(ordered);
        }

        // GET: Ordereds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordered ordered = db.Ordered.Find(id);
            if (ordered == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerID", "CustomerName", ordered.CustomerID);
            return View(ordered);
        }

        // POST: Ordereds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,CustomerID,CustomerAddress")] Ordered ordered)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordered).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerID", "CustomerName", ordered.CustomerID);
            return View(ordered);
        }

        // GET: Ordereds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordered ordered = db.Ordered.Find(id);
            if (ordered == null)
            {
                return HttpNotFound();
            }
            return View(ordered);
        }

        // POST: Ordereds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ordered ordered = db.Ordered.Find(id);
            db.Ordered.Remove(ordered);
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
