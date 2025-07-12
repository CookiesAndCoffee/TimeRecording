using Microsoft.EntityFrameworkCore;
using TimeRecording.Models;
using TimeRecording.Services.Interfaces;
using TimeRecording.Setup;

namespace TimeRecording.Services
{
    /// <summary>
    /// Provides basic CRUD operations for entities in the TimeRecording database.
    /// </summary>
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
            Save<E>(entity, GetSet());
        }

        private void Save<T>(T entity, DbSet<T> set) where T : class
        {
            // I know this should be done another way to fully rollback every change on failure
            // especially when saving references, but this is the fastest way and currently good enough.
            try
            {
                set.Add(entity);
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                set.Update(entity);
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        ///  Saves all references to the given entity.         
        /// </summary>
        protected void SavesReferences<T>(E entity, List<T> references) where T : class
        {
            if (entity is Entity identifiable && identifiable.Id > 0)
            {
                var referenceService = _dbContext.Set<T>();
                var foreignKey = identifiable.GetType().Name + "Id";
                foreach (var reference in references)
                {
                    var propertyInfo = reference.GetType().GetProperty(foreignKey);
                    if (propertyInfo != null && propertyInfo.CanWrite)
                    {
                        propertyInfo.SetValue(reference, identifiable.Id);
                    }
                    Save<T>(reference, referenceService);
                }
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
