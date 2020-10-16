using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.CustomPayrollDetail;
using Validation;
using System.Linq;
using HRM.Core.DataTransfer.Attendance;

namespace HRM.Core.Service
{
    public class CustomPayrollDetailService : ICustomPayrollDetailService
    {
        private ICustomPayrollDetailRepository _iCustomPayrollDetailRepository;

        public CustomPayrollDetailService(ICustomPayrollDetailRepository iCustomPayrollDetailRepository)
        {
            this._iCustomPayrollDetailRepository = iCustomPayrollDetailRepository;
        }

        public Dictionary<string, string> GetCustomPayrollDetailBasicSearchColumns()
        {

            return this._iCustomPayrollDetailRepository.GetCustomPayrollDetailBasicSearchColumns();

        }

        public List<SearchColumn> GetCustomPayrollDetailAdvanceSearchColumns()
        {

            return this._iCustomPayrollDetailRepository.GetCustomPayrollDetailAdvanceSearchColumns();

        }

        //New
        public virtual List<CustomPayrollDetail> GetCustomPayrollDetailAmountByPayrollId(System.Int32? PayrollId)
        {
            return _iCustomPayrollDetailRepository.GetCustomPayrollDetailAmountByPayrollId(PayrollId);
        }


        public virtual List<CustomPayrollDetail> GetCustomPayrollDetailByPayrollId(System.Int32? PayrollPolicyId)
        {
            return _iCustomPayrollDetailRepository.GetCustomPayrollDetailByPayrollId(PayrollPolicyId);
        }

        public virtual List<CustomPayrollDetail> GetCustomPayrollDetailByPayrollPolicyId(System.Int32? PayrollPolicyId)
        {
            return _iCustomPayrollDetailRepository.GetCustomPayrollDetailByPayrollPolicyId(PayrollPolicyId);
        }

        public CustomPayrollDetail GetCustomPayrollDetail(System.Int32 Id)
        {
            return _iCustomPayrollDetailRepository.GetCustomPayrollDetail(Id);
        }

        public CustomPayrollDetail UpdateCustomPayrollDetail(CustomPayrollDetail entity)
        {
            return _iCustomPayrollDetailRepository.UpdateCustomPayrollDetail(entity);
        }

        public CustomPayrollDetail UpdateCustomPayrollDetailByKeyValue(Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
        {
            return _iCustomPayrollDetailRepository.UpdateCustomPayrollDetailByKeyValue(UpdateKeyValue, Id);
        }

        public bool DeleteCustomPayrollDetail(System.Int32 Id)
        {
            return _iCustomPayrollDetailRepository.DeleteCustomPayrollDetail(Id);
        }

        public List<CustomPayrollDetail> GetAllCustomPayrollDetail()
        {
            return _iCustomPayrollDetailRepository.GetAllCustomPayrollDetail();
        }

        public CustomPayrollDetail InsertCustomPayrollDetail(CustomPayrollDetail entity)
        {
            return _iCustomPayrollDetailRepository.InsertCustomPayrollDetail(entity);
        }


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id = 0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id, out id))
            {
                CustomPayrollDetail payrolldetail = _iCustomPayrollDetailRepository.GetCustomPayrollDetail(id);
                if (payrolldetail != null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(payrolldetail);
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
            List<CustomPayrollDetail> payrolldetaillist = _iCustomPayrollDetailRepository.GetAllCustomPayrollDetail();
            if (payrolldetaillist != null && payrolldetaillist.Count > 0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(payrolldetaillist);
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
                CustomPayrollDetail Custompayrolldetail = new CustomPayrollDetail();
                PostOutput output = new PostOutput();
                Custompayrolldetail.CopyFrom(Input);
                Custompayrolldetail = _iCustomPayrollDetailRepository.InsertCustomPayrollDetail(Custompayrolldetail);
                output.CopyFrom(Custompayrolldetail);
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
                CustomPayrollDetail Custompayrolldetailinput = new CustomPayrollDetail();
                CustomPayrollDetail Custompayrolldetailoutput = new CustomPayrollDetail();
                PutOutput output = new PutOutput();
                Custompayrolldetailinput.CopyFrom(Input);
                CustomPayrollDetail payrolldetail = _iCustomPayrollDetailRepository.GetCustomPayrollDetail(Custompayrolldetailinput.Id);
                if (payrolldetail != null)
                {
                    Custompayrolldetailoutput = _iCustomPayrollDetailRepository.UpdateCustomPayrollDetail(Custompayrolldetailinput);
                    if (Custompayrolldetailoutput != null)
                    {
                        output.CopyFrom(Custompayrolldetailoutput);
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
                bool IsDeleted = _iCustomPayrollDetailRepository.DeleteCustomPayrollDetail(id);
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
    }
}
