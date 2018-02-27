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
        }

        [TestMethod]
        public void Equals_TrueForSameDescription_Item()
        {
            Student firstStudent = new Student("Joe Green", "2008-11-11");
            Student secondStudent = new Student("Joe Green", "2008-11-11");
            Assert.AreEqual(firstStudent, secondStudent);
        }
    }
}
