using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using System.Data.SqlClient;
using System.Data;

namespace HRM.Repository
{
		
	public partial class PayrollDetailRepository: PayrollDetailRepositoryBase, IPayrollDetailRepository
	{
        public virtual List<PayrollDetail> GetPayrollDetailAmountByPayrollId(System.Int32? PayrollId, string SelectClause = null)
        {

            string sql = string.IsNullOrEmpty(SelectClause) ? GetPayrollDetailSelectClause() : (string.Format("Select [{0}] ", SelectClause));
            sql += "FROM [PayrollDetail] PD INNER JOIN PayrollPolicy PP ON PD.PayrollPolicyId = PP.Id INNER JOIN PayrollVariable PV ON PP.PayrollVariableId = PV.Id Where PD.PayrollId = @PayrollId ORDER BY PD.ID ";
            SqlParameter parameter = new SqlParameter("@PayrollID", PayrollId);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<PayrollDetail>(ds, PayrollDetailFromDataRow);
        }
    }


}
