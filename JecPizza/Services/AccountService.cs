using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using JecPizza.Models;
using JecPizza.Services.BaseService;

namespace JecPizza.Services
{
    public class AccountService : BaseServiceDOA
    {
        public IEnumerable<Member> GetMembers()
        {
            DataSet ds = new DataSet();
            DataTable dt;

            string sql = "Select * from Member";

            SqlDataAdapter adapter = new SqlDataAdapter(sql, Connection);

            Connection.Open();
            if (adapter.Fill(ds, "memb") == 0) { Connection.Close(); yield break; }
            Connection.Close();

            dt = ds.Tables["memb"];

            foreach (DataRow dr in dt.Rows)
                yield return new Member()
                {
                    MemberId = dr["MemberId"].ToString(),
                    UserName = dr["Username"].ToString(),
                    NickName = dr["Nickname"].ToString(),
                    Address = dr["Address"].ToString(),
                    Email = dr["Email"].ToString(),
                    Image = dr["Image"].ToString(),
                    PhoneNumber = dr["Tel"].ToString(),
                    Password = dr["Password"].ToString(),
                    Sex = !(bool)dr["Sex"]?.ToString().Equals("M", StringComparison.OrdinalIgnoreCase),
                    Dob = DateTime.Parse(dr["DoB"].ToString() ?? DateTime.Now.ToString(CultureInfo.InvariantCulture)),
                    ZipCode = dr["PostCode"].ToString(),
                    RegisterDate = DateTime.Parse(dr["RegisterDate"].ToString() ?? DateTime.Now.ToString(CultureInfo.InvariantCulture))

                };

        }

        public bool DeleteAccount(Member member)
        {
            bool ret_res = false;

            string sql = "Delete from Member where MemberId = @membId";

            SqlCommand cmd = new SqlCommand(sql, Connection);
            cmd.Parameters.AddWithValue("@membId", member.MemberId);

            Connection.Open();
            ret_res = cmd.ExecuteNonQuery() > 0;
            Connection.Close();

            return ret_res;
        }
    }
}