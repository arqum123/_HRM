using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.AttendancePolicy;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class AttendancePolicyService : IAttendancePolicyService 
	{
		private IAttendancePolicyRepository _iAttendancePolicyRepository;
        
		public AttendancePolicyService(IAttendancePolicyRepository iAttendancePolicyRepository)
		{
			this._iAttendancePolicyRepository = iAttendancePolicyRepository;
		}
        
        public Dictionary<string, string> GetAttendancePolicyBasicSearchColumns()
        {
            
            return this._iAttendancePolicyRepository.GetAttendancePolicyBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetAttendancePolicyAdvanceSearchColumns()
        {
            
            return this._iAttendancePolicyRepository.GetAttendancePolicyAdvanceSearchColumns();
           
        }
        

		public virtual List<AttendancePolicy> GetAttendancePolicyByShiftId(System.Int32? ShiftId)
		{
			return _iAttendancePolicyRepository.GetAttendancePolicyByShiftId(ShiftId);
		}

		public virtual List<AttendancePolicy> GetAttendancePolicyByAttendanceVariableId(System.Int32? AttendanceVariableId)
		{
			return _iAttendancePolicyRepository.GetAttendancePolicyByAttendanceVariableId(AttendanceVariableId);
		}

		public AttendancePolicy GetAttendancePolicy(System.Int32 Id)
		{
			return _iAttendancePolicyRepository.GetAttendancePolicy(Id);
		}

		public AttendancePolicy UpdateAttendancePolicy(AttendancePolicy entity)
		{
			return _iAttendancePolicyRepository.UpdateAttendancePolicy(entity);
		}

		public AttendancePolicy UpdateAttendancePolicyByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iAttendancePolicyRepository.UpdateAttendancePolicyByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteAttendancePolicy(System.Int32 Id)
		{
			return _iAttendancePolicyRepository.DeleteAttendancePolicy(Id);
		}

		public List<AttendancePolicy> GetAllAttendancePolicy()
		{
			return _iAttendancePolicyRepository.GetAllAttendancePolicy();
		}

		public AttendancePolicy InsertAttendancePolicy(AttendancePolicy entity)
		{
			 return _iAttendancePolicyRepository.InsertAttendancePolicy(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				AttendancePolicy attendancepolicy = _iAttendancePolicyRepository.GetAttendancePolicy(id);
                if(attendancepolicy!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(attendancepolicy);
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
            List<AttendancePolicy> attendancepolicylist = _iAttendancePolicyRepository.GetAllAttendancePolicy();
            if (attendancepolicylist != null && attendancepolicylist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(attendancepolicylist);
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
                AttendancePolicy attendancepolicy = new AttendancePolicy();
                PostOutput output = new PostOutput();
                attendancepolicy.CopyFrom(Input);
                attendancepolicy = _iAttendancePolicyRepository.InsertAttendancePolicy(attendancepolicy);
                output.CopyFrom(attendancepolicy);
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
                AttendancePolicy attendancepolicyinput = new AttendancePolicy();
                AttendancePolicy attendancepolicyoutput = new AttendancePolicy();
                PutOutput output = new PutOutput();
                attendancepolicyinput.CopyFrom(Input);
                AttendancePolicy attendancepolicy = _iAttendancePolicyRepository.GetAttendancePolicy(attendancepolicyinput.Id);
                if (attendancepolicy!=null)
                {
                    attendancepolicyoutput = _iAttendancePolicyRepository.UpdateAttendancePolicy(attendancepolicyinput);
                    if(attendancepolicyoutput!=null)
                    {
                        output.CopyFrom(attendancepolicyoutput);
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
				 bool IsDeleted = _iAttendancePolicyRepository.DeleteAttendancePolicy(id);
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
