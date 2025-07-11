using Microsoft.EntityFrameworkCore;
using TimeRecording.Models;
using TimeRecording.Services.Interfaces;
using TimeRecording.Setup;

namespace TimeRecording.Services
{
    public abstract class BaseService<E> : IService<E> where E : class
    {
        protected TimeRecordingDBContext _dbContext;

        public BaseService(TimeRecordingDBContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        protected abstract DbSet<E> GetSet();

        public virtual void Delete(E entity)
        {
            GetSet().Remove(entity);
            _dbContext.SaveChanges();
        }

        public List<E> GetAll()
        {
            return GetSet().ToList();
        }

        public void Save(E entity)
        {
            try
            {
                GetSet().Add(entity);
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                GetSet().Update(entity);
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        ///  Deletes all references to the given entity.         
        /// </summary>
        protected void DeleteReferences<T>(E entity) where T : class
        {
            if (entity is Entity identifiable)
            {
                var referenceSet = _dbContext.Set<T>();
                var foreignKey = identifiable.GetType().Name + "Id";
                var references = referenceSet.Where(reference => EF.Property<int>(reference, foreignKey) == identifiable.Id).ToList();
                foreach (var reference in references)
                    referenceSet.Remove(reference);
            }
        }
    }
}
