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

        [HttpPost]
        public JsonResult AddStudents(string[][] studentsArry)
        {
            StudentContextClass db = new StudentContextClass();

            if (studentsArry != null && studentsArry.Length > 0)
            {
                List<Student> students = new List<Student>();
                for (int i = 0; i < studentsArry.Length; i++)
                {
                    Student stu = new Student
                    {
                        Name = studentsArry[i][1],
                        RegistrationNumb = studentsArry[i][2]
                    };
                    students.Add(stu);
                    students.TrimExcess();
                }

                List<Student> oldStudentsList;

                using (db)
                {
                    oldStudentsList = (from s in db.Students select s).ToList();
                    students.RemoveAll(x => oldStudentsList.Exists(y => y.Name == x.Name || y.RegistrationNumb == x.RegistrationNumb));
                    db.Students.AddRange(students);
                    db.SaveChanges();
                }

                return Json(new { success = true, responseText = "New Data added to Database Successfully!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, responseText = "Something Went Wrong!" }, JsonRequestBehavior.AllowGet);
        }


    }
}