using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IAttendanceRepository: IAttendanceRepositoryBase
	{
        List<Attendance> GetAttendanceByDate(DateTime dtAttendance, string SelectClause = null);
        List<Attendance> GetAttendanceByDateRange(DateTime dtStart, DateTime dtEnd, string SelectClause = null);
        List<Attendance> GetAttendanceByUserIDAndDate(int userId, DateTime dtAttendance, string SelectClause = null);

        List<Attendance> GetAttendanceByDate(DateTime dtAttendance, int BranchID, string SelectClause = null);
        List<Attendance> GetAttendanceByDateRange(DateTime dtStart, DateTime dtEnd, int BranchID, string SelectClause = null);

        void InsertPreAttendance();
        System.Data.DataSet GetAttendanceByDateAndUser(DateTime dtStart, DateTime dtEnd, Int32? UserID);
        System.Data.DataSet GetAttendanceByDateAndUser(DateTime dtStart, DateTime dtEnd, Int32? UserID, Int32? BranchID);
        System.Data.DataSet GetAttendanceByDateAndUserUpdate(DateTime StartDate, DateTime EndDate, Int32? UserID);
        System.Data.DataSet GetDailyAttendanceByDateTypeShiftBranch(DateTime dtStart, DateTime dtEnd, Int32? BranchId, string DepartmentName, Int32? ShiftID, Int32? SalaryTypeId);

        //New For GetAbsentReportByDateUserAndDepartment
        System.Data.DataSet GetAbsentReportByDateUserAndDepartment(DateTime? dtStart, DateTime? dtEnd, Int32? UserID,Int32?DepartmentId);
        //New For DailyAttendanceReport
        System.Data.DataSet GetAttendanceByDateUserAndDepartment(DateTime dtStart,Int32? UserId, Int32? DepartmentId);

        //New For DailyDetailAttendanceReport
        System.Data.DataSet GetAttendanceDetailByDateUserAndDepartment(DateTime dtStart, Int32? UserId, Int32? DepartmentId);


        //New For MonthlyAttendanceReport
        System.Data.DataSet GetMonthlyAttendanceByDateUserAndDepartment(DateTime dtStart, DateTime dtEnd, Int32? UserId, Int32? DepartmentId);

        //New For MonthlyDetailAttendanceReport
        System.Data.DataSet GetMonthlyAttendanceDetailByDateUserAndDepartment(DateTime dtStart, DateTime dtEnd, Int32? UserId, Int32? DepartmentId);
        System.Data.DataSet GetMonthlyAttendanceSummary(DateTime dtStart, DateTime dtEnd, Int32? UserID, string UserName, Int32? BranchID, string DepartmentName, Int32? ShiftID, Int32? SalaryTypeId);

      
    }
	
	
}
