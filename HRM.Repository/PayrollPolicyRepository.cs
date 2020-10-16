using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using System.Data.SqlClient;
using System.Data;
using HRM.Core.Model;
namespace HRM.Repository
{
		
	public partial class PayrollPolicyRepository: PayrollPolicyRepositoryBase, IPayrollPolicyRepository
	{

        public DataSet GetPayrollPolicyInformationSP( System.Int32? UserId, System.Boolean? IsEarly, System.Boolean? IsLate,System.String StartDate,System.String EndDate)
        {
            string sql = "SELECT_Ticket";
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter() { ParameterName = "@IsEarly", Value = IsEarly });
            sqlparams.Add(new SqlParameter() { ParameterName = "@IsLate", Value = IsLate });
            sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = StartDate });
            sqlparams.Add(new SqlParameter() { ParameterName = "@EndDate", Value = EndDate });
            if (UserId.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@UserId", Value = UserId });
            return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());
        }
      
    }
	
	
}
