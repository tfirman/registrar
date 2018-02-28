using System;
using Regist;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Registrar.Models;

namespace Registrar.Tests
{
    [TestClass]
    public class CourseTest : IDisposable
    {
        public CourseTest()
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
        public void Equals_TrueForSame_Course()
        {
            Course firstCourse = new Course("History of the World", "HIST101");
            Course secondCourse = new Course("History of the World", "HIST101");
            Assert.AreEqual(firstCourse, secondCourse);
        }

        [TestMethod]
        public void Save_AssignsIdToObject_id()
        {
            Course testCourse = new Course("History of the World", "HIST101");
            testCourse.Save();
            Course savedCourse = Course.GetAll()[0];
            int result = savedCourse.GetId();
            int testId = testCourse.GetId();
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Save_CourseSavesToDatabase_CourseList()
        {
            Course testCourse = new Course("History of the World", "HIST101");
            testCourse.Save();
            List<Course> result = Course.GetAll();
            List<Course> testList = new List<Course>{testCourse};
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Find_FindsCourseInDatabase_Course()
        {
            Course testCourse = new Course("History of the World", "HIST101");
            testCourse.Save();
            Course result = Course.Find(testCourse.GetId());
            Assert.AreEqual(testCourse, result);
        }

        [TestMethod]
        public void AddStudent_AddsStudentToCourse_StudentList()
        {
            DateTime dt = new DateTime(2008, 3, 9, 16, 5, 7);
            Student testStudent = new Student("Joe Green", dt);
            testStudent.Save();
            Course testCourse = new Course("History of the World", "HIST101");
            testCourse.Save();
            testCourse.AddStudent(testStudent);
            List<Student> result = testCourse.GetStudents();
            List<Student> testList = new List<Student>{testStudent};
            CollectionAssert.AreEqual(testList, result);
        }
        [TestMethod]
        public void AddDepartment_AddsDepartmentToCourse_DepartmentList()
        {
            Department testDepartment = new Department("History");
            testDepartment.Save();
            Course testCourse = new Course("History of the World", "HIST101");
            testCourse.Save();
            testCourse.AddDepartment(testDepartment);
            List<Department> result = testCourse.GetDepartments();
            List<Department> testList = new List<Department>{testDepartment};
            CollectionAssert.AreEqual(testList, result);
        }
    }
}
