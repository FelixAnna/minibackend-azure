using System;

namespace BookingOfflineApp.Entities
{
    public class AlipayUser
    {
        public string Id { get; set; }

        public string AlibabaUserId { get; set; }

        public string AlipayUserId { get; set; }

        public string AlipayName { get; set; }
        public string AlipayPhoto { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
