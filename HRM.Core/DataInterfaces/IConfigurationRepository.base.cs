using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IConfigurationRepositoryBase
	{
        
        Dictionary<string, string> GetConfigurationBasicSearchColumns();
        List<SearchColumn> GetConfigurationSearchColumns();
        List<SearchColumn> GetConfigurationAdvanceSearchColumns();
        

		Configuration GetConfiguration(System.Int32 Id,string SelectClause=null);
		Configuration UpdateConfiguration(Configuration entity);
		Configuration UpdateConfigurationByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteConfiguration(System.Int32 Id);
		Configuration DeleteConfiguration(Configuration entity);
		List<Configuration> GetPagedConfiguration(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<Configuration> GetAllConfiguration(string SelectClause=null);
		Configuration InsertConfiguration(Configuration entity);
		List<Configuration> GetConfigurationByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
