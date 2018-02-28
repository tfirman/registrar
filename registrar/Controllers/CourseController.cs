using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Registrar.Models;

namespace Registrar.Controllers
{
    public class CourseController : Controller
    {
        [HttpGet("/course")]
        public ActionResult CourseIndex()
        {
            List<Course> allCourses = Course.GetAll();
            return View(allCourses);
        }

        [HttpGet("/course/new")]
        public ActionResult CourseCreateForm()
        {
            return View();
        }
        [HttpPost("/course")]
        public ActionResult CourseCreate()
        {
            Course newCourse = new Course (Request.Form["new-course"], Request.Form["new-coursenum"]);
            newCourse.Save();
            return RedirectToAction("CourseIndex");
        }

        [HttpPost("/course/delete")]
        public ActionResult DeleteAll()
        {
            Course.DeleteAll();
            return RedirectToAction("Index");
        }

        [HttpGet("/course/{id}")]
        public ActionResult CourseDetails(int id)
        {
            Course course = Course.Find(id);
            return View(course);
        }

        [HttpPost("/course/{id}/update")]
        public ActionResult UpdateCourse(int id)
        {
            Course thisCourse = Course.Find(id);
            thisCourse.Edit(Request.Form["new-name"],Request.Form["edit-num"]);
            return RedirectToAction("CourseDetails",id);
        }

        [HttpPost("/course/{id}/adddept")]
        public ActionResult AddDeptToCourse(int id)
        {
            Department thisDepartment = Department.Find(Int32.Parse(Request.Form["newdept"]));
            Course thisCourse = Course.Find(id);
            thisCourse.AddDepartment(thisDepartment);
            return RedirectToAction("CourseDetails",id);
        }

        [HttpPost("/course/{id}/addstudent")]
        public ActionResult AddStudentToCourse(int id)
        {
            Student thisStudent = Student.Find(Int32.Parse(Request.Form["newstudent"]));
            Course thisCourse = Course.Find(id);
            thisCourse.AddStudent(thisStudent);
            return RedirectToAction("CourseDetails",id);
        }

        [HttpPost("/course/{id}/delete")]
        public ActionResult DeleteCourse(int id)
        {
            Course thisCourse = Course.Find(id);
            thisCourse.Delete();
            return RedirectToAction("CourseIndex");
        }
    }
}
