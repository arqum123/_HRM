using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Holiday;

namespace HRM.Core.IService
{
		
	public interface IHolidayService
	{
        Dictionary<string, string> GetHolidayBasicSearchColumns();
        
        List<SearchColumn> GetHolidayAdvanceSearchColumns();

		Holiday GetHoliday(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		Holiday UpdateHoliday(Holiday entity);
		Holiday UpdateHolidayByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteHoliday(System.Int32 Id);
		List<Holiday> GetAllHoliday();
		Holiday InsertHoliday(Holiday entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
