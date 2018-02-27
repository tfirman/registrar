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
        }

        [TestMethod]
        public void Equals_TrueForSame_Student()
        {
            DateTime dt = new DateTime(2008, 3, 9, 16, 5, 7, 0);
            Student firstStudent = new Student("Joe Green", dt);
            Student secondStudent = new Student("Joe Green", dt);
            Assert.AreEqual(firstStudent, secondStudent);
        }

        [TestMethod]
        public void Save_StudentSavesToDatabase_StudentList()
        {
            DateTime dt = new DateTime(2008, 3, 9, 16, 5, 7, 0);
            Student testStudent = new Student("Joe Green", dt);
            testStudent.Save();
            List<Student> result = Student.GetAll();
            List<Student> testList = new List<Student>{testStudent};
            CollectionAssert.AreEqual(testList, result);
        }
    }
}
