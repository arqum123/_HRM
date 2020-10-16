using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Shift;

namespace HRM.Core.IService
{
		
	public interface IShiftService
	{
        Dictionary<string, string> GetShiftBasicSearchColumns();
        
        List<SearchColumn> GetShiftAdvanceSearchColumns();

		Shift GetShift(System.Int32 Id);
        Shift GetShiftWithOffDays(System.Int32 Id);
        Shift GetShiftWithOffDaysHistory(System.Int32 Id);

		DataTransfer<List<GetOutput>> GetAll();
		Shift UpdateShift(Shift entity);
		Shift UpdateShiftByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteShift(System.Int32 Id);
		List<Shift> GetAllShift();
        List<Shift> GetAllShiftWithOffDays();
        List<Shift> GetAllShiftWithAttendancePolicy();
		Shift InsertShift(Shift entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);

        List<Shift> GetAllShiftWithOffDays(int BranchID);
        List<Shift> GetAllShiftWithAttendancePolicy(int BranchID);
        List<Shift> GetAllShift(int BranchID);
	}
	
	
}
