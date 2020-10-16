using HRM.ADO.Core.Helper;
using HRM.ADO.Core.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;

namespace HRM.ADO.Core.Repository
{
    public class ShiftRepository
    {
        private SqlConnection conn;
        private void connect() { conn = SqlHelper._dbConnect; }

        public List<Shift> GetAllShifts()
        {
            connect();
            using (SqlCommand _cmd = new SqlCommand("dbo.GetAllShifts", conn))
            {
                _cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(_cmd);
                DataTable dt = new DataTable();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                da.Fill(dt);
                if (conn.State != ConnectionState.Closed)
                    conn.Close();

                return (from DataRow dr in dt.Rows
                        select new Shift()
                        {
                            Name = Convert.ToString(dr["Name"]),
                            Description = Convert.ToString(dr["Description"]),
                            BreakHour = Convert.ToDecimal(dr["BreakHour"]),
                            UserIP = Convert.ToString(dr["UserIP"])
                        }).ToList();
            }
        }
    }
}
