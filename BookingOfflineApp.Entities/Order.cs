using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingOfflineApp.Entities
{
    public class Order
    {
        public Order()
        {
            State = (int)OrderStatus.New;
        }

        [Key]
        public int OrderId { get; set; }

        public int State { get; set; }
        public string Options { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
