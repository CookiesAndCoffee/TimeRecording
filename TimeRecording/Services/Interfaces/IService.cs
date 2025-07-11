namespace TimeRecording.Services.Interfaces
{
    public interface IService<E> where E : class
    {
        /// <summary>
        /// Saves the given entity to the database.
        /// </summary>
        /// <param name="entity"></param>
        void Save(E entity);

        /// <summary>
        ///  Return all entitys.
        /// </summary>
        /// <returns>List of Entity</returns>
        List<E> GetAll();

        /// <summary>
        /// Delete the entity
        /// </summary>
        /// <param name="entity"></param>
        void Delete(E entity);
    }
}
