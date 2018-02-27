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
        }

        [TestMethod]
        public void Equals_TrueForSame_Course()
        {
            Course firstCourse = new Course("History of the World", "HIST101");
            Course secondCourse = new Course("History of the World", "HIST101");
            Assert.AreEqual(firstCourse, secondCourse);
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

    }
}
