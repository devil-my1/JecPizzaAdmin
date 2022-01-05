#nullable enable
using System;

namespace JecPizza.Models
{
    public class Goods : ICloneable
    {
        public string GoodsId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string GoodsGroupId { get; set; }
        public string Image { get; set; }
        public bool IsRecommend { get; set; }
        public bool IsNew { get; set; }
        public bool HasTopping { get; set; }

        public override bool Equals(object? obj) => (obj as Goods != null) && Equals((Goods)obj);

        protected bool Equals(Goods other) => GoodsId == other.GoodsId && Name == other.Name && Price == other.Price && GoodsGroupId == other.GoodsGroupId && Image == other.Image && IsRecommend == other.IsRecommend && IsNew == other.IsNew && HasTopping == other.HasTopping;

        public override int GetHashCode() => HashCode.Combine(GoodsId, Name, Price, GoodsGroupId, Image, IsRecommend, IsNew, HasTopping);

        public object Clone() =>
            new Goods()
            {
                GoodsId = GoodsId,
                Image = Image,
                GoodsGroupId = GoodsGroupId,
                HasTopping = HasTopping,
                IsNew = IsNew,
                IsRecommend = IsRecommend,
                Name = Name,
                Price = Price
            };

        public static bool operator ==(Goods left, Goods right) => Equals(left, right);
        public static bool operator !=(Goods left, Goods right) => !Equals(left, right);


    }
}