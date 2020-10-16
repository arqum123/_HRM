using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HRM.ADO.Core.Helper
{
    internal class SqlHelper
    {
        private static string connectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Conn"].ToString();
            }
        }

        internal static SqlConnection _dbConnect
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }
    }
}
