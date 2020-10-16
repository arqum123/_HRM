using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using System.Data;
using System.Data.SqlClient;

namespace HRM.Repository
{
		
	public partial class LeaveRepository: LeaveRepositoryBase, ILeaveRepository
	{
        public DataSet GetApprovedLeavesByDateRangeSP(int? UserID, int? DepartmentID, DateTime StartDate, DateTime EndDate)
        {
            string sql = "GetAllApprovedLeavesByDepartmentUserNameAndDateRange";  
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            if (UserID.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@UserID", Value = UserID });
            if (DepartmentID.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@DepartmentID", Value = DepartmentID });
            sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = StartDate });
            sqlparams.Add(new SqlParameter() { ParameterName = "@EndDate", Value = EndDate });
            return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());
        }
        public DataSet GetPendingLeavesByDateRangeSP(int? UserID, int? DepartmentID, DateTime StartDate, DateTime EndDate)
        {
            string sql = "GetAllPendingLeavesByDepartmentUserNameAndDateRange";
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            if (UserID.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@UserID", Value = UserID });
            if (DepartmentID.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@DepartmentID", Value = DepartmentID });
            sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = StartDate });
            sqlparams.Add(new SqlParameter() { ParameterName = "@EndDate", Value = EndDate });
            return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());
        }
        public DataSet GetLeaveByUserIdAndDateRangeSP(int? UserID, DateTime StartDate, DateTime EndDate)
        {
            string sql = "SELECT_LeaveByUserIdAndDateRangeSP";
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            if (UserID.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@UserID", Value = UserID });
            sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = StartDate });
            sqlparams.Add(new SqlParameter() { ParameterName = "@EndDate", Value = EndDate });
            return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());
        }

    }
	
	
}
