using System;

namespace JecPizza.Models
{
    public class Card
    {
        public string CardNum { get; set; }
        public string Type { get; set; }
        public string Holder { get; set; }
        public DateTime ExpDate { get; set; }
        public string Code { get; set; }
        public string MemberId { get; set; }
    }
}