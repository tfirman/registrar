using System;
using Regist;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Registrar.Models;

namespace Registrar.Tests
{
    [TestClass]
    public class StudentTest : IDisposable
    {
        public StudentTest()
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
        public void Equals_TrueForSame_Student()
        {
            DateTime dt = new DateTime(2008, 3, 9, 16, 5, 7);
            Student firstStudent = new Student("Joe Green", dt);
            Student secondStudent = new Student("Joe Green", dt);
            Assert.AreEqual(firstStudent, secondStudent);
        }

        [TestMethod]
        public void Save_AssignsIdToObject_id()
        {
            DateTime dt = new DateTime(2008, 3, 9, 16, 5, 7);
            Student testStudent = new Student("Joe Green", dt);
            testStudent.Save();
            Student savedStudent = Student.GetAll()[0];
            int result = savedStudent.GetId();
            int testId = testStudent.GetId();
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Save_StudentSavesToDatabase_StudentList()
        {
            DateTime dt = new DateTime(2008, 3, 9, 16, 5, 7);
            Student testStudent = new Student("Joe Green", dt);
            testStudent.Save();
            List<Student> result = Student.GetAll();
            List<Student> testList = new List<Student>{testStudent};
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Find_FindsStudentInDatabase_Student()
        {
            DateTime dt = new DateTime(2008, 3, 9, 16, 5, 7);
            Student testStudent = new Student("Joe Green", dt);
            testStudent.Save();
            Student result = Student.Find(testStudent.GetId());
            Assert.AreEqual(testStudent, result);
        }

        [TestMethod]
        public void AddCourse_AddsCourseToStudent_CourseList()
        {
            DateTime dt = new DateTime(2008, 3, 9, 16, 5, 7);
            Student testStudent = new Student("Joe Green", dt);
            testStudent.Save();
            Course testCourse = new Course("History of the World", "HIST101");
            testCourse.Save();
            testStudent.AddCourse(testCourse);
            List<Course> result = testStudent.GetCourses();
            List<Course> testList = new List<Course>{testCourse};
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void AddDepartment_AddsDepartmentToStudent_DepartmentList()
        {
            DateTime dt = new DateTime(2008, 3, 9, 16, 5, 7);
            Student testStudent = new Student("Joe Green", dt);
            testStudent.Save();
            Department testDepartment = new Department("History");
            testDepartment.Save();
            testStudent.AddDepartment(testDepartment);
            List<Department> result = testStudent.GetDepartments();
            List<Department> testList = new List<Department>{testDepartment};
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Edit_EditChangesStudent_Student()
        {
            DateTime dt = new DateTime(2008, 3, 9, 16, 5, 7);
            Student testStudent = new Student("Joe Green", dt);
            testStudent.Save();
            DateTime dt2 = new DateTime(2000, 1, 2, 3, 4, 5);
            testStudent.Edit("Eric Cartman", dt2);
            Student result = Student.Find(testStudent.GetId());
            Assert.AreEqual("Eric Cartman", result.GetName());
            Assert.AreEqual(dt2, result.GetEnrollmentDate());
        }

        [TestMethod]
        public void Delete_DeletesStudentAssociationsFromDatabase_StudentList()
        {
            DateTime dt = new DateTime(2008, 3, 9, 16, 5, 7);
            Student testStudent = new Student("Joe Green", dt);
            testStudent.Save();
            Department testDepartment = new Department("History");
            testDepartment.Save();
            Course testCourse = new Course("History of the World", "HIST101");
            testCourse.Save();
            testStudent.AddCourse(testCourse);
            testStudent.AddDepartment(testDepartment);
            testStudent.Delete();
            List<Student> resultDepartmentStudents = testDepartment.GetStudents();
            List<Student> resultCourseStudents = testCourse.GetStudents();
            List<Student> testDepartmentStudents = new List<Student> {};
            List<Student> testCourseStudents = new List<Student> {};
            CollectionAssert.AreEqual(testDepartmentStudents, resultDepartmentStudents);
            CollectionAssert.AreEqual(testCourseStudents, resultCourseStudents);
        }
    }
}
