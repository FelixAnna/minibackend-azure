using System.Linq;

namespace BookingOfflineApp.Repositories.Interfaces
{
    public interface IRepository<TEntity, TKey>
    {
        TEntity FindById(TKey key);
        TEntity Create(TEntity item);
        bool Delete(TKey key, string userId);
    }
}
