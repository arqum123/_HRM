using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Device;

namespace HRM.Core.IService
{
		
	public interface IDeviceService
	{
        Dictionary<string, string> GetDeviceBasicSearchColumns();
        
        List<SearchColumn> GetDeviceAdvanceSearchColumns();

		List<Device> GetDeviceByDeviceModalId(System.Int32? DeviceModalId);
		List<Device> GetDeviceByBranchId(System.Int32? BranchId);
		Device GetDevice(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		Device UpdateDevice(Device entity);
		Device UpdateDeviceByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteDevice(System.Int32 Id);
		List<Device> GetAllDevice();
		Device InsertDevice(Device entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
