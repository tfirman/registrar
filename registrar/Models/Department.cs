using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Registrar.Models
{
    public class Department
    {
        private int _id;
        private string _name;

        public Department(string name, int Id = 0)
        {
            _id = Id;
            _name = name;
        }

        public override bool Equals(System.Object otherDepartment)
        {
            if (!(otherDepartment is Department))
            {
                return false;
            }
            else
            {
                Department newDepartment = (Department) otherDepartment;
                bool idEquality = (this.GetId() == newDepartment.GetId());
                bool nameEquality = (this.GetName() == newDepartment.GetName());
                return (idEquality && nameEquality);
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

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM departments;";

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
            cmd.CommandText = @"INSERT INTO departments (name) VALUES (@name);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Edit(string newName)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE departments SET name = @newName WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@newName";
            name.Value = newName;
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();
            _name = newName;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Delete()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"DELETE FROM departments WHERE id = @DepartmentId;
              DELETE FROM departments_courses WHERE department_id = @DepartmentId;
              DELETE FROM departments_students WHERE department_id = @DepartmentId;";

          MySqlParameter departmentIdParameter = new MySqlParameter();
          departmentIdParameter.ParameterName = "@DepartmentId";
          departmentIdParameter.Value = this.GetId();
          cmd.Parameters.Add(departmentIdParameter);

          cmd.ExecuteNonQuery();
          if (conn != null)
          {
            conn.Close();
          }
        }

        public static List<Department> GetAll()
        {
            List<Department> allDepartments = new List<Department> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM departments;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                Department newDepartment = new Department(name, id);
                allDepartments.Add(newDepartment);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allDepartments;
        }

        public static Department Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM departments WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int newid = 0;
            string name = "";
            while (rdr.Read())
            {
                newid = rdr.GetInt32(0);
                name = rdr.GetString(1);
            }
            Department foundDepartment = new Department(name, newid);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundDepartment;
        }

        public void AddStudent(Student newStudent)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO departments_students (department_id, student_id) VALUES (@DepartmentId, @StudentId);";

            MySqlParameter department_id = new MySqlParameter();
            department_id.ParameterName = "@DepartmentId";
            department_id.Value = _id;
            cmd.Parameters.Add(department_id);

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
            cmd.CommandText = @"SELECT students.* FROM departments
                JOIN departments_students ON (departments.id = departments_students.department_id)
                JOIN students ON (departments_students.student_id = students.id)
                WHERE departments.id = @DepartmentId;";

            MySqlParameter departmentIdParameter = new MySqlParameter();
            departmentIdParameter.ParameterName = "@DepartmentId";
            departmentIdParameter.Value = _id;
            cmd.Parameters.Add(departmentIdParameter);
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

        public void AddCourse(Course newCourse)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO departments_courses (department_id, course_id) VALUES (@DepartmentId, @CourseId);";

            MySqlParameter department_id = new MySqlParameter();
            department_id.ParameterName = "@DepartmentId";
            department_id.Value = _id;
            cmd.Parameters.Add(department_id);

            MySqlParameter course_id = new MySqlParameter();
            course_id.ParameterName = "@CourseId";
            course_id.Value = newCourse.GetId();
            cmd.Parameters.Add(course_id);

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
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT courses.* FROM departments
                JOIN departments_courses ON (departments.id = departments_courses.department_id)
                JOIN courses ON (departments_courses.course_id = courses.id)
                WHERE departments.id = @DepartmentId;";

            MySqlParameter departmentIdParameter = new MySqlParameter();
            departmentIdParameter.ParameterName = "@DepartmentId";
            departmentIdParameter.Value = _id;
            cmd.Parameters.Add(departmentIdParameter);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            List<Course> courses = new List<Course>{};
            while(rdr.Read())
            {
                int courseid = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                string coursenum = rdr.GetString(2);
                Course newCourse = new Course(name, coursenum, courseid);
                courses.Add(newCourse);
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
