using System.Collections.Generic;

namespace BookingOfflineApp.Services.Models
{
    public class OrderModel
    {
        public IList<OrderOptionModel> Options { get; set; }
    }

    public class OrderOptionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Default { get; set; }
        public string Order { get; set; }
    }
}
