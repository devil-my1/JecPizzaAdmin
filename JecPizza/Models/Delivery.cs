using System;

namespace JecPizza.Models
{
    public class Delivery
    {
        public string DeliveryId { get; set; }
        public string MemberId { get; set; }
        public string Address { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}