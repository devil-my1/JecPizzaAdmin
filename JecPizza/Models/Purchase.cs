using System;

namespace JecPizza.Models
{
    public class Purchase
    {
        public string PurchaseId { get; set; }
        public string CartId { get; set; }
        public string MemberId { get; set; }
        public string ToppingCartId { get; set; }
        public string CardNum { get; set; }
        public DateTime PurDate { get; set; }
        public int Total { get; set; }
    }
}