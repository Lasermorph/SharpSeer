using SharpSeer.Interfaces;
using SharpSeer.Models;

namespace SharpSeer.Services
{
    public class TeacherService : IService<Teacher>
    {

        SharpSeerDbContext context;

        public TeacherService(SharpSeerDbContext dbContext)
        {
            context = dbContext;
        }

        public void Create(Teacher? teacher)
        {
            if (teacher == null)
            {
                throw new NotImplementedException();
            }
            context.Teachers.Add(teacher);
            context.SaveChanges();
        }

        public void Delete(Teacher teacher)
        {
            if (teacher == null)
            {
                throw new NotImplementedException();
            }
            context.Teachers.Remove(teacher);
            context.SaveChanges();
        }
        

        public IEnumerable<Teacher> GetAll()
        {
            return context.Teachers;
        }

        public Teacher? GetById(int id)
        {
            return context.Teachers.Find(id);
        }

        public void Update(Teacher teacher)
        {
            var entity = context.Teachers.Find(teacher.Id);
            if (entity == null)
            {
                throw new NotImplementedException();
            }
            context.Entry(entity).CurrentValues.SetValues(teacher);
            context.SaveChanges();
        }

        public void Update(Teacher teacher, int id)
        {
            var entity = context.Teachers.Find(id);
            if (entity == null) 
            { 
                throw new NotImplementedException(); 
            }
            entity.NameId = teacher.NameId;
            entity.IsExternal = teacher.IsExternal;
            entity.Name = teacher.Name;
            entity.Email = teacher.Email;
            entity.PhoneNumber = teacher.PhoneNumber;
            context.SaveChanges();

        }
    }
}
