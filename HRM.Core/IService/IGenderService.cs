using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Gender;

namespace HRM.Core.IService
{
		
	public interface IGenderService
	{
        Dictionary<string, string> GetGenderBasicSearchColumns();
        
        List<SearchColumn> GetGenderAdvanceSearchColumns();

		Gender GetGender(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		Gender UpdateGender(Gender entity);
		Gender UpdateGenderByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteGender(System.Int32 Id);
		List<Gender> GetAllGender();
		Gender InsertGender(Gender entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
