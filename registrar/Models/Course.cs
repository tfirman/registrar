using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using System.Data;

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
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO courses (name, coursenum) VALUES (@name, @coursen);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);

            MySqlParameter courseNum = new MySqlParameter();
            courseNum.ParameterName = "@coursen";
            courseNum.Value = this._courseNum;
            cmd.Parameters.Add(courseNum);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Course> GetAll()
        {
            List<Course> allCourses = new List<Course> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM courses;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                string courseNum = rdr.GetString(2);
                Course newCourse = new Course(name, courseNum, id);
                allCourses.Add(newCourse);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCourses;
        }

        public static Course Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM courses WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int newid = 0;
            string name = "";
            string courseNum = "";
            while (rdr.Read())
            {
                newid = rdr.GetInt32(0);
                name = rdr.GetString(1);
                courseNum = rdr.GetString(2);
            }
            Course foundCourse= new Course(name, courseNum, newid);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundCourse;
        }

        public void AddStudent(Student newStudent)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO courses_students (course_id, student_id) VALUES (@CourseId, @StudentId);";

            MySqlParameter course_id = new MySqlParameter();
            course_id.ParameterName = "@CourseId";
            course_id.Value = _id;
            cmd.Parameters.Add(course_id);

            MySqlParameter student_id = new MySqlParameter();
            student_id.ParameterName = "@StudentId";
            student_id.Value = newStudent.GetId();
            cmd.Parameters.Add(student_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Student> GetStudents()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT students.* FROM courses
                JOIN courses_students ON (courses.id = courses_students.course_id)
                JOIN students ON (courses_students.student_id = students.id)
                WHERE courses.id = @CourseId;";

            MySqlParameter courseIdParameter = new MySqlParameter();
            courseIdParameter.ParameterName = "@CourseId";
            courseIdParameter.Value = _id;
            cmd.Parameters.Add(courseIdParameter);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            List<Student> students = new List<Student>{};
            while(rdr.Read())
            {
                int studentid = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                DateTime enrollmentDate = rdr.GetDateTime(2);
                Student newStudent = new Student(name, enrollmentDate, studentid);
                students.Add(newStudent);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return students;
        }

    }

}
