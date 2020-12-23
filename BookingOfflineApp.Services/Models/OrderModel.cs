using System;
using System.Collections.Generic;
using System.Text;

namespace BookingOfflineApp.Services.Models
{
    public class OrderModel
    {
        public IList<OrderOptionModel> Options { get; set; }
    }

    public class OrderOptionModel
    {
        //{id: 1, name: '加饭', type:'bool', default: false, order: 1},
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Default { get; set; }
        public string Order { get; set; }
    }
}
