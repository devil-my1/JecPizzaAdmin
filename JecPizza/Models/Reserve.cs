using System;

namespace JecPizza.Models
{
    public class Reserve : ICloneable
    {
        public string ReserveId { get; set; }
        public string Tel { get; set; }
        public DateTime Date { get; set; }
        public int Num { get; set; }
        public int TableNum { get; set; }
        public bool IsReserved { get; set; }

        public override bool Equals(object? obj) => (obj as Reserve != null) && Equals((Reserve)obj);

        protected bool Equals(Reserve other)
        {
            return ReserveId == other.ReserveId && Tel == other.Tel && Date.Equals(other.Date) && Num == other.Num && TableNum == other.TableNum && IsReserved == other.IsReserved;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ReserveId, Tel, Date, Num, TableNum, IsReserved);
        }

        public static bool operator ==(Reserve left, Reserve right) { return Equals(left, right); }
        public static bool operator !=(Reserve left, Reserve right) { return !Equals(left, right); }

        public object Clone() => new Reserve() { TableNum = TableNum, Tel = Tel, Date = Date, IsReserved = IsReserved, Num = Num, ReserveId = ReserveId };
    }
}