using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.DeviceModal;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class DeviceModalService : IDeviceModalService 
	{
		private IDeviceModalRepository _iDeviceModalRepository;
        
		public DeviceModalService(IDeviceModalRepository iDeviceModalRepository)
		{
			this._iDeviceModalRepository = iDeviceModalRepository;
		}
        
        public Dictionary<string, string> GetDeviceModalBasicSearchColumns()
        {
            
            return this._iDeviceModalRepository.GetDeviceModalBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetDeviceModalAdvanceSearchColumns()
        {
            
            return this._iDeviceModalRepository.GetDeviceModalAdvanceSearchColumns();
           
        }
        

		public DeviceModal GetDeviceModal(System.Int32 Id)
		{
			return _iDeviceModalRepository.GetDeviceModal(Id);
		}

		public DeviceModal UpdateDeviceModal(DeviceModal entity)
		{
			return _iDeviceModalRepository.UpdateDeviceModal(entity);
		}

		public DeviceModal UpdateDeviceModalByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iDeviceModalRepository.UpdateDeviceModalByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteDeviceModal(System.Int32 Id)
		{
			return _iDeviceModalRepository.DeleteDeviceModal(Id);
		}

		public List<DeviceModal> GetAllDeviceModal()
		{
			return _iDeviceModalRepository.GetAllDeviceModal();
		}

		public DeviceModal InsertDeviceModal(DeviceModal entity)
		{
			 return _iDeviceModalRepository.InsertDeviceModal(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				DeviceModal devicemodal = _iDeviceModalRepository.GetDeviceModal(id);
                if(devicemodal!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(devicemodal);
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
            List<DeviceModal> devicemodallist = _iDeviceModalRepository.GetAllDeviceModal();
            if (devicemodallist != null && devicemodallist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(devicemodallist);
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
                DeviceModal devicemodal = new DeviceModal();
                PostOutput output = new PostOutput();
                devicemodal.CopyFrom(Input);
                devicemodal = _iDeviceModalRepository.InsertDeviceModal(devicemodal);
                output.CopyFrom(devicemodal);
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
                DeviceModal devicemodalinput = new DeviceModal();
                DeviceModal devicemodaloutput = new DeviceModal();
                PutOutput output = new PutOutput();
                devicemodalinput.CopyFrom(Input);
                DeviceModal devicemodal = _iDeviceModalRepository.GetDeviceModal(devicemodalinput.Id);
                if (devicemodal!=null)
                {
                    devicemodaloutput = _iDeviceModalRepository.UpdateDeviceModal(devicemodalinput);
                    if(devicemodaloutput!=null)
                    {
                        output.CopyFrom(devicemodaloutput);
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
				 bool IsDeleted = _iDeviceModalRepository.DeleteDeviceModal(id);
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
