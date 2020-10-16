using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.AttendanceType;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class AttendanceTypeService : IAttendanceTypeService 
	{
		private IAttendanceTypeRepository _iAttendanceTypeRepository;
        
		public AttendanceTypeService(IAttendanceTypeRepository iAttendanceTypeRepository)
		{
			this._iAttendanceTypeRepository = iAttendanceTypeRepository;
		}
        
        public Dictionary<string, string> GetAttendanceTypeBasicSearchColumns()
        {
            
            return this._iAttendanceTypeRepository.GetAttendanceTypeBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetAttendanceTypeAdvanceSearchColumns()
        {
            
            return this._iAttendanceTypeRepository.GetAttendanceTypeAdvanceSearchColumns();
           
        }
        

		public AttendanceType GetAttendanceType(System.Int32 Id)
		{
			return _iAttendanceTypeRepository.GetAttendanceType(Id);
		}

		public AttendanceType UpdateAttendanceType(AttendanceType entity)
		{
			return _iAttendanceTypeRepository.UpdateAttendanceType(entity);
		}

		public AttendanceType UpdateAttendanceTypeByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iAttendanceTypeRepository.UpdateAttendanceTypeByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteAttendanceType(System.Int32 Id)
		{
			return _iAttendanceTypeRepository.DeleteAttendanceType(Id);
		}

		public List<AttendanceType> GetAllAttendanceType()
		{
			return _iAttendanceTypeRepository.GetAllAttendanceType();
		}

		public AttendanceType InsertAttendanceType(AttendanceType entity)
		{
			 return _iAttendanceTypeRepository.InsertAttendanceType(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				AttendanceType attendancetype = _iAttendanceTypeRepository.GetAttendanceType(id);
                if(attendancetype!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(attendancetype);
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
            List<AttendanceType> attendancetypelist = _iAttendanceTypeRepository.GetAllAttendanceType();
            if (attendancetypelist != null && attendancetypelist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(attendancetypelist);
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
                AttendanceType attendancetype = new AttendanceType();
                PostOutput output = new PostOutput();
                attendancetype.CopyFrom(Input);
                attendancetype = _iAttendanceTypeRepository.InsertAttendanceType(attendancetype);
                output.CopyFrom(attendancetype);
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
                AttendanceType attendancetypeinput = new AttendanceType();
                AttendanceType attendancetypeoutput = new AttendanceType();
                PutOutput output = new PutOutput();
                attendancetypeinput.CopyFrom(Input);
                AttendanceType attendancetype = _iAttendanceTypeRepository.GetAttendanceType(attendancetypeinput.Id);
                if (attendancetype!=null)
                {
                    attendancetypeoutput = _iAttendanceTypeRepository.UpdateAttendanceType(attendancetypeinput);
                    if(attendancetypeoutput!=null)
                    {
                        output.CopyFrom(attendancetypeoutput);
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
				 bool IsDeleted = _iAttendanceTypeRepository.DeleteAttendanceType(id);
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
