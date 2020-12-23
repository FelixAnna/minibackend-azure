using System.Linq;
using BookingOfflineApp.Entities;

namespace BookingOfflineApp.Repositories.Interfaces
{
    public interface IOrderItemRepository : IRepository<OrderItem, int>
    {
        IQueryable<OrderItem> FindAll(int orderId);
    }
}