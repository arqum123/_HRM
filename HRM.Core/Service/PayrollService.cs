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
using System.Data;

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

        public virtual System.Data.DataSet GetPendingTicketsDetail() //NewModfyPayroll
        {
            return _iPayrollRepository.GetPendingTicketsDetail();
        }
        public virtual System.Data.DataSet GetTicketsByDateRangeDetail(DateTime StartDate,DateTime EndDate,int?UserId,int?DepartmentId) //NewModfyPayroll
        {
            return _iPayrollRepository.GetTicketsByDateRangeDetail(StartDate,EndDate,UserId,DepartmentId);
        }
        public virtual List<Payroll> GetPayrollByPayrollCycleId(System.Int32? PayrollCycleId)
        {
            return _iPayrollRepository.GetPayrollByPayrollCycleId(PayrollCycleId);
        }
        public virtual System.Data.DataSet GetModifyPayslipEdit(System.Int32? PayrollId, System.Int32? DepartmentId, System.Int32? UserId) //NewModfyPayroll
        {
            return _iPayrollRepository.GetModifyPayslipEdit(PayrollId, DepartmentId, UserId);
        }
        public virtual System.Data.DataSet GetPayslipDetail(System.Int32? PayrollCycleId,  System.Int32? UserId) //NewModfyPayroll
        {
            return _iPayrollRepository.GetPayslipDetail(PayrollCycleId, UserId);
        }
        public virtual System.Data.DataSet GetEmpPayslipDetail(System.Int32? PayrollCycleId, System.Int32? UserId) //NewModfyPayroll
        {
            return _iPayrollRepository.GetEmpPayslipDetail(PayrollCycleId, UserId);
        }
        public virtual System.Data.DataSet GetEmpPayrollDetail( System.Int32?PayrollCycleId,System.Int32? UserId) //NewEmpPayroll
        {
            return _iPayrollRepository.GetEmpPayrollDetail(PayrollCycleId,UserId);
        }
        public virtual System.Data.DataSet GetModifyPayrollByPayrollCycleId(System.Int32? PayrollCycleId, System.Int32? DepartmentId, System.String UserName) //NewModfyPayroll
        {
            return _iPayrollRepository.GetModifyPayrollByPayrollCycleId(PayrollCycleId, DepartmentId, UserName);
        }
        public virtual List<VMModifyPayrollEdit> GetModifyPayslipDetailEdit(System.Int32? PayrollId, System.Int32? PayrollVariableId, System.Decimal? Addition, System.Decimal? Deduction) //NewModfyPayroll
        {
            return _iPayrollRepository.GetModifyPayslipDetailEdit(PayrollId, PayrollVariableId, Addition, Deduction);
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
        public void RunPayroll(int PayrollCycleID, int IsFinal)
        {
            _iPayrollRepository.RunPayroll(PayrollCycleID, IsFinal);
        }
        private string StringNull(object val) { if (val == null || val == DBNull.Value) return ""; else return val.ToString(); }
        private DateTime? DateNull(object val) { if (val == null || val == DBNull.Value) return null; else return Convert.ToDateTime(val); }
        private Double DoubleNull(object val) { if (val == null || val == DBNull.Value) return 0.0; else return Convert.ToDouble(val); }

        private Decimal DecimalNull(object val) { if (val == null || val == DBNull.Value) return 0; else return Convert.ToDecimal(val); }
        private Int32 IntNull(object val) { if (val == null || val == DBNull.Value) return 0; else return Convert.ToInt32(val); }
        private Boolean BooleanNull(object val) { if (val == null || val == DBNull.Value) return false; else return Convert.ToBoolean(val); }
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

   

        public List<VMGetPayrollEditSecond> GetModifyPayrollSummaryByPayrollCycleId(Int32? DepartmentId, string UserName, Int32? PayrollCycleId)
        {
            //var Decimal DecimalNull(object val) { if (val == null || val == DBNull.Value) return 0; else return Convert.ToDecimal(val); }

            List<VMGetPayrollEditSecond> payrollSummary = new List<VMGetPayrollEditSecond>();
            DataSet dsPayroll = _iPayrollRepository.GetModifyPayrollByPayrollCycleId(PayrollCycleId, DepartmentId, UserName);
            if (dsPayroll != null && dsPayroll.Tables.Count > 0 && dsPayroll.Tables[0] != null)
            {
                //dsPayroll.Tables[0] // Payroll , PayrollCycle,User and Department
                //dsPayroll.Tables[1] // PayrollDetail ,PayrollVariable,SalaryType  
                foreach (DataRow dr in dsPayroll.Tables[0].Rows)
                {
                    VMGetPayrollEditSecond objPayrollEdit = new VMGetPayrollEditSecond()
                    {
                        PayrollId = IntNull(dr["PayrollId"]),
                        UserId = IntNull(dr["UserId"]),
                        PayrollCycleId = IntNull(dr["PayrollCycleId"]),
                        DepartmentId = IntNull(dr["DepartmentId"]),
                        SalaryTypeId = IntNull(dr["SalaryTypeId"]),
                        UserName = dr["UserName"].ToString(),
                        DepartmentName = dr["DepartmentName"].ToString(),
                        Designation = dr["Designation"].ToString(),
                        NICNo = dr["NICNo"].ToString(),
                        SalaryTypeName = dr["SalaryTypeName"].ToString(),
                        Salary = DecimalNull(dr["Salary"]),
                        NetSalary = DecimalNull(dr["NetSalary"]),
                        Addition = DecimalNull(dr["Addition"]),
                        Deduction = DecimalNull(dr["Deduction"]),
                    };


                    payrollSummary.Add(objPayrollEdit);
                }
            }
            return payrollSummary;
        }

        public List<VMUserPayrollEdit> GetModifyPayrollSummary(Int32? PayrollId, Int32? DepartmentId, Int32? UserId)
        {
            List<VMUserPayrollEdit> payrollSummary = new List<VMUserPayrollEdit>();
            DataSet dsPayroll = _iPayrollRepository.GetModifyPayslipEdit(PayrollId, DepartmentId, UserId);
            if (dsPayroll != null && dsPayroll.Tables.Count > 0 && dsPayroll.Tables[0] != null)
            {
                //dsPayroll.Tables[0] // Payroll,Department,User,PayrollCycle 
                //dsPayroll.Tables[1] // PayrollDetail  ,PayrollVariable,PayrollPolicy
                foreach (DataRow dr in dsPayroll.Tables[0].Rows)
                {
                    VMUserPayrollEdit objPayroll = new VMUserPayrollEdit()
                    {
                        PayrollId = IntNull(dr["PayrollId"]), //Payroll
                        UserId = IntNull(dr["UserId"]), //User
                        DepartmentId = IntNull(dr["DepartmentId"]), //Department
                        SalaryTypeId = IntNull(dr["SalaryTypeId"]), //SalaryType
                        PayrollCycleId = IntNull(dr["PayrollCycleId"]), //PayrollCycle
                        UserFName = dr["UserFName"].ToString(), //User
                        UserMName = dr["UserMName"].ToString(), //User
                        UserLName = dr["UserLName"].ToString(), //User
                        Designation = dr["Designation"].ToString(), //User
                        NICNo = dr["NICNo"].ToString(), //User
                        DepartmentName = dr["DepartmentName"].ToString(), //Department
                        PayrollCycleName = dr["PayrollCycleName"].ToString(), //PayrollCycle
                        PayrollCycleMonth = dr["PayrollCycleMonth"].ToString(), //PayrollCycle
                        PayrollCycleYear = dr["PayrollCycleYear"].ToString(), //PayrollCycle
                        Salary = DecimalNull(dr["Salary"]), //Payroll
                        Addition = DecimalNull(dr["Addition"]), //Payroll
                        Deduction = DecimalNull(dr["Deduction"]), //Payroll
                        NetSalary = DecimalNull(dr["NetSalary"]), //Payroll
                        VMUserPayrollDetailEditList = new List<VMUserPayrollDetailEdit>()
                    };
                    foreach (DataRow drDetail in dsPayroll.Tables[1].Select("PayrollId=" + objPayroll.PayrollId.ToString()))
                    {
                        VMUserPayrollDetailEdit objPayrollDetail = new VMUserPayrollDetailEdit()
                        {
                            PayrollDetailId = IntNull(drDetail["PayrollDetailId"]), //PayrollDetail
                            Amount = DecimalNull(drDetail["Amount"]), //PayrollDetail
                            PayrollVariableId = IntNull(drDetail["PayrollVariableId"]), //PayrollVariable
                            PayrollPolicyName = drDetail["PayrollPolicyName"].ToString(), //PayrollPolicy
                            PayrollVariableName = drDetail["PayrollVariableName"].ToString(), //PayrollVariable
                            PayrollVariableIsActive = drDetail["PayrollVariableIsActive"].ToString(), //PayrollVariable
                        };
                        objPayroll.VMUserPayrollDetailEditList.Add(objPayrollDetail);     
                    }
                    payrollSummary.Add(objPayroll);
                }
            };
            return payrollSummary;
        }
        public List<VMEmpPayrollService> GetEmpPayrollByPayrollCycleId(Int32?PayrollCycleId,Int32? UserId)
        {
            //var Decimal DecimalNull(object val) { if (val == null || val == DBNull.Value) return 0; else return Convert.ToDecimal(val); }

            List<VMEmpPayrollService> payrollSummary = new List<VMEmpPayrollService>();
            DataSet dsPayroll = _iPayrollRepository.GetEmpPayrollDetail(PayrollCycleId,UserId);
            if (dsPayroll != null && dsPayroll.Tables.Count > 0 && dsPayroll.Tables[0] != null)
            {
                //dsPayroll.Tables[0] // Payroll , PayrollCycle,User and Department
                //dsPayroll.Tables[1] // PayrollDetail ,PayrollVariable,SalaryType  
                foreach (DataRow dr in dsPayroll.Tables[0].Rows)
                {
                    VMEmpPayrollService objPayrollEdit = new VMEmpPayrollService()
                    {
                        PayrollCycleId = IntNull(dr["PayrollCycleId"]),
                        Name = StringNull(dr["Name"]),
                        UserId = IntNull(dr["UserId"]),
                        Salary = DoubleNull(dr["Salary"]),
                        NetSalary = DoubleNull(dr["NetSalary"]),
                        Addition = DoubleNull(dr["Addition"]),
                        Deduction = DoubleNull(dr["Deduction"]),
                    };
                    payrollSummary.Add(objPayrollEdit);
                }
            }
            return payrollSummary;
        }

        public List<VMPayslipDetailUser> GetPayslip(Int32? PayrollCycleId, Int32? UserId)
        {
            List<VMPayslipDetailUser> payslipSummary = new List<VMPayslipDetailUser>();
            DataSet dsPayroll = _iPayrollRepository.GetPayslipDetail(PayrollCycleId, UserId);
            if (dsPayroll != null && dsPayroll.Tables.Count > 0 && dsPayroll.Tables[0] != null)
            {
                //dsPayroll.Tables[0] // PayrollCycle , User,Depart
                //dsPayroll.Tables[1] // Payroll  
                //dsPayroll.Tables[2] // PayrollDetail , PayrollPolicy , PayrollVariable
                foreach (DataRow dr in dsPayroll.Tables[0].Rows)
                {
                    VMPayslipDetailUser objPayroll = new VMPayslipDetailUser()
                    {
                        PayrollCycleId = IntNull(dr["PayrollCycleId"]),
                        PayrollCycleName = dr["PayrollCycleName"].ToString(),
                        PayrollCycleIsActive =BooleanNull(dr["PayrollCycleIsActive"]),
                        EmpId = IntNull(dr["EmpId"]),
                        EmpName = dr["EmpName"].ToString(),
                        Designation = dr["Designation"].ToString(),
                        DName = dr["DName"].ToString(),
                        VMPayslipDetailPayrollList = new List<VMPayslipDetailPayroll>()
                    };
                    foreach (DataRow drDetail in dsPayroll.Tables[1].Select("PayrollCycleId=" + objPayroll.PayrollCycleId.ToString()))
                    {
                        VMPayslipDetailPayroll objPayrollDetail = new VMPayslipDetailPayroll()
                        {
                            PayrollCycleId = IntNull(drDetail["PayrollCycleId"]),
                            PayrollId = IntNull(drDetail["PayrollId"]),
                            Salary = DoubleNull(drDetail["Salary"]),
                            NetSalary = DoubleNull(drDetail["NetSalary"]),
                            TotalAddition = DoubleNull(drDetail["TotalAddition"]),
                            TotalDeduction = DoubleNull(drDetail["TotalDeduction"]), //Problem is here
                            VMPayslipDetailVariablesList = new List<VMPayslipDetailVariables>()
                        };


                        foreach (DataRow drDetailVariable in dsPayroll.Tables[2].Select("PayrollId=" + objPayrollDetail.PayrollId.ToString()))
                        {
                            VMPayslipDetailVariables objPayrollVariable = new VMPayslipDetailVariables()
                            {
                                PayrollId = IntNull(drDetailVariable["PayrollId"]),
                                PayrollDetailId = IntNull(drDetailVariable["PayrollDetailId"]),
                                PayrollPolicyId = IntNull(drDetailVariable["PayrollPolicyId"]),
                                PayrollVariableId = IntNull(drDetailVariable["PayrollVariableId"]),
                                PayrollVariableName = drDetailVariable["PayrollVariableName"].ToString(),
                                PayrollPolicyName = drDetailVariable["PayrollPolicyName"].ToString(),
                                Amount = DoubleNull(drDetailVariable["Amount"]),
                            };
                            objPayrollDetail.VMPayslipDetailVariablesList.Add(objPayrollVariable);
                        }
                        objPayroll.VMPayslipDetailPayrollList.Add(objPayrollDetail);
                    }
                    payslipSummary.Add(objPayroll);
                }
               
            };
            return payslipSummary;
        }
        public List<VMEmpPayslipDetailUser> GetEmpPayslip(Int32? PayrollCycleId, Int32? UserId)
        {
            List<VMEmpPayslipDetailUser> payslipSummary = new List<VMEmpPayslipDetailUser>();
            DataSet dsPayroll = _iPayrollRepository.GetPayslipDetail(PayrollCycleId, UserId);
            if (dsPayroll != null && dsPayroll.Tables.Count > 0 && dsPayroll.Tables[0] != null)
            {
                //dsPayroll.Tables[0] // PayrollCycle , User,Depart
                //dsPayroll.Tables[1] // Payroll  
                //dsPayroll.Tables[2] // PayrollDetail , PayrollPolicy , PayrollVariable
                foreach (DataRow dr in dsPayroll.Tables[0].Rows)
                {
                    VMEmpPayslipDetailUser objPayroll = new VMEmpPayslipDetailUser()
                    {
                        PayrollCycleId = IntNull(dr["PayrollCycleId"]),
                        PayrollCycleName = dr["PayrollCycleName"].ToString(),
                        PayrollCycleIsActive = BooleanNull(dr["PayrollCycleIsActive"]),
                        EmpId = IntNull(dr["EmpId"]),
                        EmpName = dr["EmpName"].ToString(),
                        Designation = dr["Designation"].ToString(),
                        DName = dr["DName"].ToString(),
                        VMEmpPayslipDetailPayrollList = new List<VMEmpPayslipDetailPayroll>()
                    };
                    foreach (DataRow drDetail in dsPayroll.Tables[1].Select("PayrollCycleId=" + objPayroll.PayrollCycleId.ToString()))
                    {
                        VMEmpPayslipDetailPayroll objPayrollDetail = new VMEmpPayslipDetailPayroll()
                        {
                            PayrollCycleId = IntNull(drDetail["PayrollCycleId"]),
                            PayrollId = IntNull(drDetail["PayrollId"]),
                            Salary = DoubleNull(drDetail["Salary"]),
                            NetSalary = DoubleNull(drDetail["NetSalary"]),
                            TotalAddition = DoubleNull(drDetail["TotalAddition"]),
                            TotalDeduction = DoubleNull(drDetail["TotalDeduction"]), //Problem is here
                            VMEmpPayslipDetailVariablesList = new List<VMEmpPayslipDetailVariables>()
                        };


                        foreach (DataRow drDetailVariable in dsPayroll.Tables[2].Select("PayrollId=" + objPayrollDetail.PayrollId.ToString()))
                        {
                            VMEmpPayslipDetailVariables objPayrollVariable = new VMEmpPayslipDetailVariables()
                            {
                                PayrollId = IntNull(drDetailVariable["PayrollId"]),
                                PayrollDetailId = IntNull(drDetailVariable["PayrollDetailId"]),
                                PayrollPolicyId = IntNull(drDetailVariable["PayrollPolicyId"]),
                                PayrollVariableId = IntNull(drDetailVariable["PayrollVariableId"]),
                                PayrollVariableName = drDetailVariable["PayrollVariableName"].ToString(),
                                PayrollPolicyName = drDetailVariable["PayrollPolicyName"].ToString(),
                                Amount = DoubleNull(drDetailVariable["Amount"]),
                            };
                            objPayrollDetail.VMEmpPayslipDetailVariablesList.Add(objPayrollVariable);
                        }
                        objPayroll.VMEmpPayslipDetailPayrollList.Add(objPayrollDetail);
                    }
                    payslipSummary.Add(objPayroll);
                }
            };
            return payslipSummary;
        }

        public List<VMTicket> GetPendingTickets()
        {
            List<VMTicket> ticketSummary = new List<VMTicket>();
            DataSet dsTicket = _iPayrollRepository.GetPendingTicketsDetail();
            if (dsTicket != null && dsTicket.Tables.Count > 0 && dsTicket.Tables[0] != null)
            {
                //dsPayroll.Tables[0] // Payroll , PayrollCycle,User and Department
                //dsPayroll.Tables[1] // PayrollDetail ,PayrollVariable,SalaryType  
                foreach (DataRow dr in dsTicket.Tables[0].Rows)
                {
                    VMTicket objTicketSummary = new VMTicket();
                    {
                        objTicketSummary.TicketID = IntNull(dr["TicketID"]);
                        objTicketSummary.AttendanceID = IntNull(dr["AttendanceID"]);
                        if (DateNull(dr["TimeIn"]) != null)
                        {
                            objTicketSummary.TimeIn = DateNull(dr["TimeIn"]).Value;
                        }
                        else
                        {
                            objTicketSummary.TimeIn = DateTime.MinValue;
                        }
                        if (DateNull(dr["TimeOutt"]) != null)
                        {
                            objTicketSummary.TimeOut = DateNull(dr["TimeOutt"]).Value;
                        }
                        else
                        {
                            objTicketSummary.TimeOut = DateTime.MinValue;
                        }
                        if (DateNull(dr["AttendanceDate"]) != null)
                        {
                            objTicketSummary.AttendanceDate = DateNull(dr["AttendanceDate"]).Value;
                        }
                        else
                        {
                            objTicketSummary.AttendanceDate = DateTime.MinValue;
                        }
                        if (DateNull(dr["ShiftTimeIn"]) != null)
                        {
                            objTicketSummary.ShiftTimeIn = DateNull(dr["ShiftTimeIn"]).Value;
                        }
                        else
                        {
                            objTicketSummary.ShiftTimeIn = DateTime.MinValue;
                        }
                        if (DateNull(dr["ShiftTimeOut"]) != null)
                        {
                            objTicketSummary.ShiftTimeOut = DateNull(dr["ShiftTimeOut"]).Value;
                        }
                        else
                        {
                            objTicketSummary.ShiftTimeOut = DateTime.MinValue;
                        }
                        //objTicketSummary.TimeOut = Convert.ToDateTime(dr["TimeOutt"]);
                        //objTicketSummary.AttendanceDate = Convert.ToDateTime(dr["AttendanceDate"]);
                        //objTicketSummary.ShiftTimeIn = Convert.ToDateTime(dr["ShiftTimeIn"]);
                        //objTicketSummary.ShiftTimeOut = Convert.ToDateTime(dr["ShiftTimeOut"]);
                        objTicketSummary.OffDay = BooleanNull(dr["OffDay"]);
                        objTicketSummary.Reason = dr["Reason"].ToString();
                        objTicketSummary.IsApproved = BooleanNull(dr["IsApproved"]);
                        objTicketSummary.IsReject = BooleanNull(dr["IsReject"]);
                        objTicketSummary.Comments = dr["Comments"].ToString();
                        objTicketSummary.EmpName = dr["EmpName"].ToString();
                        objTicketSummary.DepartmentName = dr["DepartmentName"].ToString();
                        objTicketSummary.ShiftName = dr["ShiftName"].ToString();
                    };
                    ticketSummary.Add(objTicketSummary);
                }
            }
            return ticketSummary;
        }
        public List<VMTicketHistory> GetTicketsByDateRange(DateTime StartDate,DateTime EndDate,int? UserId, int? DepartmentId)
        {
            List<VMTicketHistory> ticketHistory = new List<VMTicketHistory>();
            DataSet dsTicket = _iPayrollRepository.GetTicketsByDateRangeDetail(StartDate,EndDate,UserId,DepartmentId);
            if (dsTicket != null && dsTicket.Tables.Count > 0 && dsTicket.Tables[0] != null)
            {  
                foreach (DataRow dr in dsTicket.Tables[0].Rows)
                {
                    VMTicketHistory objTicketSummary = new VMTicketHistory();
                    objTicketSummary.TicketID = IntNull(dr["TicketID"]);
                    objTicketSummary.AttendanceID = IntNull(dr["AttendanceID"]);
                    if (DateNull(dr["TimeIn"]) != null)
                    {
                        objTicketSummary.TimeIn = DateNull(dr["TimeIn"]).Value;
                    }
                    else
                    {
                        objTicketSummary.TimeIn = DateTime.MinValue;
                    }
                    if (DateNull(dr["TimeOutt"]) != null)
                    {
                        objTicketSummary.TimeOut = DateNull(dr["TimeOutt"]).Value;
                    }
                    else
                    {
                        objTicketSummary.TimeOut = DateTime.MinValue;
                    }
                    if (DateNull(dr["ShiftTimeIn"]) != null)
                    {
                        objTicketSummary.ShiftTimeIn = DateNull(dr["ShiftTimeIn"]).Value;
                    }
                    else
                    {
                        objTicketSummary.ShiftTimeIn = DateTime.MinValue;
                    }
                    if (DateNull(dr["ShiftTimeOut"]) != null)
                    {
                        objTicketSummary.ShiftTimeOut = DateNull(dr["ShiftTimeOut"]).Value;
                    }
                    else
                    {
                        objTicketSummary.ShiftTimeOut = DateTime.MinValue;
                    }
                    objTicketSummary.AttendanceDate = DateNull(dr["AttendanceDate"]).Value;
                    objTicketSummary.OffDay = BooleanNull(dr["OffDay"]);
                    objTicketSummary.Reason = dr["Reason"].ToString();
                    objTicketSummary.IsApproved = BooleanNull(dr["IsApproved"]);
                    objTicketSummary.IsReject = BooleanNull(dr["IsReject"]);
                    objTicketSummary.Comments = dr["Comments"].ToString();
                    objTicketSummary.EmpName = dr["EmpName"].ToString();
                    objTicketSummary.DepartmentName = dr["DepartmentName"].ToString();
                    objTicketSummary.ShiftName = dr["ShiftName"].ToString();

                    ticketHistory.Add(objTicketSummary);
                }
            }
            return ticketHistory;
        }
    }
}
