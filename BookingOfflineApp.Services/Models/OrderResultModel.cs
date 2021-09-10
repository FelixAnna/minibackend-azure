using BookingOfflineApp.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookingOfflineApp.Services.Models
{
    public class OrderResultModel
    {
        public int OrderId { get; set; }

        public IList<OrderOptionModel> Options { get; set; }

        public int State { get; set; }
        public List<OrderItemResultModel> ProductList { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string OwnerName { get; set; }

        public static OrderResultModel FromOrder(Order order, WechatUser[] wechatUsers,  AlipayUser[] alipayUsers)
        {
            var result = new OrderResultModel()
            {
                OrderId = order.OrderId,
                Options = JsonConvert.DeserializeObject<IList<OrderOptionModel>>(order.Options),
                State = order.State,
       
                CreatedAt = order.CreatedAt.ToUniversalTime(),
                CreatedBy = order.CreatedBy,
            };

            var wechatOwner = wechatUsers.FirstOrDefault(x => x.Id == order.CreatedBy);
            if (null != wechatOwner)
            {
                result.OwnerName = wechatOwner.NickName;
            }
            else
            {
                var alipayOwner = alipayUsers.FirstOrDefault(x => x.Id == order.CreatedBy);
                result.OwnerName = alipayOwner?.AlipayName;
            }

            result.ProductList = order.OrderItems?.Select(x => OrderItemResultModel.FromOrderItem(x, wechatUsers, alipayUsers)).OrderByDescending(x => x.CreatedAt).ToList() ?? new List<OrderItemResultModel>();

            return result;
        }

        public static OrderResultModel FromOrder(Order order, params AlipayUser[] users)
        {
            var result = new OrderResultModel()
            {
                OrderId = order.OrderId,
                Options= JsonConvert.DeserializeObject<IList<OrderOptionModel>>(order.Options),
                State = order.State,
                ProductList = order.OrderItems?.Select(x => OrderItemResultModel.FromOrderItem(x, users)).OrderByDescending(x=>x.CreatedAt).ToList() ?? new List<OrderItemResultModel>(),
                CreatedAt = order.CreatedAt.ToUniversalTime(),
                CreatedBy = order.CreatedBy,
                OwnerName = users.FirstOrDefault(x => x.Id == order.CreatedBy)?.AlipayName
            };

            return result;
        }

        public static OrderResultModel FromOrder(Order order, params WechatUser[] users)
        {
            var result = new OrderResultModel()
            {
                OrderId = order.OrderId,
                Options = JsonConvert.DeserializeObject<IList<OrderOptionModel>>(order.Options),
                State = order.State,
                ProductList = order.OrderItems?.Select(x => OrderItemResultModel.FromOrderItem(x, users)).OrderByDescending(x => x.CreatedAt).ToList() ?? new List<OrderItemResultModel>(),
                CreatedAt = order.CreatedAt.ToUniversalTime(),
                CreatedBy = order.CreatedBy,
                OwnerName = users.FirstOrDefault(x => x.Id == order.CreatedBy)?.NickName
            };

            return result;
        }
    }

    public class OrderItemResultModel
    {
        public int OrderItemId { get; set; }

        public int? ProductId { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }

        public string Remark { get; set; }

        public List<OrderItemOptionsResultModel> Options { get; set; }

        public DateTime CreatedAt { get; set; }
        public string OwnerId { get; set; }
        public string OwnerAvatar { get; set; }
        public string OwnerName { get; set; }
        

        public static OrderItemResultModel FromOrderItem(OrderItem iten, WechatUser[] wechatUsers, AlipayUser[] alipayUsers)
        {
            var result = new OrderItemResultModel()
            {
                OrderItemId = iten.OrderItemId,
                Name = iten.Name,
                Price = iten.Price,
                Remark = iten.Remark,
                Options = iten.OrderItemOptions?.Select(OrderItemOptionsResultModel.FromOrderItenOption).ToList() ?? new List<OrderItemOptionsResultModel>(),
                CreatedAt = iten.CreatedAt.ToUniversalTime(),
                OwnerId = iten.CreatedBy,
                
            };

            var wechatOwner = wechatUsers.FirstOrDefault(x => x.Id == iten.CreatedBy);
            if(null != wechatOwner)
            {
                result.OwnerAvatar = wechatOwner.AvatarUrl;
                result.OwnerName = wechatOwner.NickName;
            }
            else
            {
                var alipayOwner = alipayUsers.FirstOrDefault(x => x.Id == iten.CreatedBy);
                result.OwnerAvatar = alipayOwner?.AlipayPhoto;
                result.OwnerName = alipayOwner?.AlipayName;
            }
            
            

            return result;
        }
        public static OrderItemResultModel FromOrderItem(OrderItem iten, IEnumerable<AlipayUser> users)
        {
            var result = new OrderItemResultModel()
            {
                OrderItemId = iten.OrderItemId,
                Name = iten.Name,
                Price = iten.Price,
                Remark = iten.Remark,
                Options = iten.OrderItemOptions?.Select(OrderItemOptionsResultModel.FromOrderItenOption).ToList() ?? new List<OrderItemOptionsResultModel>(),
                CreatedAt = iten.CreatedAt.ToUniversalTime(),
                OwnerId = iten.CreatedBy,
                OwnerAvatar = users.FirstOrDefault(x => x.Id == iten.CreatedBy)?.AlipayPhoto,
                OwnerName = users.FirstOrDefault(x => x.Id == iten.CreatedBy)?.AlipayName
            };

            return result;
        }

        public static OrderItemResultModel FromOrderItem(OrderItem iten, IEnumerable<WechatUser> users)
        {
            var result = new OrderItemResultModel()
            {
                OrderItemId = iten.OrderItemId,
                Name = iten.Name,
                Price = iten.Price,
                Remark = iten.Remark,
                Options = iten.OrderItemOptions?.Select(OrderItemOptionsResultModel.FromOrderItenOption).ToList() ?? new List<OrderItemOptionsResultModel>(),
                CreatedAt = iten.CreatedAt.ToUniversalTime(),
                OwnerId = iten.CreatedBy,
                OwnerAvatar = users.FirstOrDefault(x => x.Id == iten.CreatedBy)?.AvatarUrl,
                OwnerName = users.FirstOrDefault(x => x.Id == iten.CreatedBy)?.NickName
            };

            return result;
        }
    }

    public class OrderItemOptionsResultModel
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public static OrderItemOptionsResultModel FromOrderItenOption(OrderItemOption option)
        {
            var result = new OrderItemOptionsResultModel()
            {
                Name = option.Name,
                Value = option.Value
            };

            return result;
        }
    }
}
