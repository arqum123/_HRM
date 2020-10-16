using HRM.Core.DataInterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HRM.Repository
{
    public partial class TicketRepository : TicketRepositoryBase, ITicketRepository
    {
        public DataSet GetPendingTicketsDetail()
        {
            string sql = "SELECT_PendingTickets";  //SELECT_Payslip
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());
        }
        public DataSet GetTicketByUserIdAndDateRangeSP(System.Int32?UserId,System.DateTime StartDate,System.DateTime EndDate)
        {
            string sql = "SELECT_TicketByUserIdAndDateRange";  //SELECT_Payslip
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = StartDate });
            sqlparams.Add(new SqlParameter() { ParameterName = "@EndDate", Value = EndDate });
            if (UserId.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@UserId", Value = UserId });
            return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());
        }
        
    }
}
