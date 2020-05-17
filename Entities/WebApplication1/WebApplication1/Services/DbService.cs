using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class DbService : IDbService
    {
        public List<Student> GetStudent()
        {

            var db = new s19092Context();
            return db.Student.ToList();

        }

        public string ChangeStudent(Student student)
        {

            var db = new s19092Context();


            try
            {
                Student stul = db.Student.First(s => s.IndexNumber == student.IndexNumber);
                stul.FirstName = student.FirstName;
                stul.LastName = student.LastName;
                stul.BirthDate = student.BirthDate;
                stul.IdEnrollment = student.IdEnrollment;
                stul.Refresh = student.Refresh;
                stul.Password = student.Password;
                stul.Salt = student.Salt;
                stul.IdEnrollmentNavigation = student.IdEnrollmentNavigation;
                db.SaveChanges();
              
                return "Ok";
            }
            catch (InvalidOperationException e)
            {
                return "error";
            }
            

        }

        public string DeleteStudent(string index)
        {

            var db = new s19092Context();

            try
            {
                var students = db.Student.FirstOrDefault(s => s.IndexNumber == index);
                if (students != null)
                {
                    db.Student.Remove(students);
                    db.SaveChanges();
                }
                else
                    return "error";

                return "Ok";
            }catch(SqlException e)
            {
                return "error";
            }
        }

        public List<Enrollment> GetEnrollment()
        {

            var db = new s19092Context();

            try
            {

                var enrollments = db.Enrollment.ToList();
                return enrollments;

            }catch(SqlException e)
            {
                return null;
                
            }

        }

        public string EnrollStudent(EnrollStudentRequest req)
        {

            var db = new s19092Context();

            try
            {


                Nullable<int> res = db.Studies.First(s => s.Name == req.Studies).IdStudy;
                if (res == null)
                    return "error";
                var idEnrollment = db.Enrollment.Max(e => e.IdEnrollment) + 1;

                db.Enrollment.Add(new Enrollment
                {

                    IdEnrollment = idEnrollment,
                    Semester = 0,
                    IdStudy = (int)res,
                    StartDate = DateTime.Now

                }); 
              

                db.Student.Add(new Student
                {

                    IndexNumber = req.IndexNumber,
                    FirstName = req.FirstName,
                    LastName = req.LastName,
                    BirthDate = req.BirthDate,
                    IdEnrollment = idEnrollment


                });
                db.SaveChanges();

                return "Ok";
                
            }
            catch(SqlException e)
            {

                return "error";

            }
            catch(InvalidOperationException eo)
            {

                return "error";

            }

        }

        public string PromoteStudents(PromoteStudentRequest req)
        {


            var db = new s19092Context();


            try
            {
                var id = db.Studies.First(s => s.Name == req.Studies).IdStudy;
                var ide = db.Enrollment.Where(e => e.IdStudy == id)
                    .Where(e => e.Semester == req.Semester);


                if (ide.Count() < 1)
                    return "error: Not found such enrollments";


                foreach(var e in ide)
                {

                    e.Semester += 1;

                }


                db.SaveChanges();


                return ("Ok");

            }catch(SqlException e)
            {

                return "error: sql exception";

            }catch(InvalidOperationException en)
            {

                return "error: Doesnt exist";

            }




        }
    }
}

