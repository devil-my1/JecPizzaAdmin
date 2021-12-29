namespace JecPizza.Models
{
    public class Goods
    {
        public string GoodsId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string GoodsGroupId { get; set; }
        public string Image { get; set; }
        public bool IsRecommend { get; set; }
        public bool IsNew { get; set; }
        public bool HasTopping { get; set; }
    }
}