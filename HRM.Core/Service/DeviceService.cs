using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Device;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class DeviceService : IDeviceService 
	{
		private IDeviceRepository _iDeviceRepository;
        
		public DeviceService(IDeviceRepository iDeviceRepository)
		{
			this._iDeviceRepository = iDeviceRepository;
		}
        
        public Dictionary<string, string> GetDeviceBasicSearchColumns()
        {
            
            return this._iDeviceRepository.GetDeviceBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetDeviceAdvanceSearchColumns()
        {
            
            return this._iDeviceRepository.GetDeviceAdvanceSearchColumns();
           
        }
        

		public virtual List<Device> GetDeviceByDeviceModalId(System.Int32? DeviceModalId)
		{
			return _iDeviceRepository.GetDeviceByDeviceModalId(DeviceModalId);
		}

		public virtual List<Device> GetDeviceByBranchId(System.Int32? BranchId)
		{
			return _iDeviceRepository.GetDeviceByBranchId(BranchId);
		}

		public Device GetDevice(System.Int32 Id)
		{
			return _iDeviceRepository.GetDevice(Id);
		}

		public Device UpdateDevice(Device entity)
		{
			return _iDeviceRepository.UpdateDevice(entity);
		}

		public Device UpdateDeviceByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iDeviceRepository.UpdateDeviceByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteDevice(System.Int32 Id)
		{
			return _iDeviceRepository.DeleteDevice(Id);
		}

		public List<Device> GetAllDevice()
		{
			return _iDeviceRepository.GetAllDevice();
		}

		public Device InsertDevice(Device entity)
		{
			 return _iDeviceRepository.InsertDevice(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				Device device = _iDeviceRepository.GetDevice(id);
                if(device!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(device);
                    tranfer.Data=output ;

                }
                else
                {
                    tranfer.IsSuccess = false;
                    tranfer.Errors = new string[1];
                    tranfer.Errors[0] = "Error: No record found.";
                }             
                
            }
            else
            {
                tranfer.IsSuccess = false;
                tranfer.Errors = new string[1];
                tranfer.Errors[0] = "Error: Invalid request.";
            }
            return tranfer;
        }   
        public DataTransfer<List<GetOutput>> GetAll()
        {
            DataTransfer<List<GetOutput>> tranfer = new DataTransfer<List<GetOutput>>();
            List<Device> devicelist = _iDeviceRepository.GetAllDevice();
            if (devicelist != null && devicelist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(devicelist);
                tranfer.Data = outputlist;

            }
            else
            {
                tranfer.IsSuccess = false;
                tranfer.Errors = new string[1];
                tranfer.Errors[0] = "Error: No record found.";
            }
            return tranfer;
        }
        public DataTransfer<PostOutput> Insert(PostInput Input)
        {
           DataTransfer<PostOutput> transer = new DataTransfer<PostOutput>();
            IList<string> errors = Validator.Validate(Input);
            if(errors.Count==0)
            {
                Device device = new Device();
                PostOutput output = new PostOutput();
                device.CopyFrom(Input);
                device = _iDeviceRepository.InsertDevice(device);
                output.CopyFrom(device);
                transer.IsSuccess = true;
                transer.Data = output;
            }
            else
            {
                transer.IsSuccess = false;
                transer.Errors = errors.ToArray<string>();
            }
            return transer;
        }

        public DataTransfer<PutOutput> Update(PutInput Input)
        {
            DataTransfer<PutOutput> transer = new DataTransfer<PutOutput>();
            IList<string> errors = Validator.Validate(Input);
            if (errors.Count == 0)
            {
                Device deviceinput = new Device();
                Device deviceoutput = new Device();
                PutOutput output = new PutOutput();
                deviceinput.CopyFrom(Input);
                Device device = _iDeviceRepository.GetDevice(deviceinput.Id);
                if (device!=null)
                {
                    deviceoutput = _iDeviceRepository.UpdateDevice(deviceinput);
                    if(deviceoutput!=null)
                    {
                        output.CopyFrom(deviceoutput);
                        transer.IsSuccess = true;
                        transer.Data = output;
                    }
                    else
                    {
                        transer.IsSuccess = false;
                        transer.Errors = new string[1];
                        transer.Errors[0] = "Error: Could not update.";
                    } 
                }
                else                
                {
                    transer.IsSuccess = false;
                    transer.Errors = new string[1];
                    transer.Errors[0] = "Error: Record not found.";
                }
            }
            else
            {
                transer.IsSuccess = false;
                transer.Errors = errors.ToArray<string>();
            }
            return transer;
        }

         public DataTransfer<string> Delete(string _id)
         {
            DataTransfer<string> tranfer = new DataTransfer<string>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				 bool IsDeleted = _iDeviceRepository.DeleteDevice(id);
                if(IsDeleted)
                {
                    tranfer.IsSuccess = true;
                    tranfer.Data=IsDeleted.ToString().ToLower() ;

                }
                else
                {
                    tranfer.IsSuccess = false;
                    tranfer.Errors = new string[1];
                    tranfer.Errors[0] = "Error: No record found.";
                }             
                
            }
            else
            {
                tranfer.IsSuccess = false;
                tranfer.Errors = new string[1];
                tranfer.Errors[0] = "Error: Invalid request.";
            }
            return tranfer;
        }
	}
	
	
}
