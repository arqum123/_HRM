using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Attendance;

namespace HRM.Core.IService
{
		
	public interface IAttendanceService
	{
        Dictionary<string, string> GetAttendanceBasicSearchColumns();
        
        List<SearchColumn> GetAttendanceAdvanceSearchColumns();

		List<Attendance> GetAttendanceByUserId(System.Int32? UserId);
		Attendance GetAttendance(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		Attendance UpdateAttendance(Attendance entity);
		Attendance UpdateAttendanceByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteAttendance(System.Int32 Id);
        List<Attendance> GetAllAttendance();
        List<Attendance> GetAllAttendanceWithAttendanceDetail(string StartDate, string EndDate);
        List<Attendance> GetAllAttendanceByDateUserWithDetail(string StartDate, string EndDate, Int32? UID);
        List<Attendance> GetAllAttendanceByDateUserWithDetail(string StartDate, string EndDate, Int32? UID,Int32? BranchId);
        List<Attendance> GetDailyAttendanceUpdateByDateUserWithDetail(string StartDate, string EndDate, Int32? UID, Int32? BranchId);
        List<HRM.Core.Model.VMAttendanceSummary> GetDailyAttendanceSummary(string StartDate, string EndDate, Int32? UserId, Int32? BranchId, string DepartmentName, Int32? ShiftID, Int32? SalaryTypeId);

        //New For DailyAttendanceReport
        List<HRM.Core.Model.VMAttendanceSummary> GetDailyAttendanceReport(string StartDate, Int32? UserId, Int32? DepartmentId);
        //New For DailyDetailAttendanceReportz
        List<HRM.Core.Model.VMAttendanceSummary> GetDailyDetailAttendanceReport(string StartDate, Int32? UserId, Int32? DepartmentId);

  
        //New For DailyDetailAttendanceReport
        List<HRM.Core.Model.VMAttendanceSummary> GetMonthlyDetailAttendanceReport(string StartDate, string EndDate, Int32? UserId, Int32? DepartmentId);


        //New PracticeMonthlyReport
        List<HRM.Core.Model.PracticeVMReport> GetMonthlyAttendanceReportPractice(string StartDate, string EndDate, Int32? UserId, Int32? DepartmentId);


        //New PracticeMonthlyReportDetailPractice
        List<HRM.Core.Model.PracticeVMReport> GetMonthlyDetailAttendanceReportPractice(string StartDate, string EndDate, Int32? UserId, Int32? DepartmentId);
        List<HRM.Core.Model.VMDailyAttendanceUpdateStatus> GetDailyAttendanceUpdateSummary(DateTime StartDate, DateTime EndDate, Int32? UserId);

        //New For AbsentReport
        List<HRM.Core.Model.VMAbsentReport> GetAbsentReport(DateTime? StartDate, DateTime? EndDate, Int32? UserId, Int32? DepartmentId);
        Attendance InsertAttendance(Attendance entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);

        
        List<Attendance> GetAttendanceByDate(DateTime dtAttendance, string SelectClause = null);
        List<Attendance> GetAttendanceByDateRange(DateTime dtStart, DateTime dtEnd, string SelectClause = null);
        List<Attendance> GetAttendanceByUserIDAndDate(int userId, DateTime dtAttendance, string SelectClause = null);

        List<Attendance> GetAttendanceByDate(DateTime dtAttendance, int BranchID, string SelectClause = null);
        List<Attendance> GetAttendanceByDateRange(DateTime dtStart, DateTime dtEnd, int BranchID, string SelectClause = null);

        void InsertPreAttendance();
        System.Data.DataSet GetAttendanceByDateAndUser(DateTime dtStart, DateTime dtEnd, Int32? UserID);
        System.Data.DataSet GetAttendanceByDateAndUser(DateTime dtStart, DateTime dtEnd, Int32? UserID, Int32? BranchId);
        System.Data.DataSet GetAttendanceByDateAndUserUpdate(DateTime dtStart, DateTime dtEnd, Int32? UserID);

        //New For GetAbsentReportByDateUserAndDepartment
        System.Data.DataSet GetAbsentReportByDateUserAndDepartment(DateTime? dtStart, DateTime? dtEnd, Int32? UserId,Int32?DepartmentId);
        List<User> GetMonthlyAttendanceSummary(DateTime dtStart, DateTime dtEnd, Int32? UserID, string UserName, Int32? BranchID, string DepartmentName, Int32? ShiftID, Int32? SalaryTypeId);
        
    }
	
	
}
