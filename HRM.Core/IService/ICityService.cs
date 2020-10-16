using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.City;

namespace HRM.Core.IService
{
		
	public interface ICityService
	{
        Dictionary<string, string> GetCityBasicSearchColumns();
        
        List<SearchColumn> GetCityAdvanceSearchColumns();

		List<City> GetCityByStateId(System.Int32? StateId);
		City GetCity(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		City UpdateCity(City entity);
		City UpdateCityByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteCity(System.Int32 Id);
		List<City> GetAllCity();
		City InsertCity(City entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
