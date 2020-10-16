using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.Model;
using System.Data.SqlClient;
using System.Data;
namespace HRM.Repository
{

    public partial class AttendanceRepository : AttendanceRepositoryBase, IAttendanceRepository
    {
        public List<Attendance> GetAttendanceByDate(DateTime dtAttendance, string SelectClause = null)
        {
            string sql = string.IsNullOrEmpty(SelectClause) ? GetAttendanceSelectClause() : (string.Format("Select [{0}] ", SelectClause));
            sql += "from [Attendance] with (nolock)  where [Date]=@Date and IsActive = 1 ";
            SqlParameter parameter = new SqlParameter("@Date", dtAttendance.Date);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<Attendance>(ds, AttendanceFromDataRow);
        }
        public List<Attendance> GetAttendanceByDateRange(DateTime dtStart, DateTime dtEnd, string SelectClause = null)
        {
            string sql = string.IsNullOrEmpty(SelectClause) ? GetAttendanceSelectClause() : (string.Format("Select [{0}] ", SelectClause));
            sql += "from [Attendance] with (nolock)  where [Date] BETWEEN @StartDate AND @EndDate and IsActive = 1  ";
            SqlParameter parameter1 = new SqlParameter("@StartDate", dtStart);
            SqlParameter parameter2 = new SqlParameter("@EndDate", dtEnd);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter1, parameter2 });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<Attendance>(ds, AttendanceFromDataRow);
        }
        public List<Attendance> GetAttendanceByUserIdAndDateRange(DateTime dtStart, DateTime dtEnd, Int32? UserID, string SelectClause = null)
        {
            string sql = string.IsNullOrEmpty(SelectClause) ? GetAttendanceSelectClause() : (string.Format("Select [{0}] ", SelectClause));
            sql += "from Attendance with (nolock) where [Date] between @StartDate and @EndDate AND UserID = @UserID  AND IsActive=1 ";
            SqlParameter[] parameterArray = new SqlParameter[]
            {
                new SqlParameter("@StartDate", dtStart)
                , new SqlParameter("@EndDate", dtEnd)
                , new SqlParameter("@UserID", UserID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, parameterArray);
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<Attendance>(ds, AttendanceFromDataRow);
        }
        public List<Attendance> GetAttendanceByUserIDAndDate(int userId, DateTime dtAttendance, string SelectClause = null)
        {
            string sql = string.IsNullOrEmpty(SelectClause) ? GetAttendanceSelectClause() : (string.Format("Select [{0}] ", SelectClause));
            sql += "from [Attendance] with (nolock)  where UserId=@UserId AND [Date]=@Date and IsActive = 1 ";
            SqlParameter parameter1 = new SqlParameter("@UserId", userId);
            SqlParameter parameter2 = new SqlParameter("@Date", dtAttendance.Date);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter1, parameter2 });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<Attendance>(ds, AttendanceFromDataRow);
        }

        public List<Attendance> GetAttendanceByDate(DateTime dtAttendance, int BranchID, string SelectClause = null)
        {
            string sql = ""; //string.IsNullOrEmpty(SelectClause) ? GetAttendanceSelectClause() : (string.Format("Select [{0}] ", SelectClause));
            sql += "select a.* from [dbo].[Attendance] a (nolock) join [dbo].[UserDepartment] ud (nolock) on a.UserID = ud.UserID join [dbo].[BranchDepartment] bd (nolock) on ud.DepartmentID = bd.DepartmentID  ";
            sql += "  where a.[Date] = @Date and a.IsActive = 1 and a.[Date] between ud.EffectiveDate and isnull(ud.RetiredDate,a.[Date]) and bd.BranchID = @BranchID and bd.IsActive = 1  ";

            SqlParameter[] parameterArray = new SqlParameter[] 
            { 
                new SqlParameter("@Date", dtAttendance.Date) 
                , new SqlParameter("@BranchID", BranchID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, parameterArray);
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<Attendance>(ds, AttendanceFromDataRow);
        }

        public List<Attendance> GetAttendanceByDateRange(DateTime dtStart, DateTime dtEnd, int BranchID, string SelectClause = null)
        {
            string sql = ""; // string.IsNullOrEmpty(SelectClause) ? GetAttendanceSelectClause() : (string.Format("Select [{0}] ", SelectClause));
            sql += "select a.* from [dbo].[Attendance] a (nolock) join [dbo].[UserDepartment] ud (nolock) on a.UserID = ud.UserID join [dbo].[BranchDepartment] bd (nolock) on ud.DepartmentID = bd.DepartmentID  ";
            sql += "  where a.[Date] BETWEEN @StartDate AND @EndDate  and a.IsActive = 1 and a.[Date] between ud.EffectiveDate and isnull(ud.RetiredDate,a.[Date]) and bd.BranchID = @BranchID and bd.IsActive = 1  ";

            SqlParameter[] parameterArray = new SqlParameter[] 
            { 
                new SqlParameter("@StartDate", dtStart) 
                , new SqlParameter("@EndDate", dtEnd) 
                , new SqlParameter("@BranchID", BranchID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, parameterArray);
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<Attendance>(ds, AttendanceFromDataRow);
        }

        public void InsertPreAttendance()
        {
            string sql = "InsertPreAttendance"; // string.IsNullOrEmpty(SelectClause) ? GetAttendanceSelectClause() : (string.Format("Select [{0}] ", SelectClause));
            SqlHelper.ExecuteNonQuery(this.ConnectionString, CommandType.StoredProcedure, sql, null);
        }
        public DataSet GetAttendanceByDateAndUser(DateTime dtStart, DateTime dtEnd, Int32? UserID)
        {
            string sql = "SELECT_AttendanceDetailForUpdate";
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = dtStart });
            sqlparams.Add(new SqlParameter() { ParameterName = "@EndDate", Value = dtEnd });
            if (UserID.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@UserId", Value = UserID });
            return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());

        }
        public DataSet GetAttendanceAndAttendanceStatusByUserIdAndDateRange(DateTime StartDate, DateTime EndDate, Int32? UserID)
        {
            string sql = "SELECT_AttendanceAndAttendanceStatusByUserIdAndDateRange";
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = StartDate });
            sqlparams.Add(new SqlParameter() { ParameterName = "@EndDate", Value = EndDate });
            if (UserID.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@UserId", Value = UserID });
            return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());

        }

        public DataSet GetAttendanceByDateAndUser(DateTime dtStart, DateTime dtEnd, Int32? UserID, Int32? BranchID)
        {
            string sql = "SELECT_Attendance";
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = dtStart });
            sqlparams.Add(new SqlParameter() { ParameterName = "@EndDate", Value = dtEnd });
            if (UserID.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@UserId", Value = UserID });
            if (BranchID.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@BranchID", Value = BranchID });
            return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());
        }
        //NewFor DailyAttendanceUpdate
        public DataSet GetAttendanceByDateAndUserUpdate(DateTime StartDate, DateTime EndDate, Int32? UserId)
        {
            string sql = "SELECT_AttendanceDetailForUpdate";
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = StartDate });
            sqlparams.Add(new SqlParameter() { ParameterName = "@EndDate", Value = EndDate });
            if (UserId.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@UserId", Value = UserId });
            return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());
        }

        //New DailyAttendanceReport
        public DataSet GetDailyAttendanceByDateUser(DateTime dtStart, Int32? UserId,Int32? DepartmentId)
        {
            string sql = "SELECT_AttendanceSummary";
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = dtStart });

            if (UserId.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@UserId", Value = UserId });
            if (DepartmentId.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@DepartmentId", Value = DepartmentId });
           
            return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());
        }

        //New For Absent
        public DataSet GetAbsentReportByDateUserAndDepartment(DateTime? dtStart, DateTime? dtEnd, Int32? UserId,Int32?DepartmentId)
        {
            string sql = "SELECT_AbsentReport";
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = dtStart });
            sqlparams.Add(new SqlParameter() { ParameterName = "@EndDate", Value = dtEnd });

            if (UserId.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@UserId", Value = UserId });
            if (DepartmentId.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@DepartmentId", Value = DepartmentId });

            return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());
        }
        public DataSet GetDailyAttendanceByDateTypeShiftBranch(DateTime dtStart, DateTime dtEnd, Int32? BranchId, string DepartmentName, Int32? ShiftID, Int32? SalaryTypeId)
        {
            string sql = "SELECT_AttendanceSummary";
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = dtStart });
            sqlparams.Add(new SqlParameter() { ParameterName = "@EndDate", Value = dtEnd });
            if (DepartmentName != null)
                sqlparams.Add(new SqlParameter() { ParameterName = "@DepartmentName", Value = DepartmentName });
            if (BranchId.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@BranchID", Value = BranchId });
            if (ShiftID.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@ShiftID", Value = ShiftID });
            if (SalaryTypeId.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@SalaryTypeId", Value = SalaryTypeId });
            return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());
        }

        //New for DailyAttendanceReport
        public DataSet GetAttendanceByDateUserAndDepartment(DateTime dtStart, Int32? UserId, Int32? DepartmentId)
        {
            string sql = "SELECT_DailyAttendanceReport";
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = dtStart });
        
            if (UserId.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@UserId", Value = UserId });
            if (DepartmentId.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@DepartmentId", Value = DepartmentId });
          
            return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());
        }

        //New for DailyDetailAttendanceReport
        public DataSet GetAttendanceDetailByDateUserAndDepartment(DateTime dtStart, Int32? UserId, Int32? DepartmentId)
        {
            string sql = "SELECT_DailyAttendanceReport2";
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = dtStart });

            if (UserId.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@UserId", Value = UserId });
            if (DepartmentId.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@DepartmentId", Value = DepartmentId });

            return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());
        }


        //NewForMonthly
        //New for MonthlyAttendanceReport
        public DataSet GetMonthlyAttendanceByDateUserAndDepartment(DateTime dtStart, DateTime dtEnd, Int32? UserId, Int32? DepartmentId)
        {
            string sql = "SELECT_MonthlyAttendanceReport2";
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = dtStart });
            sqlparams.Add(new SqlParameter() { ParameterName = "@EndDate", Value = dtEnd });

            if (UserId.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@UserId", Value = UserId });
            if (DepartmentId.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@DepartmentId", Value = DepartmentId });

            return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());
        }

        //New for MonthlyDetailAttendanceReport
        public DataSet GetMonthlyAttendanceDetailByDateUserAndDepartment(DateTime dtStart, DateTime dtEnd, Int32? UserId, Int32? DepartmentId)
        {
            string sql = "SELECT_MonthlyAttendanceReport2";
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = dtStart });
            sqlparams.Add(new SqlParameter() { ParameterName = "@EndDate", Value = dtEnd });

            if (UserId.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@UserId", Value = UserId });
            if (DepartmentId.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@DepartmentId", Value = DepartmentId });

            return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());
        }



        public DataSet GetMonthlyAttendanceSummary(DateTime dtStart, DateTime dtEnd, Int32? UserID, string UserName, Int32? BranchID, string DepartmentName, Int32? ShiftID, Int32? SalaryTypeId)
        {
            string sql = "SELECT_MonthlyAttendanceSummary";
            List<SqlParameter> sqlparams = new List<SqlParameter>();
            sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = dtStart });
            sqlparams.Add(new SqlParameter() { ParameterName = "@EndDate", Value = dtEnd });
            if (UserID.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@UserId", Value = UserID .Value});
            if (UserName != null)
                sqlparams.Add(new SqlParameter() { ParameterName = "@UserName", Value = UserName });
            if (DepartmentName != null)
                sqlparams.Add(new SqlParameter() { ParameterName = "@DepartmentName", Value = DepartmentName });
            if (BranchID.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@BranchID", Value = BranchID });
            if (ShiftID.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@ShiftID", Value = ShiftID });
            if (SalaryTypeId.HasValue)
                sqlparams.Add(new SqlParameter() { ParameterName = "@SalaryTypeId", Value = SalaryTypeId });
            return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());
        }
    }
}
