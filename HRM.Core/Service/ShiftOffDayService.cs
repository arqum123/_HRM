using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.ShiftOffDay;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class ShiftOffDayService : IShiftOffDayService 
	{
		private IShiftOffDayRepository _iShiftOffDayRepository;
        
		public ShiftOffDayService(IShiftOffDayRepository iShiftOffDayRepository)
		{
			this._iShiftOffDayRepository = iShiftOffDayRepository;
		}
        
        public Dictionary<string, string> GetShiftOffDayBasicSearchColumns()
        {
            
            return this._iShiftOffDayRepository.GetShiftOffDayBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetShiftOffDayAdvanceSearchColumns()
        {
            
            return this._iShiftOffDayRepository.GetShiftOffDayAdvanceSearchColumns();
           
        }
        

		public virtual List<ShiftOffDay> GetShiftOffDayByShiftId(System.Int32? ShiftId)
		{
			return _iShiftOffDayRepository.GetShiftOffDayByShiftId(ShiftId);
		}

		public ShiftOffDay GetShiftOffDay(System.Int32 Id)
		{
			return _iShiftOffDayRepository.GetShiftOffDay(Id);
		}

		public ShiftOffDay UpdateShiftOffDay(ShiftOffDay entity)
		{
			return _iShiftOffDayRepository.UpdateShiftOffDay(entity);
		}

		public ShiftOffDay UpdateShiftOffDayByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iShiftOffDayRepository.UpdateShiftOffDayByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteShiftOffDay(System.Int32 Id)
		{
			return _iShiftOffDayRepository.DeleteShiftOffDay(Id);
		}

		public List<ShiftOffDay> GetAllShiftOffDay()
		{
			return _iShiftOffDayRepository.GetAllShiftOffDay();
		}

		public ShiftOffDay InsertShiftOffDay(ShiftOffDay entity)
		{
			 return _iShiftOffDayRepository.InsertShiftOffDay(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				ShiftOffDay shiftoffday = _iShiftOffDayRepository.GetShiftOffDay(id);
                if(shiftoffday!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(shiftoffday);
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
            List<ShiftOffDay> shiftoffdaylist = _iShiftOffDayRepository.GetAllShiftOffDay();
            if (shiftoffdaylist != null && shiftoffdaylist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(shiftoffdaylist);
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
                ShiftOffDay shiftoffday = new ShiftOffDay();
                PostOutput output = new PostOutput();
                shiftoffday.CopyFrom(Input);
                shiftoffday = _iShiftOffDayRepository.InsertShiftOffDay(shiftoffday);
                output.CopyFrom(shiftoffday);
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
                ShiftOffDay shiftoffdayinput = new ShiftOffDay();
                ShiftOffDay shiftoffdayoutput = new ShiftOffDay();
                PutOutput output = new PutOutput();
                shiftoffdayinput.CopyFrom(Input);
                ShiftOffDay shiftoffday = _iShiftOffDayRepository.GetShiftOffDay(shiftoffdayinput.Id);
                if (shiftoffday!=null)
                {
                    shiftoffdayoutput = _iShiftOffDayRepository.UpdateShiftOffDay(shiftoffdayinput);
                    if(shiftoffdayoutput!=null)
                    {
                        output.CopyFrom(shiftoffdayoutput);
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
				 bool IsDeleted = _iShiftOffDayRepository.DeleteShiftOffDay(id);
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
