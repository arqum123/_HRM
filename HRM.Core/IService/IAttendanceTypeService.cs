using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.AttendanceType;

namespace HRM.Core.IService
{
		
	public interface IAttendanceTypeService
	{
        Dictionary<string, string> GetAttendanceTypeBasicSearchColumns();
        
        List<SearchColumn> GetAttendanceTypeAdvanceSearchColumns();

		AttendanceType GetAttendanceType(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		AttendanceType UpdateAttendanceType(AttendanceType entity);
		AttendanceType UpdateAttendanceTypeByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteAttendanceType(System.Int32 Id);
		List<AttendanceType> GetAllAttendanceType();
		AttendanceType InsertAttendanceType(AttendanceType entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
