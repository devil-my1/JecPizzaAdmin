using System;

namespace JecPizza.Models
{
    public class Reserve
    {
        public string ReserveId { get; set; }
        public string Tel { get; set; }
        public DateTime Date { get; set; }
        public int Num { get; set; }
        public int TableNum { get; set; }
        public bool IsReserved { get; set; }
    }
}