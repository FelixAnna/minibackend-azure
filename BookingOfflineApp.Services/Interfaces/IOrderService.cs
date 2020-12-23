using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookingOfflineApp.Entities;
using BookingOfflineApp.Services.Models;

namespace BookingOfflineApp.Services.Interfaces
{
    public interface IOrderService
    {
        OrderResultModel CreateOrder<T>(string userId, OrderModel order);
        OrderResultModel GetOrder<T>(int orderId);
        OrderCollectionResultModel GetOrders<T>(string userId, 
            DateTime? startDate, 
            DateTime? endDate, 
            int page = 1, 
            int size = 10);
        bool RemoveOrder(int orderId, string userId);
        Task<bool> UnlockOrderAsync(int orderId, string userId);
        Task<bool> LockOrderAsync(int orderId, string userId);
        Task<OrderResultModel> UpdateOrderAsync<T>(int id, string userId, OrderModel model);
    }
}