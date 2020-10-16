using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.State;

namespace HRM.Core.IService
{
		
	public interface IStateService
	{
        Dictionary<string, string> GetStateBasicSearchColumns();
        
        List<SearchColumn> GetStateAdvanceSearchColumns();

		List<State> GetStateByCountryId(System.Int32? CountryId);
		State GetState(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		State UpdateState(State entity);
		State UpdateStateByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteState(System.Int32 Id);
		List<State> GetAllState();
		State InsertState(State entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
