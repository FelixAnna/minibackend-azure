using BookingOfflineApp.Entities;
using System.Linq;

namespace BookingOfflineApp.Repositories.Interfaces
{
    public interface IOrderItemRepository : IRepository<OrderItem, int>
    {
        IQueryable<OrderItem> FindAll(int orderId);
    }
}