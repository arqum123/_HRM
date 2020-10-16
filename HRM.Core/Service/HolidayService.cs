using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Holiday;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class HolidayService : IHolidayService 
	{
		private IHolidayRepository _iHolidayRepository;
        
		public HolidayService(IHolidayRepository iHolidayRepository)
		{
			this._iHolidayRepository = iHolidayRepository;
		}
        
        public Dictionary<string, string> GetHolidayBasicSearchColumns()
        {
            
            return this._iHolidayRepository.GetHolidayBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetHolidayAdvanceSearchColumns()
        {
            
            return this._iHolidayRepository.GetHolidayAdvanceSearchColumns();
           
        }
        

		public Holiday GetHoliday(System.Int32 Id)
		{
			return _iHolidayRepository.GetHoliday(Id);
		}

		public Holiday UpdateHoliday(Holiday entity)
		{
			return _iHolidayRepository.UpdateHoliday(entity);
		}

		public Holiday UpdateHolidayByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iHolidayRepository.UpdateHolidayByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteHoliday(System.Int32 Id)
		{
			return _iHolidayRepository.DeleteHoliday(Id);
		}

		public List<Holiday> GetAllHoliday()
		{
			return _iHolidayRepository.GetAllHoliday();
		}

		public Holiday InsertHoliday(Holiday entity)
		{
			 return _iHolidayRepository.InsertHoliday(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				Holiday holiday = _iHolidayRepository.GetHoliday(id);
                if(holiday!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(holiday);
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
            List<Holiday> holidaylist = _iHolidayRepository.GetAllHoliday();
            if (holidaylist != null && holidaylist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(holidaylist);
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
                Holiday holiday = new Holiday();
                PostOutput output = new PostOutput();
                holiday.CopyFrom(Input);
                holiday = _iHolidayRepository.InsertHoliday(holiday);
                output.CopyFrom(holiday);
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
                Holiday holidayinput = new Holiday();
                Holiday holidayoutput = new Holiday();
                PutOutput output = new PutOutput();
                holidayinput.CopyFrom(Input);
                Holiday holiday = _iHolidayRepository.GetHoliday(holidayinput.Id);
                if (holiday!=null)
                {
                    holidayoutput = _iHolidayRepository.UpdateHoliday(holidayinput);
                    if(holidayoutput!=null)
                    {
                        output.CopyFrom(holidayoutput);
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
				 bool IsDeleted = _iHolidayRepository.DeleteHoliday(id);
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
