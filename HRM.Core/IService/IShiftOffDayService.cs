using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.ShiftOffDay;

namespace HRM.Core.IService
{
		
	public interface IShiftOffDayService
	{
        Dictionary<string, string> GetShiftOffDayBasicSearchColumns();
        
        List<SearchColumn> GetShiftOffDayAdvanceSearchColumns();

		List<ShiftOffDay> GetShiftOffDayByShiftId(System.Int32? ShiftId);
		ShiftOffDay GetShiftOffDay(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		ShiftOffDay UpdateShiftOffDay(ShiftOffDay entity);
		ShiftOffDay UpdateShiftOffDayByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteShiftOffDay(System.Int32 Id);
		List<ShiftOffDay> GetAllShiftOffDay();
		ShiftOffDay InsertShiftOffDay(ShiftOffDay entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
