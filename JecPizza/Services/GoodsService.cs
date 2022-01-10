using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using JecPizza.Models;
using JecPizza.Services.BaseService;

namespace JecPizza.Services
{
    public class GoodsService : BaseServiceDOA
    {
        public IEnumerable<Goods> GetAllGoods()
        {
            DataTable dt = null;
            string path = Environment.CurrentDirectory;

            string sql = "Select * from goods";

            Connection.Open();

            SqlDataAdapter adapter = new SqlDataAdapter(sql, Connection);
            DataSet ds = new DataSet();

            if (adapter.Fill(ds, "goods") <= 0) yield break;

            dt = ds.Tables["goods"];


            Connection.Close();

            foreach (DataRow data in dt.Rows)
            {
                Goods goods = new Goods()
                {
                    GoodsId = data[0].ToString()!,
                    Name = data["Name"].ToString()!,
                    Price = int.Parse(data["Price"].ToString()!, CultureInfo.InvariantCulture),
                    IsRecommend = bool.Parse(data["IsRecommend"].ToString()!),
                    GoodsGroupId = data["GoodsGroupId"].ToString()!,
                    HasTopping = bool.Parse(data["HasTopping"].ToString()!),
                    Image = path.Substring(0, path.IndexOf("\\bin", StringComparison.OrdinalIgnoreCase)) + "\\Content\\Images\\" + data["Image"].ToString(),
                    IsNew = bool.Parse(data["IsNew"].ToString()!),
                };
                yield return goods;

            }



        }

        public IDictionary<string, string> GetAllGoodsGroup()
        {
            IDictionary<string, string> ret_dict = null;


            Connection.Open();

            string sql = "Select * from GoodsGroup";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, Connection);
            DataSet ds = new DataSet();

            if (adapter.Fill(ds, "ggtable") == 0) return null;

            DataTable dt = ds.Tables["ggtable"];
            ret_dict = new Dictionary<string, string>();

            foreach (DataRow row in dt.Rows)
                ret_dict.Add(row["GroupName"].ToString() ?? "", row["GoodsGroupId"].ToString());
            

            Connection.Close();


            return ret_dict;
        }

        public Goods GetGoods(string gId)
        {
            Goods ret_goods = null;

            string sql = "Select * from goods where GoodsId = @gId";

            Connection.Open();

            SqlDataAdapter adapter = new SqlDataAdapter(sql, Connection);
            adapter.SelectCommand.Parameters.AddWithValue("@gId", gId);

            DataSet ds = new DataSet();

            if (adapter.Fill(ds, "goods") <= 0) return null;

            DataTable dt = ds.Tables["goods"];

            ret_goods = new Goods()
            {
                GoodsId = gId,
                Name = dt.Rows[0]["Name"].ToString()!,
                Price = int.Parse(dt.Rows[0]["Price"].ToString()!, CultureInfo.InvariantCulture),
                IsRecommend = bool.Parse(dt.Rows[0]["IsRecommend"].ToString()!),
                GoodsGroupId = dt.Rows[0]["GoodsGroupId"].ToString()!,
                HasTopping = bool.Parse(dt.Rows[0]["HasTopping"].ToString()!),
                Image = dt.Rows[0]["Image"].ToString()!,
                IsNew = bool.Parse(dt.Rows[0]["IsNew"].ToString()!),

            };

            Connection.Close();

            return ret_goods;
        }

        public bool EditGoods(Goods goods)
        {
            var ret = false;

            Connection.Open();

            string sql = @"Update Goods set Name = @name, Price = @price, GoodsGroupId = @goodsGroupId, Image = @image, IsRecommend = @isRec, IsNew = @isNew, HasTopping = @hasTopping where GoodsId = @gId";
            SqlCommand cmd = new SqlCommand(sql, Connection);
            cmd.Parameters.AddWithValue("@name", goods.Name);
            cmd.Parameters.AddWithValue("@price", goods.Price);
            cmd.Parameters.AddWithValue("@goodsGroupId", goods.GoodsGroupId);
            cmd.Parameters.AddWithValue("@image", goods.Image.Remove(0, goods.Image.IndexOf("Images\\", StringComparison.OrdinalIgnoreCase) + 7));
            cmd.Parameters.AddWithValue("@isRec", goods.IsRecommend);
            cmd.Parameters.AddWithValue("@isNew", goods.IsNew);
            cmd.Parameters.AddWithValue("@hasTopping", goods.HasTopping);
            cmd.Parameters.AddWithValue("@gId", goods.GoodsId);

            ret = cmd.ExecuteNonQuery() > 0;

            Connection.Close();

            return ret;
        }

        public bool DeleteGoods(Goods goods)
        {
            var ret = false;

            Connection.Open();

            string sql = @"Delete from Goods where GoodsId = @gId";

            SqlCommand cmd = new SqlCommand(sql, Connection);

            cmd.Parameters.AddWithValue("@gId", goods.GoodsId);

            ret = cmd.ExecuteNonQuery() > 0;

            Connection.Close();

            return ret;
        }

        public bool InsertGoods(Goods goods)
        {
            bool ret = false;

            Connection.Open();

            string sql = "Insert into Goods Values(@gId,@name,@price,@goodsGroupId,@image,@isRec,@isNew,@hasTopping)";
            SqlCommand cmd = new SqlCommand(sql, Connection);
            cmd.Parameters.AddWithValue("@name", goods.Name);
            cmd.Parameters.AddWithValue("@price", goods.Price);
            cmd.Parameters.AddWithValue("@goodsGroupId", goods.GoodsGroupId);
            cmd.Parameters.AddWithValue("@image", goods.Image.Remove(0, goods.Image.IndexOf("Images\\", StringComparison.OrdinalIgnoreCase) + 7));
            cmd.Parameters.AddWithValue("@isRec", goods.IsRecommend);
            cmd.Parameters.AddWithValue("@isNew", goods.IsNew);
            cmd.Parameters.AddWithValue("@hasTopping", goods.HasTopping);
            cmd.Parameters.AddWithValue("@gId", goods.GoodsId);

            ret = cmd.ExecuteNonQuery() > 0;

            Connection.Close();

            return ret;
        }
    }
}