using BookingOfflineApp.Services.Interfaces;
using BookingOfflineApp.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace BookingOfflineApp.Functions.OrderItem
{
    public class OrderItemFunction
    {
        private readonly IOrderItemService _service;
        private readonly ILogger _log;

        public OrderItemFunction(IOrderItemService service, ILogger log)
        {
            this._service = service;
            this._log = log;
        }

        [FunctionName("CreateOrderItem")]
        public async Task<IActionResult> CreateOrderItem(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "order-items")] HttpRequest req)
        {
            var userId = req.Query["userId"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var model = JsonConvert.DeserializeObject<OrderItemModel>(requestBody);

            _service.CreateOrderItem(userId, model);
            return new OkResult();
        }

        [FunctionName("RemoveOrderItem")]
        public IActionResult RemoveOrderItem(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "order-items/{orderItemId}")] HttpRequest req, int orderItemId)
        {
            var userId = req.Query["userId"];

            if(_service.RemoveOrderItem(orderItemId, userId))
            {
                _log.LogInformation($"user {userId} deleted orderItem {orderItemId}.");
            }

            return new OkResult();
        }
    }
}
