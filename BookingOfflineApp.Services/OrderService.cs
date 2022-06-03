using BookingOfflineApp.Entities;
using BookingOfflineApp.Repositories.Interfaces;
using BookingOfflineApp.Services.Interfaces;
using BookingOfflineApp.Services.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingOfflineApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IOrderRepository _orderRepo;
        private readonly IUserRepository<AlipayUser> _userRepository;
        private readonly IUserRepository<WechatUser> _weUserRepository;
        public OrderService(IOrderRepository orderRepo, IUserRepository<AlipayUser> userRepository,
            IUserRepository<WechatUser> weUserRepository,
            ILogger<OrderService> logger)
        {
            _orderRepo = orderRepo;
            _userRepository = userRepository;
            _weUserRepository = weUserRepository;
            _logger = logger;
        }

        public OrderResultModel CreateOrder<T>(string userId, OrderModel order)
        {
            var newOrder = _orderRepo.Create(new Order()
            {
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId,
                Options = JsonConvert.SerializeObject(order.Options)
            });

            if (typeof(T) == typeof(AlipayUser))
            {
                var user = _userRepository.FindById(userId);
                return OrderResultModel.FromOrder(newOrder, user);
            }
            else
            {
                var user = _weUserRepository.FindById(userId);
                return OrderResultModel.FromOrder(newOrder, user);
            }
        }

        public async Task<OrderResultModel> UpdateOrderAsync<T>(int id, string userId, OrderModel model)
        {
            var order = _orderRepo.FindById(id);
            if (order == null)
            {
                return null;
            }

            order.Options = JsonConvert.SerializeObject(model.Options);
            await _orderRepo.UpdateAsync(order);

            var newOrder = _orderRepo.FindById(id);

            if (typeof(T) == typeof(AlipayUser))
            {
                var user = _userRepository.FindById(userId);
                return OrderResultModel.FromOrder(newOrder, user);
            }
            else
            {
                var user = _weUserRepository.FindById(userId);
                return OrderResultModel.FromOrder(newOrder, user);
            }
        }

        public OrderResultModel GetOrder<T>(int orderId)
        {
            var order = _orderRepo.FindById(orderId);
            if (order == null)
            {
                return null;
            }

            var result = new OrderResultModel();
            var relatedUserIds = order.OrderItems.Select(x => x.CreatedBy).ToList();
            relatedUserIds.Add(order.CreatedBy);
            if (relatedUserIds.Any())
            {
                var alipayUsers = _userRepository.FindAll(relatedUserIds.ToArray()).ToList();
                var wechatUsers = _weUserRepository.FindAll(relatedUserIds.ToArray()).ToList();
                result = OrderResultModel.FromOrder(order, wechatUsers.ToArray(), alipayUsers.ToArray());

                //if (typeof(T) == typeof(AlipayUser))
                //{
                //    var users = _userRepository.FindAll(relatedUserIds.ToArray()).ToList();
                //    result = OrderResultModel.FromOrder(order, users.ToArray());
                //}
                //else
                //{
                //    var users = _weUserRepository.FindAll(relatedUserIds.ToArray()).ToList();
                //    result = OrderResultModel.FromOrder(order, users.ToArray());
                //}
            }
            else
            {
                result = OrderResultModel.FromOrder(order, new List<AlipayUser>().ToArray());
            }


            return result;
        }

        public OrderCollectionResultModel GetOrders<T>(string userId, DateTime? startDate = null, DateTime? endDate = null, int page = 1, int size = 10)
        {
            var orders = _orderRepo.FindAll(userId);
            if (startDate.HasValue)
            {
                orders = orders.Where(x => x.CreatedAt > startDate);
            }

            if (endDate.HasValue)
            {
                orders = orders.Where(x => x.CreatedAt < endDate.Value.AddDays(1));
            }

            var returnedOrders = orders?.Skip((page - 1) * size).Take(size).ToList();
            var relatedUserIds = returnedOrders.Select(x => x.CreatedBy).ToList();

            OrderCollectionResultModel results;
            if (relatedUserIds.Any())
            {
                var alipayUsers = _userRepository.FindAll(relatedUserIds.ToArray()).ToList();
                var wechatUsers = _weUserRepository.FindAll(relatedUserIds.ToArray()).ToList();

                results = new OrderCollectionResultModel()
                {
                    TotalCount = orders.Count(),
                    Orders = (from od in returnedOrders
                              let wechatUs = wechatUsers.FirstOrDefault(u => u.Id == od.CreatedBy)
                              let alipayUs = alipayUsers.FirstOrDefault(u => u.Id == od.CreatedBy)
                              select OrderCollectionItem.ToOrderCollectionItem(od, wechatUs, alipayUs)).OrderByDescending(x => x.CreatedAt).ToList()
                };

            }
            else
            {
                results = new OrderCollectionResultModel()
                {
                    TotalCount = orders.Count(),
                    Orders = (from od in returnedOrders
                              select OrderCollectionItem.ToOrderCollectionItem(od, null, null)).OrderByDescending(x => x.CreatedAt).ToList()
                };
            }

            return results;
        }

        public bool RemoveOrder(int orderId, string userId)
        {
            if (_orderRepo.Delete(orderId, userId))
            {
                _logger.LogInformation($"User {userId} deleted the order {orderId}");
                return true;
            }
            else
            {
                throw new Exception($"Failed to remove order: {orderId}");
            }
        }

        public async Task<bool> LockOrderAsync(int orderId, string userId)
        {
            var order = _orderRepo.FindById(orderId);
            if (order == null)
            {
                return false;
            }

            order.State = (int)OrderStatus.Locked;
            await _orderRepo.UpdateAsync(order);
            _logger.LogInformation($"User {userId} locked the order {orderId}");
            return true;
        }

        public async Task<bool> UnlockOrderAsync(int orderId, string userId)
        {
            var order = _orderRepo.FindById(orderId);
            if (order == null)
            {
                return false;
            }

            order.State = (int)OrderStatus.New;
            await _orderRepo.UpdateAsync(order);
            _logger.LogInformation($"User {userId} unlocked the order {orderId}");
            return true;
        }
    }
}
