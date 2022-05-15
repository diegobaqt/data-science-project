namespace DataAccess.Base.Interfaces
{
    public interface IUnitOfWork
    {
        void SaveChanges();

        IRepository<T> Repository<T>() where T : EntityBase;

        BaseContext GetContext();
    }
}