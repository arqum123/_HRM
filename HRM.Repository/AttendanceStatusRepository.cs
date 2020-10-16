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
	public partial class AttendanceStatusRepository: AttendanceStatusRepositoryBase, IAttendanceStatusRepository
	{
        //New
        //public DataSet GetAttendanceStatusByDateRange(DateTime dtStart, DateTime dtEnd)
        //{
        //    string sql = "SELECT_AttendanceStatusByDateRange";
        //    List<SqlParameter> sqlparams = new List<SqlParameter>();
        //    sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = dtStart });
        //    sqlparams.Add(new SqlParameter() { ParameterName = "@EndDate", Value = dtEnd });

        //    return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());
        //}

        //New
        public virtual List<AttendanceStatus> GetAttendanceStatusByDateRangeSharp(DateTime dtStart, DateTime dtEnd)
        {
            string sql = ""; // string.IsNullOrEmpty(SelectClause) ? GetAttendanceSelectClause() : (string.Format("Select [{0}] ", SelectClause));
            sql += "SELECT AtS.ID ,AtS.AttendanceId,AtS.IsShiftOffDay,AtS.IsHoliday,AtS.IsLeaveDay,AtS.IsQuarterDay,AtS.IsHalfDay,AtS.IsFullDay,AtS.IsApproved,AtS.IsLate,AtS.IsEarly,Ats.IsActive ";
            sql += "from [Attendance] A Inner JOIN AttendanceStatus AtS ON A.ID = AtS.AttendanceID Where A.[Date] BETWEEN @StartDate AND @EndDate ORDER BY A.ID  ";

            SqlParameter[] parameterArray = new SqlParameter[]
            {
                new SqlParameter("@StartDate", dtStart)
                , new SqlParameter("@EndDate", dtEnd)
            };
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, parameterArray);
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<AttendanceStatus>(ds, AttendanceStatusFromDataRow);
        }
    }
	
	
}
