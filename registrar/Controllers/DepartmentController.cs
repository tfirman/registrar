using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Registrar.Models;

namespace Registrar.Controllers
{
    public class DepartmentController : Controller
    {
        [HttpGet("/department")]
        public ActionResult DepartmentIndex()
        {
            List<Department> allDepartments = Department.GetAll();
            return View(allDepartments);
        }

        [HttpGet("/department/new")]
        public ActionResult DepartmentCreateForm()
        {
            return View();
        }
        [HttpPost("/department")]
        public ActionResult DepartmentCreate()
        {
            Department newDepartment = new Department (Request.Form["new-department"]);
            newDepartment.Save();
            return RedirectToAction("DepartmentIndex");
        }

        [HttpPost("/department/delete")]
        public ActionResult DeleteAll()
        {
            Department.DeleteAll();
            return RedirectToAction("Index");
        }

        [HttpGet("/department/{id}")]
        public ActionResult DepartmentDetails(int id)
        {
            Department department = Department.Find(id);
            return View(department);
        }

        [HttpPost("/department/{id}/update")]
        public ActionResult UpdateDepartment(int id)
        {
            Department thisDepartment = Department.Find(id);
            thisDepartment.Edit(Request.Form["new-name"]);
            return RedirectToAction("DepartmentDetails",id);
        }

        [HttpPost("/department/{id}/addcourse")]
        public ActionResult AddCourseToDept(int id)
        {
            Course thisCourse = Course.Find(Int32.Parse(Request.Form["newcourse"]));
            Department thisDepartment = Department.Find(id);
            thisDepartment.AddCourse(thisCourse);
            return RedirectToAction("DepartmentDetails",id);
        }

        [HttpPost("/department/{id}/addstudent")]
        public ActionResult AddStudentToDept(int id)
        {
            Student thisStudent = Student.Find(Int32.Parse(Request.Form["newstudent"]));
            Department thisDepartment = Department.Find(id);
            thisDepartment.AddStudent(thisStudent);
            return RedirectToAction("DepartmentDetails",id);
        }

        [HttpPost("/department/{id}/delete")]
        public ActionResult DeleteDepartment(int id)
        {
            Department thisDepartment = Department.Find(id);
            thisDepartment.Delete();
            return RedirectToAction("DepartmentIndex");
        }
    }
}
