using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.DeviceModal;

namespace HRM.Core.IService
{
		
	public interface IDeviceModalService
	{
        Dictionary<string, string> GetDeviceModalBasicSearchColumns();
        
        List<SearchColumn> GetDeviceModalAdvanceSearchColumns();

		DeviceModal GetDeviceModal(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		DeviceModal UpdateDeviceModal(DeviceModal entity);
		DeviceModal UpdateDeviceModalByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteDeviceModal(System.Int32 Id);
		List<DeviceModal> GetAllDeviceModal();
		DeviceModal InsertDeviceModal(DeviceModal entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
