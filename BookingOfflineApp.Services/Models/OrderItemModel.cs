using System.Collections.Generic;

namespace BookingOfflineApp.Services.Models
{
    public class OrderItemModel
    {
        public int OrderId { get; set; }
        
        public int? ProductId { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }

        public string Remark { get; set; }

        public OrderItemOptionModel[] Options { get; set; } = new List<OrderItemOptionModel>().ToArray();
    }

    public class OrderItemOptionModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
