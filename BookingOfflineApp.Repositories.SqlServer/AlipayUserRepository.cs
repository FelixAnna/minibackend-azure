using BookingOfflineApp.Entities;
using BookingOfflineApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookingOfflineApp.Repositories.SqlServer
{
    public class AlipayUserRepository : IUserRepository<AlipayUser>
    {
        private readonly BODBContext _context;
        public AlipayUserRepository(BODBContext context)
        {
            _context = context;
        }

        public AlipayUser Create(AlipayUser user)
        {
            //db insert new user
            var result = _context.AlipayUsers.Add(user);
            _context.SaveChanges();
            return result.Entity;
        }

        public bool Delete(string key, string userId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<AlipayUser> FindAll(params string[] userIds)
        {
            var user = _context.AlipayUsers.Where(x => userIds.Contains(x.Id));
            return user;
        }

        public AlipayUser FindByOpenId(string alipayUserId)
        {
            var user = _context.AlipayUsers.FirstOrDefault(x => x.AlipayUserId == alipayUserId);
            return user;
        }

        public AlipayUser FindById(string userId)
        {
            var user = _context.AlipayUsers.FirstOrDefault(x => x.Id == userId);
            return user;
        }

        public async Task UpdateAsync(AlipayUser user)
        {
            _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
