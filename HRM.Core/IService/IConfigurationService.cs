using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Configuration;

namespace HRM.Core.IService
{
		
	public interface IConfigurationService
	{
        Dictionary<string, string> GetConfigurationBasicSearchColumns();
        
        List<SearchColumn> GetConfigurationAdvanceSearchColumns();

		Configuration GetConfiguration(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		Configuration UpdateConfiguration(Configuration entity);
		Configuration UpdateConfigurationByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteConfiguration(System.Int32 Id);
		List<Configuration> GetAllConfiguration();
		Configuration InsertConfiguration(Configuration entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
