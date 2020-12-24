using BookingOfflineApp.Entities;
using BookingOfflineApp.Services.Interfaces;
using BookingOfflineApp.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BookingOfflineApp.Functions.Order
{
    public class OrderFunction
    {
        private readonly IOrderService _service;
        private readonly ILogger _log;

        public OrderFunction(IOrderService service, ILogger log)
        {
            this._service = service;
            this._log = log;
        }

        private bool isAlipayUser(HttpRequest req)
        {
            return req.Query.ContainsKey("alibabaUserId");
        }

        /// <summary>
        /// Create a new order (with no orderItem)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [FunctionName("CreateOrder")]
        public async Task<ActionResult> CreateOrder([HttpTrigger(AuthorizationLevel.Function, "post", Route = "orders")] HttpRequest req)
        {
            var userId = req.Query["userId"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var model = JsonConvert.DeserializeObject<OrderModel>(requestBody);
            OrderResultModel order;
            if (isAlipayUser(req))
            {
                order = _service.CreateOrder<AlipayUser>(userId, model);
            }
            else
            {
                order = _service.CreateOrder<WechatUser>(userId, model);
            }

            return new OkObjectResult(order);
        }

        /// <summary>
        /// Create a new order (with no orderItem)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [FunctionName("UpdateOrder")]
        public async Task<ActionResult> UpdateOrder([HttpTrigger(AuthorizationLevel.Function, "post", Route = "orders/options")] HttpRequest req)
        {
            int Id = int.Parse(req.Query["id"]);
            var userId = req.Query["userId"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var model = JsonConvert.DeserializeObject<OrderModel>(requestBody);
            OrderResultModel order; 
            if (isAlipayUser(req))
            {
                order = await _service.UpdateOrderAsync<AlipayUser>(Id, userId, model);
            }
            else
            {
                order = await _service.UpdateOrderAsync<WechatUser>(Id, userId, model);
            }

            if (order == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(order);
        }

        /// <summary>
        /// Remove a order and it related orderItem
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [FunctionName("RemoveOrder")]
        public ActionResult RemoveOrder([HttpTrigger(AuthorizationLevel.Function, "post", Route = "orders/remove")] HttpRequest req)               
        {
            int orderId = int.Parse(req.Query["orderId"]);
            var userId = req.Query["userId"];
            if(_service.RemoveOrder(orderId, userId))
            {
                _log.LogInformation($"user {userId} removed order {orderId}.");
            }

            return new OkResult();
        }

        /// <summary>
        /// Lock an order if it is unlockeds
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [FunctionName("LockOrder")]
        public async Task<ActionResult> LockOrder([HttpTrigger(AuthorizationLevel.Function, "post", Route = "orders/lock")] HttpRequest req)
        {
            int orderId = int.Parse(req.Query["orderId"]);
            var userId = req.Query["userId"];
            if(await _service.LockOrderAsync(orderId, userId))
            {
                _log.LogInformation($"user {userId} locked order {orderId}.");
            }
            return new OkResult();
        }

        /// <summary>
        /// Unlock an order if it is locked
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [FunctionName("UnlockOrder")]
        public async Task<ActionResult> UnlockOrder([HttpTrigger(AuthorizationLevel.Function, "post", Route = "orders/unlock")] HttpRequest req)
        {
            int orderId = int.Parse(req.Query["orderId"]);
            var userId = req.Query["userId"];

            await _service.UnlockOrderAsync(orderId, userId);
            return new OkResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [FunctionName("GetOrder")]
        public ActionResult GetOrder([HttpTrigger(AuthorizationLevel.Function, "get", Route = "orders")] HttpRequest req)
        {
            int orderId = int.Parse(req.Query["orderId"]);
            OrderResultModel order;
            if (isAlipayUser(req))
            {
                order = _service.GetOrder<AlipayUser>(orderId);
            }
            else
            {
                order = _service.GetOrder<WechatUser>(orderId);
            }

            if (order != null)
            {
                return new OkObjectResult(order);
            }

            return new NotFoundResult();
        }

        [FunctionName("GetOrders")]
        public ActionResult GetOrders([HttpTrigger(AuthorizationLevel.Function, "get", Route = "orders/list")] HttpRequest req)
        {
            int.TryParse(req.Query["page"], out int page);
            int.TryParse(req.Query["size"], out int size);
            page = page == 0 ? 1 : page;
            size = size == 0 ? 10 : size;

            DateTime? startDate = null, endDate = null;
            if (req.Query.ContainsKey("startDate"))
            {
                startDate = Convert.ToDateTime(req.Query["startDate"]);
            }

            if (req.Query.ContainsKey("endDate"))
            {
                endDate = Convert.ToDateTime(req.Query["endDate"]);
            }

            var userId = req.Query["userId"];
            OrderCollectionResultModel orders;
            if (isAlipayUser(req))
            {
                orders = _service.GetOrders<AlipayUser>(userId, startDate, endDate, page, size);
            }
            else
            {
                orders = _service.GetOrders<WechatUser>(userId, startDate, endDate, page, size);
            }

            if (orders.TotalCount > 0)
            {
                return new OkObjectResult(orders);
            }

            return new NotFoundResult();
        }
    }
}
