using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Practicing_AJAX_JQuery.Models;
using VP_TestPro1.Controllers;

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
            try
            {
                StudentContextClass db = new StudentContextClass();
                using (db)
                {
                    List<Student> students = (from s in db.Students select s).ToList();
                    if (students.Count > 0)
                    {
                        return Json(students, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { msg = "No Data Found!", err = 1 }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { msg = "Cannot Fetch Data From Database. SERVER Error", err = 1 }, JsonRequestBehavior.AllowGet);
            }

        }

        // Fully Server Sourced DataTable Workout

        public JsonResult GetDataToDataTables(DataTablesParam param)
        {

            int pageNo = 1;
            int totalCount = 0;

            if (param.iDisplayStart >= param.iDisplayLength)
            {
                pageNo = (param.iDisplayStart / param.iDisplayLength) + 1;
            }

            StudentContextClass db = new StudentContextClass();
            List<Student> list;
            using (db)
            {
                if (param.sSearch != null)
                {
                    totalCount = (from s in db.Students.Where(x =>
                            x.Name.Contains(param.sSearch) || x.RegistrationNumb.Contains(param.sSearch))
                                  select s).Count();

                    list = (from s in db.Students
                            .Where(x => x.Name.Contains(param.sSearch) || x.RegistrationNumb.Contains(param.sSearch))
                            .OrderBy(x => x.Id)
                            .Skip((pageNo - 1) * param.iDisplayLength)
                            .Take(param.iDisplayLength)
                            select s).ToList();
                }
                else
                {
                    totalCount = db.Students.Count();
                    list = (from s in db.Students
                            .OrderBy(x => x.Id)
                            .Skip((pageNo - 1) * param.iDisplayLength)
                            .Take(param.iDisplayLength)
                            select s).ToList();
                }
            }

            return Json(new
            {
                aaData = list,
                sEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = totalCount

            }
                , JsonRequestBehavior.AllowGet);

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

        public JsonResult DeleteStudent(int studentId)
        {
            StudentContextClass db = new StudentContextClass();
            if (studentId != 0)
            {
                Student stu = (from s in db.Students where s.Id == studentId select s).FirstOrDefault();
                if (stu != null)
                {
                    using (db)
                    {
                        db.Students.Remove(stu);
                        db.SaveChanges();
                    }

                    return Json(new { success = true, responseText = "Data " + stu.Name + " Deleted from Database Succesfully!" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, responseText = "Cannot Delete Data" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateStudent(Student student)
        {
            StudentContextClass db = new StudentContextClass();
            try
            {
                using (db)
                {
                    Student oldStudent = db.Students.Find(student.Id);

                    if (oldStudent != null)
                    {
                        oldStudent.Name = student.Name;
                        oldStudent.RegistrationNumb = student.RegistrationNumb;

                        db.Entry(oldStudent).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return Json(new { msg = "Successfully Update <b><u>" + student.Name + "</u></b> Data", err = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { msg = "Cannot Update Data to Server! Try Again Later", err = 1 }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}