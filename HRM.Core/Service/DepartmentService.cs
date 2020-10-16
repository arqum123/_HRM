using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Department;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{

    public class DepartmentService : IDepartmentService
    {
        private IDepartmentRepository _iDepartmentRepository;

        public DepartmentService(IDepartmentRepository iDepartmentRepository)
        {
            this._iDepartmentRepository = iDepartmentRepository;
        }

        public Dictionary<string, string> GetDepartmentBasicSearchColumns()
        {

            return this._iDepartmentRepository.GetDepartmentBasicSearchColumns();

        }

        public List<SearchColumn> GetDepartmentAdvanceSearchColumns()
        {

            return this._iDepartmentRepository.GetDepartmentAdvanceSearchColumns();

        }


        public Department GetDepartment(System.Int32 Id)
        {
            return _iDepartmentRepository.GetDepartment(Id);
        }

        public Department UpdateDepartment(Department entity)
        {
            return _iDepartmentRepository.UpdateDepartment(entity);
        }

        public Department UpdateDepartmentByKeyValue(Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
        {
            return _iDepartmentRepository.UpdateDepartmentByKeyValue(UpdateKeyValue, Id);
        }

        public bool DeleteDepartment(System.Int32 Id)
        {
            return _iDepartmentRepository.DeleteDepartment(Id);
        }

        public List<Department> GetAllDepartment()
        {
            return _iDepartmentRepository.GetAllDepartment();
        }

        public Department InsertDepartment(Department entity)
        {
            return _iDepartmentRepository.InsertDepartment(entity);
        }


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id = 0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id, out id))
            {
                Department department = _iDepartmentRepository.GetDepartment(id);
                if (department != null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(department);
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
            List<Department> departmentlist = _iDepartmentRepository.GetAllDepartment();
            if (departmentlist != null && departmentlist.Count > 0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(departmentlist);
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
                Department department = new Department();
                PostOutput output = new PostOutput();
                department.CopyFrom(Input);
                department = _iDepartmentRepository.InsertDepartment(department);
                output.CopyFrom(department);
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
                Department departmentinput = new Department();
                Department departmentoutput = new Department();
                PutOutput output = new PutOutput();
                departmentinput.CopyFrom(Input);
                Department department = _iDepartmentRepository.GetDepartment(departmentinput.Id);
                if (department != null)
                {
                    departmentoutput = _iDepartmentRepository.UpdateDepartment(departmentinput);
                    if (departmentoutput != null)
                    {
                        output.CopyFrom(departmentoutput);
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
                bool IsDeleted = _iDepartmentRepository.DeleteDepartment(id);
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

        public List<Department> GetAllDepartment(int BranchID)
        {
            List<Department> _deptList = new List<Department>();
            List<Department> _tempDeptList = new List<Department>();
            _tempDeptList = _iDepartmentRepository.GetAllDepartment();
            
            IBranchDepartmentService iBranchDepartmentService = IoC.Resolve<IBranchDepartmentService>("BranchDepartmentService");
            List<BranchDepartment> _branchDeptList = new List<BranchDepartment>();
            _branchDeptList = iBranchDepartmentService.GetBranchDepartmentByBranchId(BranchID);

            if (_tempDeptList != null && _branchDeptList != null && BranchID > 0)
            {
                _tempDeptList = _tempDeptList.Where(x => x.IsActive == true).ToList();
                _deptList.AddRange(_tempDeptList);
                
                
                foreach (var d in _tempDeptList)
                {
                    if (!_branchDeptList.Where(z => z.DepartmentId == d.Id).Any())
                        _deptList.Remove(d);
                }
            }
            return _deptList;
        }
    }


}
