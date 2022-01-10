using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using JecPizza.Models;
using JecPizza.Services.BaseService;

namespace JecPizza.Services
{
    public class ReservationService : BaseServiceDOA
    {
        public IEnumerable<Reserve> GetAllReserve()
        {
            DataTable dt = null;

            Connection.Open();


            string sql = "Select * from Reservation";

            SqlDataAdapter adapter = new SqlDataAdapter(sql, Connection);
            DataSet ds = new DataSet();

            if (adapter.Fill(ds, "reserv") == 0) yield break;

            dt = ds.Tables["reserv"];

            Connection.Close();

            foreach (DataRow row in dt.Rows)
                yield return new Reserve()
                {
                    ReserveId = row["ReserveId"].ToString(),
                    Num = int.Parse(row["Num"].ToString() ?? "0"),
                    Date = DateTime.Parse(row["Date"].ToString() ?? DateTime.Now.ToShortDateString()),
                    IsReserved = bool.Parse(row["IsReserved"].ToString() ?? "true"),
                    TableNum = int.Parse(row["TableNum"].ToString() ?? "0"),
                    Tel = row["Tel"].ToString()
                };


        }

        public int UpdadateReservedTable()
        {
            int ret = 0;

            string sql = "Update Reservation set IsReserved = case when Date <= GETDATE() then 0 end where Date <= GETDATE() AND IsReserved != 0";

            SqlCommand cmd = new SqlCommand(sql, Connection);

            Connection.Open();
            ret = cmd.ExecuteNonQuery();
            Connection.Close();

            return ret;
        }

        public bool UpdateReserve(Reserve reserve)
        {
            bool res = false;

            Connection.Open();
            string sql = "Update Reservation set Tel = @tel, Date = @date, Num = @num, TableNum = @tableNum, IsReserved = @isRes where ReserveId = @rId";
            SqlCommand cmd = new SqlCommand(sql, Connection);
            cmd.Parameters.AddWithValue("@tel", reserve.Tel);
            cmd.Parameters.AddWithValue("@num", reserve.Num);
            cmd.Parameters.AddWithValue("@date", reserve.Date);
            cmd.Parameters.AddWithValue("@TableNum", reserve.TableNum);
            cmd.Parameters.AddWithValue("@isRes", reserve.IsReserved);
            cmd.Parameters.AddWithValue("@rId", reserve.ReserveId);

            res = cmd.ExecuteNonQuery() > 0;

            Connection.Close();

            return res;
        }

        public bool DeleteReserve(string rId)
        {
            bool res = false;

            Connection.Open();

            string sql = "Delete from Reservation where ReserveId = @rId";

            SqlCommand cmd = new SqlCommand(sql, Connection);
            cmd.Parameters.AddWithValue("@rId", rId);

            res = cmd.ExecuteNonQuery() > 0;

            Connection.Close();

            return res;
        }

        public bool InsertReservation(Reserve reserve)
        {
            bool res = false;

            Connection.Open();
            string sql = "Insert into Reservation Values(@rId, @tel, @date, @num, @tableNum, @isRes)";
            SqlCommand cmd = new SqlCommand(sql, Connection);
            cmd.Parameters.AddWithValue("@tel", reserve.Tel);
            cmd.Parameters.AddWithValue("@num", reserve.Num);
            cmd.Parameters.AddWithValue("@date", reserve.Date);
            cmd.Parameters.AddWithValue("@TableNum", reserve.TableNum);
            cmd.Parameters.AddWithValue("@isRes", reserve.IsReserved);
            cmd.Parameters.AddWithValue("@rId", reserve.ReserveId);

            res = cmd.ExecuteNonQuery() > 0;

            Connection.Close();

            return res;
        }

        public Reserve GetReserve(string rId)
        {
            Reserve ret_reserve = null;




            string sql = "Select * from Reservation where ReserveId = @rId";

            SqlDataAdapter adapter = new SqlDataAdapter(sql, Connection);
            adapter.SelectCommand.Parameters.AddWithValue("@rId", rId);

            DataSet ds = new DataSet();

            Connection.Open();
            if (adapter.Fill(ds, "reserve") == 0) return null;
            Connection.Close();

            DataTable dt = ds.Tables["reserve"];

            ret_reserve = new Reserve()
            {
                ReserveId = dt.Rows[0]["ReserveId"].ToString(),
                Num = int.Parse(dt?.Rows[0]["Num"].ToString() ?? "0"),
                Date = DateTime.Parse(dt.Rows[0]["Date"].ToString() ?? DateTime.Now.ToShortDateString()),
                IsReserved = bool.Parse(dt.Rows[0]["IsReserved"].ToString() ?? "true"),
                TableNum = int.Parse(dt.Rows[0]["TableNum"].ToString() ?? "0"),
                Tel = dt.Rows[0]["Tel"].ToString()
            };

            return ret_reserve;
        }
    }
}