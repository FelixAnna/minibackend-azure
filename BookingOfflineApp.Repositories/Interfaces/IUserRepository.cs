using BookingOfflineApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
