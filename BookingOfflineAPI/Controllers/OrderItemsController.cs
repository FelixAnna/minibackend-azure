using BookingOfflineApp.Services.Interfaces;
using BookingOfflineApp.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BookingOfflineApp.Web.Controllers
{
    [Authorize]
    [Route("order-items")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService _service;
        private readonly ILogger<OrderItemsController> _log;

        public OrderItemsController(IOrderItemService service, ILogger<OrderItemsController> log)
        {
            this._service = service;
            this._log = log;
        }

        /// <summary>
        /// Add a new orderItem to an existing order
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult CreateOrderItem([FromBody] OrderItemModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _service.CreateOrderItem(userId, model);
            return Ok();
        }

        /// <summary>
        /// Remove orderItem
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <returns></returns>
        [HttpDelete("{orderItemId}")]
        public ActionResult RemoveOrderItem([FromRoute] int orderItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (_service.RemoveOrderItem(orderItemId, userId))
            {
                _log.LogInformation($"user {userId} deleted orderItem {orderItemId}.");
            }

            return Ok();
        }
    }
}
