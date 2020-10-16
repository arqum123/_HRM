using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.AttendancePolicy;

namespace HRM.Core.IService
{
		
	public interface IAttendancePolicyService
	{
        Dictionary<string, string> GetAttendancePolicyBasicSearchColumns();
        
        List<SearchColumn> GetAttendancePolicyAdvanceSearchColumns();

		List<AttendancePolicy> GetAttendancePolicyByShiftId(System.Int32? ShiftId);
		List<AttendancePolicy> GetAttendancePolicyByAttendanceVariableId(System.Int32? AttendanceVariableId);
		AttendancePolicy GetAttendancePolicy(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		AttendancePolicy UpdateAttendancePolicy(AttendancePolicy entity);
		AttendancePolicy UpdateAttendancePolicyByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteAttendancePolicy(System.Int32 Id);
		List<AttendancePolicy> GetAllAttendancePolicy();
		AttendancePolicy InsertAttendancePolicy(AttendancePolicy entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
