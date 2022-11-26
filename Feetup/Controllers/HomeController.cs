using Feetup.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Feetup.Controllers
{
    public class HomeController : Controller
    {

        private DBfeetEntities db = new DBfeetEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HomePage(int?page)
        { 
            int pagesize = 9, pageindex = 1;
        pageindex = page.HasValue? Convert.ToInt32(page) : 1;
            var list = db.Product.ToList();
        IPagedList<Product> stu = list.ToPagedList(pageindex, pagesize);
        
            return View(stu);

        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}