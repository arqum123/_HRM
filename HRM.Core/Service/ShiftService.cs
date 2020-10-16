using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Shift;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class ShiftService : IShiftService 
	{
		private IShiftRepository _iShiftRepository;
        
		public ShiftService(IShiftRepository iShiftRepository)
		{
			this._iShiftRepository = iShiftRepository;
		}
        
        public Dictionary<string, string> GetShiftBasicSearchColumns()
        {
            
            return this._iShiftRepository.GetShiftBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetShiftAdvanceSearchColumns()
        {
            
            return this._iShiftRepository.GetShiftAdvanceSearchColumns();
           
        }
        

		public Shift GetShift(System.Int32 Id)
		{
			return _iShiftRepository.GetShift(Id);
		}

        public Shift GetShiftWithOffDays(System.Int32 Id)
        {
            Shift Shift = _iShiftRepository.GetShift(Id);
            IShiftOffDayService ObjShiftOffDayService = IoC.Resolve<IShiftOffDayService>("ShiftOffDayService");
            List<ShiftOffDay> ShiftOffDayList = ObjShiftOffDayService.GetAllShiftOffDay().Where(x => x.ShiftId == Shift.Id && x.RetiredDate == null).ToList();

            if (ShiftOffDayList.Count > 0)
                Shift.ShiftOffDayList = ShiftOffDayList.OrderBy(x => x.OffDayOfWeek).ToList();
            return Shift;
        }

        public Shift GetShiftWithOffDaysHistory(System.Int32 Id)
        {
            Shift Shift = _iShiftRepository.GetShift(Id);
            if (Shift != null)
            {
                IShiftOffDayService ObjShiftOffDayService = IoC.Resolve<IShiftOffDayService>("ShiftOffDayService");
                List<ShiftOffDay> ShiftOffDayList = ObjShiftOffDayService.GetAllShiftOffDay().Where(x => x.ShiftId == Shift.Id).ToList();

                if (ShiftOffDayList != null && ShiftOffDayList.Count > 0)
                {
                    Shift.ShiftOffDayList = ShiftOffDayList.OrderBy(x => x.EffectiveDate).ThenByDescending(x => x.RetiredDate).ToList();
                }
            }
            return Shift;
        }

		public Shift UpdateShift(Shift entity)
		{
			return _iShiftRepository.UpdateShift(entity);
		}

		public Shift UpdateShiftByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iShiftRepository.UpdateShiftByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteShift(System.Int32 Id)
		{
			return _iShiftRepository.DeleteShift(Id);
		}

		public List<Shift> GetAllShift()
		{
			return _iShiftRepository.GetAllShift();
		}

        public List<Shift> GetAllShiftWithOffDays()
        {
            List<Shift> ShiftList = _iShiftRepository.GetAllShift();
            if (ShiftList != null && ShiftList.Count > 0)
            {
                IShiftOffDayService ObjShiftOffDayService = IoC.Resolve<IShiftOffDayService>("ShiftOffDayService");
                List<ShiftOffDay> ShiftOffDayList = ObjShiftOffDayService.GetAllShiftOffDay();

                foreach (var shift in ShiftList)
                {
                    if (ShiftOffDayList != null && ShiftOffDayList.Count > 0 && ShiftOffDayList.Where(x => x.ShiftId == shift.Id && x.RetiredDate == null) != null)
                        shift.ShiftOffDayList = ShiftOffDayList.Where(x => x.ShiftId == shift.Id && x.RetiredDate == null).OrderBy(x => x.OffDayOfWeek).ToList();
                }
            }
            return ShiftList;
        }

        public List<Shift> GetAllShiftWithAttendancePolicy()
        {
            List<Shift> ShiftList = _iShiftRepository.GetAllShift();
            if (ShiftList != null && ShiftList.Count > 0)
            {
                IAttendancePolicyService ObjAttendancePolicyService = IoC.Resolve<IAttendancePolicyService>("AttendancePolicyService");
                List<AttendancePolicy> AttendancePolicyList = ObjAttendancePolicyService.GetAllAttendancePolicy();

                foreach (var shift in ShiftList)
                {
                    if (AttendancePolicyList != null && AttendancePolicyList.Count > 0 && AttendancePolicyList.Where(x => x.ShiftId == shift.Id && x.RetiredDate == null) != null)
                        shift.AttendancePolicyList = AttendancePolicyList.Where(x => x.ShiftId == shift.Id && x.RetiredDate == null).OrderBy(x => x.AttendanceVariableId).ToList();
                }
            }
            return ShiftList;
        }

		public Shift InsertShift(Shift entity)
		{
			 return _iShiftRepository.InsertShift(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				Shift shift = _iShiftRepository.GetShift(id);
                if(shift!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(shift);
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
            List<Shift> shiftlist = _iShiftRepository.GetAllShift();
            if (shiftlist != null && shiftlist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(shiftlist);
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
                Shift shift = new Shift();
                PostOutput output = new PostOutput();
                shift.CopyFrom(Input);
                shift = _iShiftRepository.InsertShift(shift);
                output.CopyFrom(shift);
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
                Shift shiftinput = new Shift();
                Shift shiftoutput = new Shift();
                PutOutput output = new PutOutput();
                shiftinput.CopyFrom(Input);
                Shift shift = _iShiftRepository.GetShift(shiftinput.Id);
                if (shift!=null)
                {
                    shiftoutput = _iShiftRepository.UpdateShift(shiftinput);
                    if(shiftoutput!=null)
                    {
                        output.CopyFrom(shiftoutput);
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
				 bool IsDeleted = _iShiftRepository.DeleteShift(id);
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

         public List<Shift> GetAllShiftWithOffDays(int BranchID)
         {
             List<Shift> ShiftList = new List<Shift>();
             List<Shift> tempShiftList = _iShiftRepository.GetAllShift();

             IBranchShiftService iBranchShiftService = IoC.Resolve<IBranchShiftService>("BranchShiftService");
             List<BranchShift> _branchShiftList = iBranchShiftService.GetBranchShiftByBranchId(BranchID);

             if (tempShiftList != null && tempShiftList.Count > 0 && _branchShiftList != null)
             {
                 tempShiftList = tempShiftList.Where(x => x.IsActive == true).ToList();
                 ShiftList.AddRange(tempShiftList);

                 IShiftOffDayService ObjShiftOffDayService = IoC.Resolve<IShiftOffDayService>("ShiftOffDayService");
                 List<ShiftOffDay> ShiftOffDayList = ObjShiftOffDayService.GetAllShiftOffDay();

                 foreach (var shift in tempShiftList)
                 {
                     if (_branchShiftList.Where(x => x.ShiftId == shift.Id).Any())
                     {
                         if (ShiftOffDayList != null && ShiftOffDayList.Count > 0 && ShiftOffDayList.Where(x => x.ShiftId == shift.Id && x.RetiredDate == null) != null)
                             shift.ShiftOffDayList = ShiftOffDayList.Where(x => x.ShiftId == shift.Id && x.RetiredDate == null).OrderBy(x => x.OffDayOfWeek).ToList();
                     }
                     else
                     {
                         ShiftList.Remove(shift);
                     }
                 }
             }
             return ShiftList;
         }

         public List<Shift> GetAllShiftWithAttendancePolicy(int BranchID)
         {
             List<Shift> ShiftList = new List<Shift>();
             List < Shift > tempShiftList = _iShiftRepository.GetAllShift(); 

             IBranchShiftService iBranchShiftService = IoC.Resolve<IBranchShiftService>("BranchShiftService");
             List<BranchShift> _branchShiftList = iBranchShiftService.GetBranchShiftByBranchId(BranchID);

             if (tempShiftList != null && tempShiftList.Count > 0 && _branchShiftList != null)
             {
                 tempShiftList = tempShiftList.Where(x => x.IsActive == true).ToList();
                 ShiftList.AddRange(tempShiftList);
                 IAttendancePolicyService ObjAttendancePolicyService = IoC.Resolve<IAttendancePolicyService>("AttendancePolicyService");
                 List<AttendancePolicy> AttendancePolicyList = ObjAttendancePolicyService.GetAllAttendancePolicy();

                 foreach (var shift in tempShiftList)
                 {
                     if (_branchShiftList.Where(x => x.ShiftId == shift.Id).Any())
                     {
                         if (AttendancePolicyList != null && AttendancePolicyList.Count > 0 && AttendancePolicyList.Where(x => x.ShiftId == shift.Id && x.RetiredDate == null) != null)
                             shift.AttendancePolicyList = AttendancePolicyList.Where(x => x.ShiftId == shift.Id && x.RetiredDate == null).OrderBy(x => x.AttendanceVariableId).ToList();
                     }
                     else
                     {
                         ShiftList.Remove(shift);
                     }
                 }
             }
             return ShiftList;
         }

         public List<Shift> GetAllShift(int BranchID)
         {
             List<Shift> _shiftList = new List<Shift>();
             List<Shift> _tempShiftList = _iShiftRepository.GetAllShift();

             IBranchShiftService iBranchShiftService = IoC.Resolve<IBranchShiftService>("BranchShiftService");
             List<BranchShift> _branchShiftList = iBranchShiftService.GetBranchShiftByBranchId(BranchID);


             if (_tempShiftList != null && BranchID > 0 && _branchShiftList != null)
             {
                 _tempShiftList = _tempShiftList.Where(x => x.IsActive == true).ToList();
                 _shiftList.AddRange(_tempShiftList);
                 foreach (var s in _tempShiftList)
                 {
                     if (!_branchShiftList.Where(x => x.ShiftId == s.Id).Any())
                     {
                         _shiftList.Remove(s);
                     }
                 }
             }

             return _shiftList;
         }
	}
	
	
}
