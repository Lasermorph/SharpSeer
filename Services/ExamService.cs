using Microsoft.EntityFrameworkCore;
using SharpSeer.Interfaces;
using SharpSeer.Models;

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
            return context.Exams;
        }

        public Exam? GetById(int id)
        {
            return context.Exams.Find(id);
        }
        public void Update(Exam exam)
        {
            var entity = context.Exams.Find(exam.Id);
            if (entity == null)
            {
                throw new NotImplementedException();
            }
            context.Entry(entity).CurrentValues.SetValues(exam);
            context.SaveChanges();
        }

        public void Update(Exam t, int id)
        {
            throw new NotImplementedException();
        }
    }
}