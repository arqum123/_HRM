using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IDeviceRepositoryBase
	{
        
        Dictionary<string, string> GetDeviceBasicSearchColumns();
        List<SearchColumn> GetDeviceSearchColumns();
        List<SearchColumn> GetDeviceAdvanceSearchColumns();
        

		List<Device> GetDeviceByDeviceModalId(System.Int32? DeviceModalId,string SelectClause=null);
		List<Device> GetDeviceByBranchId(System.Int32? BranchId,string SelectClause=null);
		Device GetDevice(System.Int32 Id,string SelectClause=null);
		Device UpdateDevice(Device entity);
		Device UpdateDeviceByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteDevice(System.Int32 Id);
		Device DeleteDevice(Device entity);
		List<Device> GetPagedDevice(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<Device> GetAllDevice(string SelectClause=null);
		Device InsertDevice(Device entity);
		List<Device> GetDeviceByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
