using BookingOfflineApp.Entities;
using BookingOfflineApp.Repositories.Interfaces;
using BookingOfflineApp.Services.Interfaces;
using BookingOfflineApp.Services.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace BookingOfflineApp.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly ILogger<OrderItemService> _logger;
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderItemRepository _orderItemRepo;

        public OrderItemService(IOrderRepository orderRepo, IOrderItemRepository productRepo, ILogger<OrderItemService> logger)
        {
            _orderRepo = orderRepo;
            _orderItemRepo = productRepo;
            _logger = logger;
        }

        public OrderItem CreateOrderItem(string userId, OrderItemModel item)
        {
            if (_orderRepo.FindById(item.OrderId)?.State == (int)OrderStatus.New)
            {
                var newOrderItem = _orderItemRepo.Create(new OrderItem()
                {
                    Name = item.Name,
                    Price = item.Price,
                    Remark = item.Remark,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                    OrderId = item.OrderId,
                    OrderItemOptions = item.Options.Select(x => new OrderItemOption()
                    {
                        Name = x.Name,
                        Value = x.Value
                    }).ToList()
                });

                return newOrderItem;
            }
            else
            {
                throw new Exception($"Failed to create orderItem for order: {item.OrderId}");
            }
        }

        public bool RemoveOrderItem(int orderItemId, string userId)
        {
            var item = _orderItemRepo.FindById(orderItemId);
            if (_orderRepo.FindById(item?.OrderId ?? 0)?.State == (int)OrderStatus.New
                && _orderItemRepo.Delete(orderItemId, userId))
            {
                _logger.LogInformation($"User {userId} deleted the order {orderItemId}");
                return true;
            }
            else
            {
                throw new Exception($"Failed to remove orderItem: {orderItemId}");
            }
        }
    }
}
