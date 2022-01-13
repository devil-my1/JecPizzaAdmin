using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using JecPizza.Models;
using JecPizza.Services.BaseService;

namespace JecPizza.Services
{
    public class PurchaseDeliveryService : BaseServiceDOA
    {

        public IEnumerable<Delivery> GetAllDeliveries()
        {
            DataSet ds = new DataSet();

            string sql = "Select M.Username, M.Tel,  Delivery.* from Delivery join Purchase as P on Delivery.PurchaseId = P.PurchaseId join Member as M on P.MemberId = M.MemberId";

            SqlDataAdapter adapter = new SqlDataAdapter(sql, Connection);

            Connection.Open();
            if (adapter.Fill(ds, "delivery") == 0) { Connection.Close(); yield break; }
            Connection.Close();

            var dt = ds.Tables["delivery"];

            foreach (DataRow row in dt.Rows)
                yield return new Delivery()
                {
                    DeliveryId = row["DeliveryId"].ToString(),
                    UserName = row["Username"].ToString(),
                    UserPhone = row["Tel"].ToString(),
                    PurchaseId = row["PurchaseId"].ToString(),
                    Address = row["Address"].ToString(),
                    DeliveryDate = DateTime.Parse(row["DeliveryDate"].ToString()!, CultureInfo.InvariantCulture)
                };

        }

        public IEnumerable<Purchase> GetAllPurchases()
        {
            DataSet ds = new DataSet();

            string sql = "Select * from Purchase";

            SqlDataAdapter adapter = new SqlDataAdapter(sql, Connection);

            Connection.Open();
            if (adapter.Fill(ds, "purch") == 0) { Connection.Close(); yield break; }
            Connection.Close();

            var dt = ds.Tables["purch"];

            foreach (DataRow row in dt.Rows)
                yield return new Purchase()
                {
                    PurchaseId = row["PurchaseId"].ToString(),
                    MemberId = row["MemberId"].ToString(),
                    CardNum = row["CardNum"].ToString(),
                    CartId = row["CartId"].ToString(),
                    ToppingCartId = row["ToppingCartId"].ToString(),
                    Total = int.Parse(row["Total"].ToString() ?? "0"),
                    PurDate = DateTime.Parse(row["PurDate"].ToString()!, CultureInfo.InvariantCulture)
                };

        }

        public double GetTotalSales()
        {
            double total = 0;

            string sql = "Select SUM(total) as Total from Purchase";

            SqlDataAdapter adapter = new SqlDataAdapter(sql, Connection);
            DataSet ds = new DataSet();

            Connection.Open();
            if (adapter.Fill(ds, "purch") == 0) { Connection.Close(); return 0; }
            Connection.Close();

            DataTable dt = ds.Tables["purch"];

            total += double.Parse(dt.Rows[0]["Total"]?.ToString() ?? "0");


            return total;
        }

        public double GetTodaysTotalSales()
        {
            double todays_total = 0;
            string sql = "Select SUM(Total) as total from Purchase where PurDate = @date group by PurDate";

            SqlDataAdapter adapter = new SqlDataAdapter(sql, Connection);
            adapter.SelectCommand.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));

            DataSet ds = new DataSet();
            Connection.Open();
            if (adapter.Fill(ds, "purch") == 0) { Connection.Close(); return 0; }
            Connection.Close();

            DataTable dt = ds.Tables["purch"];

            todays_total += double.Parse(dt.Rows[0]["total"]?.ToString() ?? "0");

            return todays_total;
        }

        public IDictionary<DateTime, int> GetTotalSalesByMonth(int month)
        {
            IDictionary<DateTime, int> month_data = new Dictionary<DateTime, int>();

            string sql = "Select purDate, SUM(Total) as total from Purchase where PurDate like @month  group by PurDate";

            string corectFormatMonth = "-" + (month.ToString().Length == 2 ? month.ToString() : "0" + month) + "%";

            SqlDataAdapter adapter = new SqlDataAdapter(sql, Connection);

            adapter.SelectCommand.Parameters.AddWithValue("@month", DateTime.Now.Year + corectFormatMonth);

            DataSet ds = new DataSet();

            Connection.Open();
            if (adapter.Fill(ds, "purch") == 0) { Connection.Close(); return null; }
            Connection.Close();

            DataTable dt = ds.Tables["purch"];

            foreach (DataRow dt_row in dt.Rows)
                month_data.Add(DateTime.Parse(dt_row["purDate"].ToString() ?? DateTime.Now.ToString(CultureInfo.InvariantCulture)), int.Parse(dt_row["total"].ToString() ?? "0"));


            return month_data;
        }

        public IDictionary<string, int> GetTodaysPercentageOfGoods(string customDate = null)
        {
            IDictionary<string, int> data = new Dictionary<string, int>();
            List<string> carts_list = new List<string>();

            customDate ??= DateTime.Now.ToString("MM-dd");

            string sql = "Select CartId from Purchase where PurDate = @date";

            SqlDataAdapter adapter = new SqlDataAdapter(sql, Connection);
            adapter.SelectCommand.Parameters.AddWithValue("@date", DateTime.Now.Year + "-" + customDate.Replace('/', '-'));

            DataSet ds = new DataSet();
            Connection.Open();
            if (adapter.Fill(ds, "carts") == 0)
            {
                Connection.Close();
                return null;
            }


            DataTable dt = ds.Tables["carts"];

            foreach (DataRow dr in dt.Rows)
                carts_list.Add(dr[0].ToString());

            sql = "Select  GG.GroupName, SUM(Quantity) as Quantity from Cart join  Goods as G on G.GoodsId = Cart.GoodsId join GoodsGroup as GG on GG.GoodsGroupId = G.GoodsGroupId where CartId = @cart group by GG.GroupName";

            foreach (string cart in carts_list)
            {
                adapter = new SqlDataAdapter(sql, Connection);
                adapter.SelectCommand.Parameters.AddWithValue("@cart", cart);

                if (adapter.Fill(ds, "ggo") == 0)
                {
                    Connection.Close();
                    return null;
                }
                dt = ds.Tables["ggo"];

                foreach (DataRow dr in dt.Rows)
                {
                    if (!data.ContainsKey(dr[0]?.ToString() ?? string.Empty))
                    {
                        data.Add(dr[0]?.ToString() ?? string.Empty, int.Parse(dr[1]?.ToString() ?? string.Empty, NumberStyles.Integer));
                    }
                    else
                        data[dr[0]?.ToString() ?? string.Empty] += int.Parse(dr[1]?.ToString() ?? string.Empty, NumberStyles.Integer);
                }


            }

            Connection.Close();

            return data;
        }
    }
}