using Microsoft.EntityFrameworkCore;
using SharpSeer.Interfaces;
using SharpSeer.Models;

namespace SharpSeer.Services
{
    public class ExamService : IService<Exam>
    {
        SharpSeerDBContext context;

        public void Create(Exam ? exam)
        {
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
    }
}