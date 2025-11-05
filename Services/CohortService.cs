using SharpSeer.Interfaces;
using SharpSeer.Models;

namespace SharpSeer.Services
{
    public class CohortService : IService<Cohort>
    {

        SharpSeerDbContext context;

        public CohortService(SharpSeerDbContext dbContext)
        {
            context = dbContext;
        }

        public void Create(Cohort? cohort)
        {
            if (cohort == null)
            {
                throw new NotImplementedException();
            }
            context.Cohorts.Add(cohort);
            context.SaveChanges();
        }

        public void Delete(Cohort cohort)
        {
            if (cohort == null)
            {
                throw new NotImplementedException();
            }
            context.Cohorts.Remove(cohort);
            context.SaveChanges();
        }

        public IEnumerable<Cohort> GetAll()
        {
            return context.Cohorts;
        }

        public void Update(Cohort cohort)
        {
            var entity = context.Cohorts.Find(cohort.Id);
            if (entity == null)
            {
                throw new NotImplementedException();
            }
            context.Entry(entity).CurrentValues.SetValues(cohort);
            context.SaveChanges();
        }
    }
}

