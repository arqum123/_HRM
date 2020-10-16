using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Religion;

namespace HRM.Core.IService
{
		
	public interface IReligionService
	{
        Dictionary<string, string> GetReligionBasicSearchColumns();
        
        List<SearchColumn> GetReligionAdvanceSearchColumns();

		Religion GetReligion(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		Religion UpdateReligion(Religion entity);
		Religion UpdateReligionByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteReligion(System.Int32 Id);
		List<Religion> GetAllReligion();
		Religion InsertReligion(Religion entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
