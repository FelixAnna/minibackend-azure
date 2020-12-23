using BookingOfflineApp.Entities;
using BookingOfflineApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookingOfflineApp.Repositories.SqlServer
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly BODBContext _context;
        public OrderItemRepository(BODBContext context)
        {
            _context = context;
        }

        public OrderItem FindById(int key)
        {
            return _context.OrderItems
                .Include(o => o.OrderItemOptions)
                .FirstOrDefault(x => x.OrderItemId == key);
        }

        public OrderItem Create(OrderItem item)
        {
            var newItem = _context.OrderItems.Add(item);
            _context.SaveChanges();
            return newItem.Entity;
        }

        public bool Delete(int key, string userId)
        {
            var item = _context.OrderItems
                    .Include(a => a.OrderItemOptions)
                    .Include(a => a.Order)
                    .FirstOrDefault(x => x.OrderItemId == key && (x.Order.CreatedBy == userId || x.CreatedBy == userId));
            if (item == null)
            {
                return false;
            }

            _context.Remove(item);
            _context.SaveChanges();
            return true;
        }

        public IQueryable<OrderItem> FindAll(int orderId)
        {
            return _context.OrderItems
                .Include(o => o.OrderItemOptions)
                .Where(x => x.OrderId == orderId);
        }
    }
}
