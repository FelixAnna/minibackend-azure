using System.Linq;
using System.Threading.Tasks;
using BookingOfflineApp.Entities;

namespace BookingOfflineApp.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order, int>
    {
        IQueryable<Order> FindAll(string userId);
        Task UpdateAsync(Order order);
    }
}