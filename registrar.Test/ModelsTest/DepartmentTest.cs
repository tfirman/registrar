using System;
using Regist;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Registrar.Models;

namespace Registrar.Tests
{
    [TestClass]
    public class DepartmentTest : IDisposable
    {
        public DepartmentTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=regist_test;";
        }
        public void Dispose()
        {
            Student.DeleteAll();
            Course.DeleteAll();
            Department.DeleteAll();
        }

        [TestMethod]
        public void Equals_TrueForSame_Department()
        {
            Department firstDepartment = new Department("History");
            Department secondDepartment = new Department("History");
            Assert.AreEqual(firstDepartment, secondDepartment);
        }

        [TestMethod]
        public void Save_AssignsIdToObject_id()
        {
            Department testDepartment = new Department("History");
            testDepartment.Save();
            Department savedDepartment = Department.GetAll()[0];
            int result = savedDepartment.GetId();
            int testId = testDepartment.GetId();
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Save_DepartmentSavesToDatabase_DepartmentList()
        {
            Department testDepartment = new Department("History");
            testDepartment.Save();
            List<Department> result = Department.GetAll();
            List<Department> testList = new List<Department>{testDepartment};
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Find_FindsDepartmentInDatabase_Department()
        {
            Department testDepartment = new Department("History");
            testDepartment.Save();
            Department result = Department.Find(testDepartment.GetId());
            Assert.AreEqual(testDepartment, result);
        }

        [TestMethod]
        public void AddStudent_AddsStudentToDepartment_StudentList()
        {
            DateTime dt = new DateTime(2008, 3, 9, 16, 5, 7);
            Student testStudent = new Student("Joe Green", dt);
            testStudent.Save();
            Department testDepartment = new Department("History");
            testDepartment.Save();
            testDepartment.AddStudent(testStudent);
            List<Student> result = testDepartment.GetStudents();
            List<Student> testList = new List<Student>{testStudent};
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void AddCourse_AddsCourseToDepartment_CourseList()
        {
            Course testCourse = new Course("History of the World", "HIST101");
            testCourse.Save();
            Department testDepartment = new Department("History");
            testDepartment.Save();
            testDepartment.AddCourse(testCourse);
            List<Course> result = testDepartment.GetCourses();
            List<Course> testList = new List<Course>{testCourse};
            CollectionAssert.AreEqual(testList, result);
        }
    }
}
