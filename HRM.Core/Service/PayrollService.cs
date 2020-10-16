using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Payroll;
using Validation;
using System.Linq;
using HRM.Core.Model;

namespace HRM.Core.Service
{

    public class PayrollService : IPayrollService
    {
        private IPayrollRepository _iPayrollRepository;

        public PayrollService(IPayrollRepository iPayrollRepository)
        {
            this._iPayrollRepository = iPayrollRepository;
        }

        public Dictionary<string, string> GetPayrollBasicSearchColumns()
        {

            return this._iPayrollRepository.GetPayrollBasicSearchColumns();

        }

        public List<SearchColumn> GetPayrollAdvanceSearchColumns()
        {

            return this._iPayrollRepository.GetPayrollAdvanceSearchColumns();

        }


        public virtual List<Payroll> GetPayrollByPayrollCycleId(System.Int32? PayrollCycleId)
        {
            return _iPayrollRepository.GetPayrollByPayrollCycleId(PayrollCycleId);
        }
        public virtual List<VMModifyPayrollEdit> GetModifyPayrollByPayrollCycleId(System.Int32? PayrollCycleId, System.Int32? DepartmentId, System.String UserName) //NewModfyPayroll
        {
            return _iPayrollRepository.GetModifyPayrollByPayrollCycleId(PayrollCycleId, DepartmentId, UserName);
        }
        public virtual List<VMModifyPayrollEdit> GetModifyPayslipEdit(System.Int32? PayrollId, System.Int32? DepartmentId, System.Int32? UserId) //NewModfyPayroll
        {
            return _iPayrollRepository.GetModifyPayslipEdit(PayrollId,  DepartmentId,  UserId);
        }
        public Payroll GetPayroll(System.Int32 Id)
        {
            return _iPayrollRepository.GetPayroll(Id);
        }

        public Payroll UpdatePayroll(Payroll entity)
        {
            return _iPayrollRepository.UpdatePayroll(entity);
        }

        public Payroll UpdatePayrollByKeyValue(Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
        {
            return _iPayrollRepository.UpdatePayrollByKeyValue(UpdateKeyValue, Id);
        }

        public bool DeletePayroll(System.Int32 Id)
        {
            return _iPayrollRepository.DeletePayroll(Id);
        }

        public List<Payroll> GetAllPayroll()
        {
            return _iPayrollRepository.GetAllPayroll();
        }

        public Payroll InsertPayroll(Payroll entity)
        {
            return _iPayrollRepository.InsertPayroll(entity);
        }


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id = 0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id, out id))
            {
                Payroll payroll = _iPayrollRepository.GetPayroll(id);
                if (payroll != null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(payroll);
                    tranfer.Data = output;

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
            List<Payroll> payrolllist = _iPayrollRepository.GetAllPayroll();
            if (payrolllist != null && payrolllist.Count > 0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(payrolllist);
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
            if (errors.Count == 0)
            {
                Payroll payroll = new Payroll();
                PostOutput output = new PostOutput();
                payroll.CopyFrom(Input);
                payroll = _iPayrollRepository.InsertPayroll(payroll);
                output.CopyFrom(payroll);
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
                Payroll payrollinput = new Payroll();
                Payroll payrolloutput = new Payroll();
                PutOutput output = new PutOutput();
                payrollinput.CopyFrom(Input);
                Payroll payroll = _iPayrollRepository.GetPayroll(payrollinput.Id);
                if (payroll != null)
                {
                    payrolloutput = _iPayrollRepository.UpdatePayroll(payrollinput);
                    if (payrolloutput != null)
                    {
                        output.CopyFrom(payrolloutput);
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
            System.Int32 id = 0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id, out id))
            {
                bool IsDeleted = _iPayrollRepository.DeletePayroll(id);
                if (IsDeleted)
                {
                    tranfer.IsSuccess = true;
                    tranfer.Data = IsDeleted.ToString().ToLower();

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

        public List<Payroll> GetAllPayrollWithUser(int BranchID)
        {
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            List<User> BranchUsers = objUserService.GetAllUserWithDepartment(BranchID);

            IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");

            List<Payroll> PayrollList = _iPayrollRepository.GetAllPayroll();
            List<Payroll> tempPayrollList = new List<Payroll>();
            if (PayrollList != null && PayrollList.Count > 0)
            {
                PayrollList = PayrollList.Where(x => x.IsActive == true).ToList();
                tempPayrollList.AddRange(PayrollList);
                foreach (var payroll in tempPayrollList)
                {
                    if (BranchUsers.Exists(x => x.Id == payroll.UserId))
                    {
                        payroll.User = objUserService.GetUserWithDepartment((int)payroll.UserId);
                        payroll.PayrollCycle = objPayrollCycleService.GetPayrollCycle((int)payroll.PayrollCycleId);
                    }
                    else
                        PayrollList.Remove(payroll);
                }
            }
            return PayrollList;
        }

      

        public List<Payroll> GetAllPayrollWithUserWithTestPayroll(int BranchID)
        {
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            List<User> BranchUsers = objUserService.GetAllUserWithDepartment(BranchID);

            IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");

            List<Payroll> PayrollList = _iPayrollRepository.GetAllPayroll();
            List<Payroll> tempPayrollList = new List<Payroll>();
            if (PayrollList != null && PayrollList.Count > 0)
            {
                //PayrollList = PayrollList.Where(x => x.IsActive == true).ToList();
                tempPayrollList.AddRange(PayrollList);
                foreach (var payroll in tempPayrollList)
                {
                    if (BranchUsers.Exists(x => x.Id == payroll.UserId))
                    {
                        payroll.User = objUserService.GetUserWithDepartment((int)payroll.UserId);
                        payroll.PayrollCycle = objPayrollCycleService.GetPayrollCycle((int)payroll.PayrollCycleId);
                    }
                    else
                        PayrollList.Remove(payroll);
                }
            }
            return PayrollList;
        }

        public List<Payroll> GetAllPayrollWithUser(int BranchID, int IsActive)
        {
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            List<User> BranchUsers = objUserService.GetAllUserWithDepartment(BranchID);

            IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");

            List<Payroll> PayrollList = _iPayrollRepository.GetAllPayroll();
            List<Payroll> tempPayrollList = new List<Payroll>();
            if (PayrollList != null && PayrollList.Count > 0)
            {
                PayrollList = PayrollList.Where(x => x.IsActive == Convert.ToBoolean(IsActive)).ToList();
                tempPayrollList.AddRange(PayrollList);
                foreach (var payroll in tempPayrollList)
                {
                    if (BranchUsers.Exists(x => x.Id == payroll.UserId))
                    {
                        payroll.User = objUserService.GetUserWithDepartment((int)payroll.UserId);
                        payroll.PayrollCycle = objPayrollCycleService.GetPayrollCycle((int)payroll.PayrollCycleId);
                    }
                    else
                        PayrollList.Remove(payroll);
                }
            }
            return PayrollList;
        }

        public void RunPayroll(int PayrollCycleID, int IsFinal)
        {
            _iPayrollRepository.RunPayroll(PayrollCycleID, IsFinal);
        }

        //public List<VMModifyPayrollEdit> GetModifyPayslipEdit(int? PayrollId, int? DepartmentId, int? UserId)
        //{
        //    throw new NotImplementedException();
        //}
    }


}
