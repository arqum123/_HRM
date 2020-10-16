using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Country;

namespace HRM.Core.IService
{
		
	public interface ICountryService
	{
        Dictionary<string, string> GetCountryBasicSearchColumns();
        
        List<SearchColumn> GetCountryAdvanceSearchColumns();

		Country GetCountry(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		Country UpdateCountry(Country entity);
		Country UpdateCountryByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteCountry(System.Int32 Id);
		List<Country> GetAllCountry();
		Country InsertCountry(Country entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
