using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IDeviceModalRepositoryBase
	{
        
        Dictionary<string, string> GetDeviceModalBasicSearchColumns();
        List<SearchColumn> GetDeviceModalSearchColumns();
        List<SearchColumn> GetDeviceModalAdvanceSearchColumns();
        

		DeviceModal GetDeviceModal(System.Int32 Id,string SelectClause=null);
		DeviceModal UpdateDeviceModal(DeviceModal entity);
		DeviceModal UpdateDeviceModalByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteDeviceModal(System.Int32 Id);
		DeviceModal DeleteDeviceModal(DeviceModal entity);
		List<DeviceModal> GetPagedDeviceModal(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<DeviceModal> GetAllDeviceModal(string SelectClause=null);
		DeviceModal InsertDeviceModal(DeviceModal entity);
		List<DeviceModal> GetDeviceModalByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
