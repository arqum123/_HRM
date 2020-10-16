using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.User;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{

    public class UserService : IUserService
    {
        private IUserRepository _iUserRepository;

        public UserService(IUserRepository iUserRepository)
        {
            this._iUserRepository = iUserRepository;
        }

        public Dictionary<string, string> GetUserBasicSearchColumns()
        {

            return this._iUserRepository.GetUserBasicSearchColumns();

        }

        public List<SearchColumn> GetUserAdvanceSearchColumns()
        {

            return this._iUserRepository.GetUserAdvanceSearchColumns();

        }


        public virtual List<User> GetUserByUserTypeId(System.Int32? UserTypeId)
        {
            return _iUserRepository.GetUserByUserTypeId(UserTypeId);
        }

        public virtual List<User> GetUserByGenderId(System.Int32? GenderId)
        {
            return _iUserRepository.GetUserByGenderId(GenderId);
        }

        public virtual List<User> GetUserByReligionId(System.Int32? ReligionId)
        {
            return _iUserRepository.GetUserByReligionId(ReligionId);
        }

        public virtual List<User> GetUserByCountryId(System.Int32? CountryId)
        {
            return _iUserRepository.GetUserByCountryId(CountryId);
        }

        public virtual List<User> GetUserByCityId(System.Int32? CityId)
        {
            return _iUserRepository.GetUserByCityId(CityId);
        }

        public virtual List<User> GetUserByStateId(System.Int32? StateId)
        {
            return _iUserRepository.GetUserByStateId(StateId);
        }

        public virtual List<User> GetUserBySalaryTypeId(System.Int32? SalaryTypeId)
        {
            return _iUserRepository.GetUserBySalaryTypeId(SalaryTypeId);
        }

        public User GetUser(System.Int32 Id)
        {
            return _iUserRepository.GetUser(Id);
        }

        public User GetUserWithDepartment(System.Int32 Id)
        {
            IUserDepartmentService objUserDepartmentService = IoC.Resolve<IUserDepartmentService>("UserDepartmentService");
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
            IUserShiftService objUserShiftService = IoC.Resolve<IUserShiftService>("UserShiftService");
            IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
            IUserContactService objUserContactService = IoC.Resolve<IUserContactService>("UserContactService");
            User User = _iUserRepository.GetUser(Id);

            if (User != null)
            {
                List<UserDepartment> UserDepartmentList = objUserDepartmentService.GetAllUserDepartment().Where(x => x.UserId == User.Id && x.RetiredDate == null).ToList();
                if (UserDepartmentList.Count > 0)
                {
                    User.UserDepartment = UserDepartmentList.OrderByDescending(x => x.EffectiveDate).FirstOrDefault();
                    User.Department = objDepartmentService.GetAllDepartment().Where(x => x.Id == User.UserDepartment.DepartmentId).FirstOrDefault();
                }

                List<UserShift> UserShiftList = objUserShiftService.GetAllUserShift();
                if (UserShiftList != null)
                {
                    UserShiftList = UserShiftList.Where(x => x.UserId == User.Id && DateTime.Now >= x.EffectiveDate && DateTime.Now <= (x.RetiredDate == null ? DateTime.Now : x.RetiredDate)).ToList();
                    if (UserShiftList.Count > 0)
                    {
                        User.UserShift = UserShiftList.OrderByDescending(x => x.EffectiveDate).FirstOrDefault();
                        User.Shift = objShiftService.GetAllShift().Where(x => x.Id == User.UserShift.ShiftId).FirstOrDefault();
                    }
                }

                List<UserContact> _userContactList = objUserContactService.GetUserContactByUserId(User.Id);
                if (_userContactList != null && _userContactList.Count > 0)
                {
                    User.UserContactEmail = _userContactList.Where(x => x.IsActive == true && x.ContactTypeId == (int)HRM.Core.Enum.ContactType.EmailAddress).FirstOrDefault();
                    User.UserContactMobile = _userContactList.Where(x => x.IsActive == true && x.ContactTypeId == (int)HRM.Core.Enum.ContactType.MobileNumber).FirstOrDefault();
                    User.UserContactAlternateMobile = _userContactList.Where(x => x.IsActive == true && x.ContactTypeId == (int)HRM.Core.Enum.ContactType.AlternateMobileNumber).FirstOrDefault();
                }
            }

            return User;
        }

        public User UpdateUser(User entity)
        {
            return _iUserRepository.UpdateUser(entity);
        }

        public User UpdateUserByKeyValue(Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
        {
            return _iUserRepository.UpdateUserByKeyValue(UpdateKeyValue, Id);
        }

        public bool DeleteUser(System.Int32 Id)
        {
            return _iUserRepository.DeleteUser(Id);
        }

        public List<User> GetAllUser()
        {
            return _iUserRepository.GetAllUser();
        }

        public List<User> GetAllUserWithDepartment()
        {
            IUserDepartmentService objUserDepartmentService = IoC.Resolve<IUserDepartmentService>("UserDepartmentService");
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
            IUserContactService objUserContactService = IoC.Resolve<IUserContactService>("UserContactService");
            IUserShiftService objUserShiftService = IoC.Resolve<IUserShiftService>("UserShiftService");
            IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
            List<UserDepartment> UserDepartmentList = objUserDepartmentService.GetAllUserDepartment();
            List<Department> DepartmentList = objDepartmentService.GetAllDepartment();
            List<UserShift> UserShiftList = objUserShiftService.GetAllUserShift();
            List<Shift> ShiftList = objShiftService.GetAllShift();


            List<User> UserList = _iUserRepository.GetAllUser();
            if (UserList != null && UserList.Count > 0)
            {
                foreach (var user in UserList)
                {
                    if (UserDepartmentList != null && UserDepartmentList.Where(x => x.UserId == user.Id && x.RetiredDate == null) != null)
                    {
                        user.UserDepartment = UserDepartmentList.Where(x => x.UserId == user.Id && x.RetiredDate == null).OrderByDescending(x => x.EffectiveDate).FirstOrDefault();
                        user.Department = DepartmentList.Where(x => x.Id == user.UserDepartment.DepartmentId).FirstOrDefault();
                    }

                    if (UserShiftList != null && UserShiftList.Where(x => x.UserId == user.Id && DateTime.Now >= x.EffectiveDate && DateTime.Now <= (x.RetiredDate == null ? DateTime.Now : x.RetiredDate)) != null && UserShiftList.Where(x => x.UserId == user.Id && DateTime.Now >= x.EffectiveDate && DateTime.Now <= (x.RetiredDate == null ? DateTime.Now : x.RetiredDate)).ToList().Count > 0)
                    {
                        user.UserShift = UserShiftList.Where(x => x.UserId == user.Id && DateTime.Now >= x.EffectiveDate && DateTime.Now <= (x.RetiredDate == null ? DateTime.Now : x.RetiredDate)).OrderByDescending(x => x.EffectiveDate).FirstOrDefault();
                        user.Shift = ShiftList.Where(x => x.Id == user.UserShift.ShiftId).FirstOrDefault();
                    }

                    List<UserContact> _userContactList = objUserContactService.GetUserContactByUserId(user.Id);
                    if (_userContactList != null && _userContactList.Count > 0)
                    {
                        user.UserContactEmail = _userContactList.Where(x => x.IsActive == true && x.ContactTypeId == (int)HRM.Core.Enum.ContactType.EmailAddress).FirstOrDefault();
                        user.UserContactMobile = _userContactList.Where(x => x.IsActive == true && x.ContactTypeId == (int)HRM.Core.Enum.ContactType.MobileNumber).FirstOrDefault();
                        user.UserContactAlternateMobile = _userContactList.Where(x => x.IsActive == true && x.ContactTypeId == (int)HRM.Core.Enum.ContactType.AlternateMobileNumber).FirstOrDefault();
                    }
                }
            }
            return UserList;
        }

        public User InsertUser(User entity)
        {
            return _iUserRepository.InsertUser(entity);
        }


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id = 0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id, out id))
            {
                User user = _iUserRepository.GetUser(id);
                if (user != null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(user);
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
            List<User> userlist = _iUserRepository.GetAllUser();
            if (userlist != null && userlist.Count > 0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(userlist);
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
                User user = new User();
                PostOutput output = new PostOutput();
                user.CopyFrom(Input);
                user = _iUserRepository.InsertUser(user);
                output.CopyFrom(user);
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
                User userinput = new User();
                User useroutput = new User();
                PutOutput output = new PutOutput();
                userinput.CopyFrom(Input);
                User user = _iUserRepository.GetUser(userinput.Id);
                if (user != null)
                {
                    useroutput = _iUserRepository.UpdateUser(userinput);
                    if (useroutput != null)
                    {
                        output.CopyFrom(useroutput);
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
                bool IsDeleted = _iUserRepository.DeleteUser(id);
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

        public User GetUser(string LoginId)
        {
            return _iUserRepository.GetUser(LoginId);
        }

        public User GetUserWithDepartment(string LoginId)
        {
            IUserDepartmentService objUserDepartmentService = IoC.Resolve<IUserDepartmentService>("UserDepartmentService");
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
            User User = _iUserRepository.GetUser(LoginId);

            if (User != null)
            {
                List<UserDepartment> UserDepartmentList = objUserDepartmentService.GetAllUserDepartment();
                if (UserDepartmentList != null && UserDepartmentList.Count > 0)
                {
                    User.UserDepartment = UserDepartmentList.Where(x => x.UserId == User.Id && x.RetiredDate == null).OrderByDescending(x => x.EffectiveDate).FirstOrDefault();
                    User.Department = objDepartmentService.GetAllDepartment().Where(x => x.Id == User.UserDepartment.DepartmentId).FirstOrDefault();
                }

            }
            return User;
        }

        public List<User> GetAllUserWithDepartment(int BranchID)
        {
            IUserDepartmentService objUserDepartmentService = IoC.Resolve<IUserDepartmentService>("UserDepartmentService");
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
            IUserContactService objUserContactService = IoC.Resolve<IUserContactService>("UserContactService");
            IUserShiftService objUserShiftService = IoC.Resolve<IUserShiftService>("UserShiftService");
            IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");

            IBranchDepartmentService iBranchDepartmentService = IoC.Resolve<IBranchDepartmentService>("BranchDepartmentService");
            List<BranchDepartment> _branchDeptList = iBranchDepartmentService.GetBranchDepartmentByBranchId(BranchID);

            List<UserDepartment> UserDepartmentList = objUserDepartmentService.GetAllUserDepartment();
            List<Department> DepartmentList = objDepartmentService.GetAllDepartment();
            List<UserShift> UserShiftList = objUserShiftService.GetAllUserShift();
            List<Shift> ShiftList = objShiftService.GetAllShift();


            List<User> UserList = _iUserRepository.GetAllUser();
            List<User> tempUserList = new List<User>();
            if (UserList != null && UserList.Count > 0)
            {
                UserList = UserList.Where(x => x.IsActive == true).ToList();
                tempUserList.AddRange(UserList);
                List<UserDepartment> _userDeptList = new List<UserDepartment>();
                foreach (var user in tempUserList)
                {
                    _userDeptList = UserDepartmentList.Where(x => x.UserId == user.Id && x.RetiredDate == null).ToList();
                    if (_branchDeptList != null && _branchDeptList.Where(x => x.DepartmentId == _userDeptList.FirstOrDefault().DepartmentId).Any())
                    {
                        if (UserDepartmentList != null && _userDeptList != null)
                        {
                            user.UserDepartment = UserDepartmentList.Where(x => x.UserId == user.Id && x.RetiredDate == null).OrderByDescending(x => x.EffectiveDate).FirstOrDefault();
                            user.Department = DepartmentList.Where(x => x.Id == user.UserDepartment.DepartmentId).FirstOrDefault();
                        }

                        if (UserShiftList != null && UserShiftList.Where(x => x.UserId == user.Id && DateTime.Now >= x.EffectiveDate && DateTime.Now <= (x.RetiredDate == null ? DateTime.Now : x.RetiredDate)) != null && UserShiftList.Where(x => x.UserId == user.Id && DateTime.Now >= x.EffectiveDate && DateTime.Now <= (x.RetiredDate == null ? DateTime.Now : x.RetiredDate)).ToList().Count > 0)
                        {
                            user.UserShift = UserShiftList.Where(x => x.UserId == user.Id && DateTime.Now >= x.EffectiveDate && DateTime.Now <= (x.RetiredDate == null ? DateTime.Now : x.RetiredDate)).OrderByDescending(x => x.EffectiveDate).FirstOrDefault();
                            user.Shift = ShiftList.Where(x => x.Id == user.UserShift.ShiftId).FirstOrDefault();
                        }

                        List<UserContact> _userContactList = objUserContactService.GetUserContactByUserId(user.Id);
                        if (_userContactList != null && _userContactList.Count > 0)
                        {
                            user.UserContactEmail = _userContactList.Where(x => x.IsActive == true && x.ContactTypeId == (int)HRM.Core.Enum.ContactType.EmailAddress).FirstOrDefault();
                            user.UserContactMobile = _userContactList.Where(x => x.IsActive == true && x.ContactTypeId == (int)HRM.Core.Enum.ContactType.MobileNumber).FirstOrDefault();
                            user.UserContactAlternateMobile = _userContactList.Where(x => x.IsActive == true && x.ContactTypeId == (int)HRM.Core.Enum.ContactType.AlternateMobileNumber).FirstOrDefault();
                        }
                    }
                    else
                    {
                        UserList.Remove(user);
                    }
                }
            }
            return UserList;
        }
    }


}
