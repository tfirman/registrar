using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Registrar.Models;

namespace Registrar.Controllers
{
    public class StudentController : Controller
    {
        [HttpGet("/student")]
        public ActionResult StudentIndex()
        {
            List<Student> allStudents = Student.GetAll();
            return View(allStudents);
        }

        [HttpGet("/student/new")]
        public ActionResult StudentCreateForm()
        {
            return View();
        }

        [HttpPost("/student")]
        public ActionResult StudentCreate()
        {
            Student newStudent = new Student (Request.Form["new-student"], DateTime.Parse(Request.Form["enroll-date"]));
            newStudent.Save();
            return RedirectToAction("StudentIndex");
        }

        [HttpPost("/student/delete")]
        public ActionResult DeleteAll()
        {
            Student.DeleteAll();
            return RedirectToAction("Index");
        }

        [HttpGet("/student/{id}")]
        public ActionResult StudentDetails(int id)
        {
            Student student = Student.Find(id);
            return View(student);
        }

        [HttpPost("/student/{id}/update")]
        public ActionResult UpdateStudent(int id)
        {
            Student thisStudent = Student.Find(id);
            thisStudent.Edit(Request.Form["new-student"], DateTime.Parse(Request.Form["enroll-date"]));
            return RedirectToAction("StudentDetails",id);
        }

        [HttpPost("/student/{id}/addcourse")]
        public ActionResult AddCourseToStudent(int id)
        {
            Course thisCourse = Course.Find(Int32.Parse(Request.Form["newcourse"]));
            Student thisStudent = Student.Find(id);
            thisStudent.AddCourse(thisCourse);
            return RedirectToAction("StudentDetails",id);
        }

        [HttpPost("/student/{id}/adddept")]
        public ActionResult AddDepartmentToStudent(int id)
        {
            Department thisDepartment = Department.Find(Int32.Parse(Request.Form["newdept"]));
            Student thisStudent = Student.Find(id);
            thisStudent.AddDepartment(thisDepartment);
            return RedirectToAction("StudentDetails",id);
        }

        [HttpPost("/student/{id}/delete")]
        public ActionResult DeleteStudent(int id)
        {
            Student thisStudent = Student.Find(id);
            thisStudent.Delete();
            return RedirectToAction("StudentIndex");
        }
    }
}
