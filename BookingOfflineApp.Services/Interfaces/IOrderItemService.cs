using BookingOfflineApp.Entities;
using BookingOfflineApp.Services.Models;

namespace BookingOfflineApp.Services.Interfaces
{
    public interface IOrderItemService
    {
        OrderItem CreateOrderItem(string userId, OrderItemModel item);
        bool RemoveOrderItem(int orderItemId, string userId);
    }
}