using System;
using System.Data.SqlClient;

namespace JecPizza.Services.BaseService
{
    internal  abstract class BaseServiceDOA: IDisposable
    {
        private static readonly string _DBConn = Properties.Settings.Default.Db;
        private readonly SqlConnection _Connection = new SqlConnection(_DBConn);

        public void Dispose()
        {
            _Connection.Close();
        }
    }
}