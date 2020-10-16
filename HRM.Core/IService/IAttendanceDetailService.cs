using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.AttendanceDetail;

namespace HRM.Core.IService
{
		
	public interface IAttendanceDetailService
	{
        Dictionary<string, string> GetAttendanceDetailBasicSearchColumns();
        
        List<SearchColumn> GetAttendanceDetailAdvanceSearchColumns();

		List<AttendanceDetail> GetAttendanceDetailByAttendanceId(System.Int32? AttendanceId);
		List<AttendanceDetail> GetAttendanceDetailByAttendanceTypeId(System.Int32? AttendanceTypeId);
		AttendanceDetail GetAttendanceDetail(System.Int32 Id);

 

        DataTransfer<List<GetOutput>> GetAll();
		AttendanceDetail UpdateAttendanceDetail(AttendanceDetail entity);
		AttendanceDetail UpdateAttendanceDetailByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteAttendanceDetail(System.Int32 Id);
		List<AttendanceDetail> GetAllAttendanceDetail();
		AttendanceDetail InsertAttendanceDetail(AttendanceDetail entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
