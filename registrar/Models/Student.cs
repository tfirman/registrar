using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Registrar.Models
{
    public class Student
    {
        private int _id;
        private string _name;
        private DateTime _enrollmentDate;

        public Student(string name, DateTime enrollmentDate, int Id = 0)
        {
            _id = Id;
            _name = name;
            _enrollmentDate = enrollmentDate;
        }

        public override bool Equals(System.Object otherStudent)
        {
            if (!(otherStudent is Student))
            {
                return false;
            }
            else
            {
                Student newStudent = (Student) otherStudent;
                bool idEquality = (this.GetId() == newStudent.GetId());
                bool nameEquality = (this.GetName() == newStudent.GetName());
                bool enrollmentDateEquality = (this.GetEnrollmentDate() == newStudent.GetEnrollmentDate());
                return (idEquality && nameEquality && enrollmentDateEquality);
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

        public DateTime GetEnrollmentDate()
        {
            return _enrollmentDate;
        }
        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM students;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO students (name, enrollmentdate) VALUES (@name, @enrolldate);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);

            MySqlParameter enrolldate = new MySqlParameter();
            enrolldate.ParameterName = "@enrolldate";
            enrolldate.Value = this._enrollmentDate;
            enrolldate.MySqlDbType = MySqlDbType.DateTime;
            cmd.Parameters.Add(enrolldate);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Student> GetAll()
        {
            List<Student> allStudents = new List<Student> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM students;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                DateTime enrollmentDate = rdr.GetDateTime(2);
                Student newStudent = new Student(name, enrollmentDate, id);
                allStudents.Add(newStudent);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allStudents;
        }

        public static Student Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM students WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int newid = 0;
            string name = "";
            DateTime enrollmentDate = new DateTime (2000, 1, 1, 1, 0, 0);
            while (rdr.Read())
            {
                newid = rdr.GetInt32(0);
                name = rdr.GetString(1);
                enrollmentDate = rdr.GetDateTime(2);
            }
            Student foundStudent = new Student(name, enrollmentDate, newid);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundStudent;
        }

        public void AddCourse(Course newCourse)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO courses_students (course_id, student_id) VALUES (@CourseId, @StudentId);";

            MySqlParameter course_id = new MySqlParameter();
            course_id.ParameterName = "@CourseId";
            course_id.Value = newCourse.GetId();
            cmd.Parameters.Add(course_id);

            MySqlParameter student_id = new MySqlParameter();
            student_id.ParameterName = "@StudentId";
            student_id.Value = _id;
            cmd.Parameters.Add(student_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Course> GetCourses()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT courses.* FROM students
                JOIN courses_students ON (students.id = courses_students.student_id)
                JOIN courses ON (courses_students.course_id = courses.id)
                WHERE students.id = @StudentId;";

            MySqlParameter StudentIdParameter = new MySqlParameter();
            StudentIdParameter.ParameterName = "@StudentId";
            StudentIdParameter.Value = _id;
            cmd.Parameters.Add(StudentIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            List<Course> courses = new List<Course> {};

            while(rdr.Read())
            {
                int newid = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                string courseNum = rdr.GetString(2);
                Course foundCourse = new Course(name, courseNum, newid);
                courses.Add(foundCourse);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return courses;
        }
    }
}
