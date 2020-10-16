using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.AttendanceVariable;

namespace HRM.Core.IService
{
		
	public interface IAttendanceVariableService
	{
        Dictionary<string, string> GetAttendanceVariableBasicSearchColumns();
        
        List<SearchColumn> GetAttendanceVariableAdvanceSearchColumns();

		AttendanceVariable GetAttendanceVariable(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		AttendanceVariable UpdateAttendanceVariable(AttendanceVariable entity);
		AttendanceVariable UpdateAttendanceVariableByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteAttendanceVariable(System.Int32 Id);
		List<AttendanceVariable> GetAllAttendanceVariable();
		AttendanceVariable InsertAttendanceVariable(AttendanceVariable entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
