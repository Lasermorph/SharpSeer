using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SharpSeer.Interfaces;
using SharpSeer.Models;
using SharpSeer.Pages.Exams;

namespace SharpSeer.Services
{
    public class ExamService : IService<Exam>
    {
        SharpSeerDbContext context;
        public ExamService(SharpSeerDbContext dbContext)
        {
            context = dbContext;
        }
        public void Create(Exam ? exam)
        {
            foreach (var teacher in exam.Teachers)
            {
                foreach  (var ex in teacher.Exams)
                {
                    if (ex.FirstExamDate == exam.FirstExamDate)
                    {
                        throw new Exception("Teacher already has an exam scheduled at this time.");
                    }
                }
            }
            if (exam == null) 
            {
                throw new NotImplementedException();
            }
            context.Exams.Add(exam);
            context.SaveChanges();
        }

        public void Delete(Exam exam)
        {
            if (exam == null) 
            {
                throw new NotImplementedException();
            } 
            context.Exams.Remove(exam);
            context.SaveChanges();
        }

        public IEnumerable<Exam> GetAll()
        {
            return context.Exams.Include(e => e.Cohorts).Include(e => e.Teachers);
        }

        public Exam? GetById(int id)
        {
            return context.Exams.Include(e => e.Cohorts)
                .Include(e => e.Teachers)
                .FirstOrDefault(e => e.Id == id);
        }

        //public ICollection<Cohort> GetReferenceObj() 
        //{ 
        //    return context.;
        //}
       


        public void Update(Exam exam)
        {
            var entity = context.Exams
                .Include(e => e.Cohorts)
                .Include(e => e.Teachers)
                .FirstOrDefault(e => e.Id == exam.Id);
            if (entity == null)
            {
                throw new NotImplementedException();
            }
            // Update scalar/owned properties
            context.Entry(entity).CurrentValues.SetValues(exam);
            // Update Cohorts
            entity.Cohorts.Clear();
            if (exam.Cohorts != null)
            {
                foreach (var cohort in exam.Cohorts)
                {
                    // Attach the DB tracked cohort by id to avoid duplicate entries
                    var dbC = context.Cohorts.Find(cohort.Id);
                    if (dbC != null)
                    {
                        entity.Cohorts.Add(dbC);
                    }
                }
            }
            // Update Teachers
            entity.Teachers.Clear();
            if (exam.Teachers != null)
            {
                foreach (var teacher in exam.Teachers)
                {
                    var dbT = context.Teachers.Find(teacher.Id);
                    if (dbT != null)
                    {
                        entity.Teachers.Add(dbT);
                    }
                }
            }
            context.SaveChanges();
        }

        public void Update(Exam t, int id)
        {
            throw new NotImplementedException();
        }
    }
    public static class Extensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attribute = field.GetCustomAttributes(typeof(DisplayAttribute), false)
                                 .FirstOrDefault() as DisplayAttribute;

            return attribute?.Name ?? value.ToString();
        }
    }
}