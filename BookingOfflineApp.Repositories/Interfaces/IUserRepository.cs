using System.Linq;
using System.Threading.Tasks;

namespace BookingOfflineApp.Repositories.Interfaces
{
    public interface IUserRepository<TEntity> : IRepository<TEntity, string>
    {
        IQueryable<TEntity> FindAll(params string[] userIds);

        Task UpdateAsync(TEntity user);
        TEntity FindByOpenId(string openId);
    }
}
