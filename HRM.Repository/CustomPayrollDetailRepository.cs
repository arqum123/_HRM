using HRM.Core.DataInterfaces;
using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HRM.Repository
{
    public partial class CustomPayrollDetailRepository : CustomPayrollDetailRepositoryBase, ICustomPayrollDetailRepository
    {
        public virtual List<CustomPayrollDetail> GetCustomPayrollDetailAmountByPayrollId(System.Int32? PayrollId, string SelectClause = null)
        {

            string sql = string.IsNullOrEmpty(SelectClause) ? GetCustomPayrollDetailSelectClause() : (string.Format("Select [{0}] ", SelectClause));
            sql += "FROM [CustomPayrollDetail] PD INNER JOIN PayrollPolicy PP ON PD.PayrollPolicyId = PP.Id INNER JOIN PayrollVariable PV ON PP.PayrollVariableId = PV.Id Where PD.PayrollId = @PayrollId ORDER BY PD.ID ";
            SqlParameter parameter = new SqlParameter("@PayrollID", PayrollId);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<CustomPayrollDetail>(ds, CustomPayrollDetailFromDataRow);
        }
    }

}
