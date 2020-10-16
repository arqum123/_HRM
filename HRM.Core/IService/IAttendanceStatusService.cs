using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.AttendanceStatus;

namespace HRM.Core.IService
{
		
	public interface IAttendanceStatusService
	{
        Dictionary<string, string> GetAttendanceStatusBasicSearchColumns();
        
        List<SearchColumn> GetAttendanceStatusAdvanceSearchColumns();

		List<AttendanceStatus> GetAttendanceStatusByAttendanceId(System.Int32? AttendanceId);

        List<AttendanceStatus> GetAttendanceStatusByDateRangeSharp(DateTime dtStart, DateTime dtEnd); //New

        AttendanceStatus GetAttendanceStatus(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		AttendanceStatus UpdateAttendanceStatus(AttendanceStatus entity);
		AttendanceStatus UpdateAttendanceStatusByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteAttendanceStatus(System.Int32 Id);
		List<AttendanceStatus> GetAllAttendanceStatus();

        List<AttendanceStatus> GetAllAttendanceStatusStartAndEndDate(System.String StartDate, System.String EndDate);

        
        AttendanceStatus InsertAttendanceStatus(AttendanceStatus entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
