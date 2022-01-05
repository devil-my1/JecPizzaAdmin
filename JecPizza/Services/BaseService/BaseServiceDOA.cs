using System;
using System.Data.SqlClient;

namespace JecPizza.Services.BaseService
{
    public abstract class BaseServiceDOA : IDisposable
    {
        protected static string DBStr { get; } = Properties.Settings.Default.Db;
        protected SqlConnection Connection { get; set; } = new SqlConnection(DBStr);


        public void Dispose()
        {
            Connection.Close();
        }
    }
}