using System;

namespace JecPizza.Models
{
    public class Delivery
    {
        public string DeliveryId { get; set; }
        public string PurchaseId { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string Address { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}