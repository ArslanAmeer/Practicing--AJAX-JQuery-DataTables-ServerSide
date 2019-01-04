using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Practicing_AJAX_JQuery.Models;

namespace Practicing_AJAX_JQuery.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            StudentContextClass db = new StudentContextClass();
            List<Student> students;
            using (db)
            {
                students = (from s in db.Students select s).ToList();
            }
            return View(students);
        }
    }
}