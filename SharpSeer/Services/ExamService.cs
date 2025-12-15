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
            context.Exams.Add(exam);
            context.SaveChanges();
        }

        public void Delete(Exam exam)
        {
            context.Exams.Remove(exam);
            context.SaveChanges();
        }

        public IEnumerable<Exam> GetAll()
        {
            // Explicitly set list in exam from junctiontable
            return context.Exams.Include(e => e.Cohorts).Include(e => e.Teachers);
        }

        public Exam? GetById(int id)
        {
            // Explicitly set list in exam from junctiontable
            return context.Exams.Include(e => e.Cohorts)
                .Include(e => e.Teachers)
                .FirstOrDefault(e => e.Id == id);
        }       


        public void Update(Exam exam)
        {
            Exam? entity = context.Exams
                .Include(e => e.Cohorts)
                .Include(e => e.Teachers)
                .FirstOrDefault(e => e.Id == exam.Id);

            // Update scalar/owned properties
            context.Entry(entity).CurrentValues.SetValues(exam);
            // Update Cohorts
            entity.Cohorts = exam.Cohorts;

            // Update Teachers
            entity.Teachers = exam.Teachers;
            context.SaveChanges();
        }

    }
}