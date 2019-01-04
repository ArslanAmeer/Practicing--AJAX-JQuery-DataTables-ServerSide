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

            return View();
        }

        public JsonResult GetData()
        {
            StudentContextClass db = new StudentContextClass();
            try
            {
                using (db)
                {
                    List<Student> students = (from s in db.Students select s).ToList();
                    if (students.Count > 0)
                    {
                        return Json(students, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { msg = "Cannot Fetch Data From Database. SERVER Error" }, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

    }
}