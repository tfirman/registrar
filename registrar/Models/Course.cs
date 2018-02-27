using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Registrar.Models
{
    public class Course
    {
        private int _id;
        private string _name;
        private string _courseNum;

        public Course(string name, string courseNum, int Id = 0)
        {
            _id = Id;
            _name = name;
            _courseNum = courseNum;
        }

        public override bool Equals(System.Object otherCourse)
        {
            if (!(otherCourse is Course))
            {
                return false;
            }
            else
            {
                Course newCourse = (Course) otherCourse;
                bool idEquality = (this.GetId() == newCourse.GetId());
                bool nameEquality = (this.GetName() == newCourse.GetName());
                bool courseNumEquality = (this.GetCourseNum() == newCourse.GetCourseNum());
                return (idEquality && nameEquality && courseNumEquality);
            }
        }
        public override int GetHashCode()
        {
            return this.GetId().GetHashCode();
        }

        public string GetName()
        {
            return _name;
        }

        public void SetName(string newName)
        {
            _name = newName;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetCourseNum()
        {
            return _courseNum;
        }
        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM courses;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }

}
