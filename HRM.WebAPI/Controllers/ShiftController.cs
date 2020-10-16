using HRM.Core;
using HRM.Core.Entities;
using HRM.Core.Helper;
using HRM.Core.IService;
using HRM.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.WebAPI.Controllers
{
    public class ShiftController : Controller
    {
        //
        // GET: /Shift/

        [HttpGet]
        [Authenticate]
        public ActionResult Index(string Id)
        {
            VMShiftModel model = new VMShiftModel();
            var Sunday = new DaysOfWeek { Id = 7, Name = "Sunday" };
            var Monday = new DaysOfWeek { Id = 1, Name = "Monday" };
            var Tuesday = new DaysOfWeek { Id = 2, Name = "Tuesday" };
            var Wednesday = new DaysOfWeek { Id = 3, Name = "Wednesday" };
            var Thursday = new DaysOfWeek { Id = 4, Name = "Thursday" };
            var Friday = new DaysOfWeek { Id = 5, Name = "Friday" };
            var Saturday = new DaysOfWeek { Id = 6, Name = "Saturday" };

            model.DaysOfWeekList = new List<DaysOfWeek>();
            model.DaysOfWeekList.Add(Sunday);
            model.DaysOfWeekList.Add(Monday);
            model.DaysOfWeekList.Add(Tuesday);
            model.DaysOfWeekList.Add(Wednesday);
            model.DaysOfWeekList.Add(Thursday);
            model.DaysOfWeekList.Add(Friday);
            model.DaysOfWeekList.Add(Saturday);

            ShiftPrerequisiteData();

            try
            {
                if (Id != null && Id != "")
                {
                    IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
                    IShiftOffDayService objShiftOffDayService = IoC.Resolve<IShiftOffDayService>("ShiftOffDayService");
                    model.Shift = objShiftService.GetShift(int.Parse(Id));
                    if (model.Shift != null)
                    {
                        string[] starttime = model.Shift.StartHour.Split(':').ToArray();
                        string[] endtime = model.Shift.EndHour.Split(':').ToArray();
                        //string[] breaktime = model.Shift.BreakHour.Split(':').ToArray();
                        if (starttime.Length > 0)
                        {
                            model.Shift.StartHourOnly = starttime[0];
                            model.Shift.StartMinuteOnly = starttime[1];
                        }
                        if (endtime.Length > 0)
                        {
                            model.Shift.EndHourOnly = endtime[0];
                            model.Shift.EndMinuteOnly = endtime[1];
                        }
                        //if (breaktime.Length > 0)
                        //{
                        //    model.Shift.BreakHourOnly = breaktime[0];
                        //    model.Shift.BreakMinuteOnly = breaktime[1];
                        //}
                        List<ShiftOffDay> _ShiftOffDayList = objShiftOffDayService.GetShiftOffDayByShiftId(model.Shift.Id);
                        if (_ShiftOffDayList != null && _ShiftOffDayList.Count > 0)
                        {
                            _ShiftOffDayList = _ShiftOffDayList.Where(x => x.RetiredDate == null).ToList();
                            model.ShiftOffDayList = _ShiftOffDayList;
                            foreach (var day in model.DaysOfWeekList)
                            {
                                foreach (var offday in model.ShiftOffDayList)
                                {
                                    if (offday.OffDayOfWeek == day.Id)
                                        day.IsSelected = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult Index(VMShiftModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Shift.Id != 0)
                    {
                        #region Shift Updation
                        IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");

                        model.Shift.UpdateDate = DateTime.Now;
                        model.Shift.UpdateBy = AuthBase.UserId;
                        model.Shift.UserIp = Request.UserHostAddress;
                        model.Shift.BreakHour = Convert.ToDecimal(model.Shift.BreakHourOnly) + Convert.ToDecimal(model.Shift.BreakMinuteOnly);

                        Shift _shift = objShiftService.UpdateShift(model.Shift);

                        #endregion

                        #region Shift Off Day Update and Entry
                        IShiftOffDayService objShiftOffDayService = IoC.Resolve<IShiftOffDayService>("ShiftOffDayService");
                        List<ShiftOffDay> _ShiftOffDayList = objShiftOffDayService.GetShiftOffDayByShiftId(model.Shift.Id);
                        if (_ShiftOffDayList != null && _ShiftOffDayList.Count() > 0)
                        {
                            _ShiftOffDayList = _ShiftOffDayList.Where(x => x.RetiredDate == null).ToList();
                            model.ShiftOffDayList = _ShiftOffDayList;
                            foreach (var day in model.DaysOfWeekList)
                            {
                                bool previousentry = false;
                                foreach (var offday in model.ShiftOffDayList)
                                {
                                    if (offday.OffDayOfWeek == day.Id && day.IsSelected == false)
                                    {
                                        Dictionary<string, string> dicShiftOffDay = new Dictionary<string, string>();
                                        dicShiftOffDay.Add("RetiredDate", string.Format("{0:dd-MMM-yyyy}", DateTime.Now.AddDays(-1)) + " 23:59:59");
                                        dicShiftOffDay.Add("UpdateDate", DateTime.Now.ToString());
                                        dicShiftOffDay.Add("UpdateBy", AuthBase.UserId.ToString());
                                        dicShiftOffDay.Add("UserIp", Request.UserHostAddress);
                                        objShiftOffDayService.UpdateShiftOffDayByKeyValue(dicShiftOffDay, offday.Id);
                                    }
                                    else if (offday.OffDayOfWeek == day.Id && day.IsSelected == true)
                                    {
                                        previousentry = true;
                                    }
                                }
                                if (previousentry == false && day.IsSelected == true)
                                {
                                    ShiftOffDay ShiftOffDay = new ShiftOffDay();
                                    ShiftOffDay.ShiftId = _shift.Id;
                                    ShiftOffDay.OffDayOfWeek = day.Id;
                                    ShiftOffDay.EffectiveDate = Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", DateTime.Now));
                                    ShiftOffDay.CreationDate = DateTime.Now;
                                    ShiftOffDay.UpdateBy = AuthBase.UserId;
                                    ShiftOffDay.UserIp = Request.UserHostAddress;

                                    ShiftOffDay= objShiftOffDayService.InsertShiftOffDay(ShiftOffDay);
                                }
                            }
                        }
                        else
                        {
                            foreach (var day in model.DaysOfWeekList)
                            {
                                if (day.IsSelected)
                                {
                                    ShiftOffDay ShiftOffDay = new ShiftOffDay();
                                    ShiftOffDay.ShiftId = _shift.Id;
                                    ShiftOffDay.OffDayOfWeek = day.Id;
                                    ShiftOffDay.EffectiveDate = Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", DateTime.Now));
                                    ShiftOffDay.CreationDate = DateTime.Now;
                                    ShiftOffDay.UpdateBy = AuthBase.UserId;
                                    ShiftOffDay.UserIp = Request.UserHostAddress;

                                    objShiftOffDayService.InsertShiftOffDay(ShiftOffDay);
                                }
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        #region Shift Entry
                        IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");

                        model.Shift.StartHour = model.Shift.StartHourOnly + ":" + model.Shift.StartMinuteOnly;
                        model.Shift.EndHour = model.Shift.EndHourOnly + ":" + model.Shift.EndMinuteOnly;
                        model.Shift.BreakHour = Convert.ToDecimal(model.Shift.BreakHourOnly)  + Convert.ToDecimal(model.Shift.BreakMinuteOnly);
                        model.Shift.IsActive = true;
                        model.Shift.CreationDate = DateTime.Now;
                        model.Shift.UpdateBy = AuthBase.UserId;
                        model.Shift.UserIp = Request.UserHostAddress;

                        Shift _shift = objShiftService.InsertShift(model.Shift);

                        #endregion

                        #region Shift Off Day Entry
                        IShiftOffDayService objShiftOffDayService = IoC.Resolve<IShiftOffDayService>("ShiftOffDayService");
                        foreach (var day in model.DaysOfWeekList)
                        {
                            if (day.IsSelected)
                            {
                                ShiftOffDay ShiftOffDay = new ShiftOffDay();
                                ShiftOffDay.ShiftId = _shift.Id;
                                ShiftOffDay.OffDayOfWeek = day.Id;
                                ShiftOffDay.EffectiveDate = Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", DateTime.Now));
                                ShiftOffDay.CreationDate = DateTime.Now;
                                ShiftOffDay.UpdateBy = AuthBase.UserId;

                                ShiftOffDay.UserIp = Request.UserHostAddress;

                                objShiftOffDayService.InsertShiftOffDay(ShiftOffDay);
                            }
                        }
                        #endregion

                        #region BranchShift

                        IBranchShiftService iBranchShiftService = IoC.Resolve<IBranchShiftService>("BranchShiftService");
                        BranchShift _branchShift = new BranchShift()
                        {
                            BranchId = AuthBase.BranchId,
                            ShiftId = _shift.Id,
                            IsActive = true,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            UpdatedBy = AuthBase.UserId,
                            UserIp = Request.UserHostAddress
                        };

                        iBranchShiftService.InsertBranchShift(_branchShift);

                        #endregion
                    }
                    return RedirectToAction("ShiftList", "Shift");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ModelState.AddModelError("", "Unable to process your request. Please contact Administration");

            var Sunday = new DaysOfWeek { Id = 7, Name = "Sunday" };
            var Monday = new DaysOfWeek { Id = 1, Name = "Monday" };
            var Tuesday = new DaysOfWeek { Id = 2, Name = "Tuesday" };
            var Wednesday = new DaysOfWeek { Id = 3, Name = "Wednesday" };
            var Thursday = new DaysOfWeek { Id = 4, Name = "Thursday" };
            var Friday = new DaysOfWeek { Id = 5, Name = "Friday" };
            var Saturday = new DaysOfWeek { Id = 6, Name = "Saturday" };

            model.DaysOfWeekList = new List<DaysOfWeek>();
            model.DaysOfWeekList.Add(Sunday);
            model.DaysOfWeekList.Add(Monday);
            model.DaysOfWeekList.Add(Tuesday);
            model.DaysOfWeekList.Add(Wednesday);
            model.DaysOfWeekList.Add(Thursday);
            model.DaysOfWeekList.Add(Friday);
            model.DaysOfWeekList.Add(Saturday);
            ShiftPrerequisiteData();
            return View();
        }

        [HttpGet]
        [Authenticate]
        public ActionResult ShiftList()
        {
            VMShiftModel VMShiftModel = new VMShiftModel();
            try
            {
                IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
                List<Shift> ShiftList = objShiftService.GetAllShiftWithOffDays(AuthBase.BranchId);
                if (ShiftList != null && ShiftList.Count > 0)
                {
                    VMShiftModel.ShiftList = ShiftList;
                    foreach (var shift in VMShiftModel.ShiftList)
                    {
                        if (shift.ShiftOffDayList == null || shift.ShiftOffDayList.Count <= 0)
                        {
                            shift.OffDays = "";
                        }
                        else
                        {
                            foreach (var offDay in shift.ShiftOffDayList)
                            {
                                if (offDay.OffDayOfWeek == 7)
                                    offDay.OffDayOfWeek = 0;
                                shift.OffDays += Enum.GetName(typeof(System.DayOfWeek), offDay.OffDayOfWeek ) + ", ";
                            }
                            if (shift.OffDays != null)
                                shift.OffDays = shift.OffDays.Substring(0, shift.OffDays.LastIndexOf(", "));
                        }
                    }
                }
                else
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(VMShiftModel);
        }

        [HttpGet]
        [Authenticate]
        public ActionResult Variable(string Id)
        {
            VariablePrerequisiteData();
            VMShiftModel model = new VMShiftModel();
            try
            {
                IAttendanceVariableService ObjAttendanceVariableService = IoC.Resolve<IAttendanceVariableService>("AttendanceVariableService");
                IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
                model.AttendanceVariableList = ObjAttendanceVariableService.GetAllAttendanceVariable();

                if (Id != null && Id != "")
                {
                    model.ShiftId = int.Parse(Id);
                    IAttendancePolicyService ObjAttendancePolicyService = IoC.Resolve<IAttendancePolicyService>("AttendancePolicyService");
                    List<AttendancePolicy> _AttendancePolicyList = ObjAttendancePolicyService.GetAttendancePolicyByShiftId(model.ShiftId);
                    if (_AttendancePolicyList != null && _AttendancePolicyList.Count > 0)
                    {
                        _AttendancePolicyList = _AttendancePolicyList.Where(x => x.RetiredDate == null).ToList();
                        foreach (var policy in _AttendancePolicyList)
                        {
                            if (model.AttendanceVariableList != null && model.AttendanceVariableList.Count > 0)
                            {
                                foreach (var variable in model.AttendanceVariableList)
                                {
                                    if (variable.Id == policy.AttendanceVariableId)
                                    {
                                        variable.IsSelected = true;
                                        variable.Hours = Convert.ToDecimal(policy.Hours);
                                        variable.Reason = policy.Description;
                                    }
                                }
                            }
                        }
                    }
                    //else
                    //    ModelState.AddModelError("", "No Records Found");
                }
                //else
                //    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult Variable(VMShiftModel model)
        {
            if (ModelState.IsValid)
            {                try
                {
                    IAttendancePolicyService objAttendancePolicyService = IoC.Resolve<IAttendancePolicyService>("AttendancePolicyService");
                    List<AttendancePolicy> _AttendancePolicyList = objAttendancePolicyService.GetAttendancePolicyByShiftId(model.ShiftId);
                    if (_AttendancePolicyList != null && _AttendancePolicyList.Count > 0 && model.ShiftId != 0)
                    {
                        _AttendancePolicyList = _AttendancePolicyList.Where(x => x.RetiredDate == null).ToList();
                        #region Shift Variable Update and Entry
                        if (_AttendancePolicyList.Count > 0)
                        {
                            foreach (var variable in model.AttendanceVariableList)
                            {
                                bool previousentry = false;
                                foreach (var policy in _AttendancePolicyList)
                                {
                                    if (policy.AttendanceVariableId == variable.Id && variable.IsSelected == false)
                                    {
                                        Dictionary<string, string> dicAttendancePolicy = new Dictionary<string, string>();
                                        dicAttendancePolicy.Add("RetiredDate", string.Format("{0:dd-MMM-yyyy}", DateTime.Now.AddDays(-1)) + " 23:59:59");
                                        dicAttendancePolicy.Add("UpdateDate", DateTime.Now.ToString());
                                        dicAttendancePolicy.Add("UpdateBy", AuthBase.UserId.ToString());
                                        dicAttendancePolicy.Add("UserIp", Request.UserHostAddress);
                                        objAttendancePolicyService.UpdateAttendancePolicyByKeyValue(dicAttendancePolicy, policy.Id);
                                    }
                                    else if (policy.AttendanceVariableId == variable.Id && variable.IsSelected == true)
                                    {
                                        if (policy.Hours != variable.Hours)
                                        {
                                            Dictionary<string, string> dicAttendancePolicy = new Dictionary<string, string>();
                                            dicAttendancePolicy.Add("RetiredDate", string.Format("{0:dd-MMM-yyyy}", DateTime.Now.AddDays(-1)) + " 23:59:59");
                                            dicAttendancePolicy.Add("UpdateDate", DateTime.Now.ToString());
                                            dicAttendancePolicy.Add("UpdateBy", AuthBase.UserId.ToString());
                                            dicAttendancePolicy.Add("UserIp", Request.UserHostAddress);
                                            objAttendancePolicyService.UpdateAttendancePolicyByKeyValue(dicAttendancePolicy, policy.Id);
                                        }
                                        else if (policy.Description != (variable.Reason == null ? "" : variable.Reason))
                                        {
                                            Dictionary<string, string> dicAttendancePolicy = new Dictionary<string, string>();
                                            dicAttendancePolicy.Add("Description", variable.Reason == null ? "" : variable.Reason);
                                            dicAttendancePolicy.Add("UpdateDate", DateTime.Now.ToString());
                                            dicAttendancePolicy.Add("UpdateBy", AuthBase.UserId.ToString());
                                            dicAttendancePolicy.Add("UserIp", Request.UserHostAddress);
                                            objAttendancePolicyService.UpdateAttendancePolicyByKeyValue(dicAttendancePolicy, policy.Id);

                                            previousentry = true;
                                        }
                                        else
                                            previousentry = true;
                                    }
                                }
                                if (previousentry == false && variable.IsSelected == true)
                                {
                                    AttendancePolicy AttendancePolicy = new AttendancePolicy();
                                    AttendancePolicy.ShiftId = model.ShiftId;
                                    AttendancePolicy.AttendanceVariableId = variable.Id;
                                    AttendancePolicy.Hours = Convert.ToDecimal(variable.Hours);
                                    AttendancePolicy.Description = variable.Reason;
                                    AttendancePolicy.EffectiveDate = Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", DateTime.Now));
                                    AttendancePolicy.CreationDate = DateTime.Now;
                                    AttendancePolicy.UpdateBy = AuthBase.UserId;
                                    AttendancePolicy.UserIp = Request.UserHostAddress;
                                    objAttendancePolicyService.InsertAttendancePolicy(AttendancePolicy);
                                }
                            }
                        }
                        else
                        {
                            foreach (var variable in model.AttendanceVariableList)
                            {
                                if (variable.IsSelected)
                                {
                                    AttendancePolicy AttendancePolicy = new AttendancePolicy();
                                    AttendancePolicy.ShiftId = model.ShiftId;
                                    AttendancePolicy.AttendanceVariableId = variable.Id;
                                    AttendancePolicy.Hours = Convert.ToDecimal(variable.Hours);
                                    AttendancePolicy.Description = variable.Reason;
                                    AttendancePolicy.EffectiveDate = Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", DateTime.Now));
                                    AttendancePolicy.CreationDate = DateTime.Now;
                                    AttendancePolicy.UpdateBy = AuthBase.UserId;
                                    AttendancePolicy.UserIp = Request.UserHostAddress;

                                    objAttendancePolicyService.InsertAttendancePolicy(AttendancePolicy);
                                }
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        #region Shift Variable Entry
                        foreach (var variable in model.AttendanceVariableList)
                        {
                            if (variable.IsSelected)
                            {
                                AttendancePolicy AttendancePolicy = new AttendancePolicy();
                                AttendancePolicy.ShiftId = model.ShiftId;
                                AttendancePolicy.AttendanceVariableId = variable.Id;
                                AttendancePolicy.Hours = Convert.ToDecimal(variable.Hours);
                                AttendancePolicy.Description = variable.Reason;
                                AttendancePolicy.EffectiveDate = Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", DateTime.Now));
                                AttendancePolicy.CreationDate = DateTime.Now;
                                AttendancePolicy.UpdateBy = AuthBase.UserId;
                                AttendancePolicy.UserIp = Request.UserHostAddress;

                                objAttendancePolicyService.InsertAttendancePolicy(AttendancePolicy);
                            }
                        }
                        #endregion
                    }
                    return RedirectToAction("VariableList", "Shift");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            VariablePrerequisiteData();
            ModelState.AddModelError("", "Unable to process your request. Please contact Administration");
            return View();
        }

        [HttpGet]
        [Authenticate]
        public ActionResult VariableList()
        {
            VMShiftModel VMShiftModel = new VMShiftModel();
            try
            {
                IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
                List<Shift> ShiftList = objShiftService.GetAllShiftWithAttendancePolicy(AuthBase.BranchId);
                if (ShiftList != null && ShiftList.Count > 0)
                {
                    VMShiftModel.ShiftList = ShiftList;
                    foreach (var shift in VMShiftModel.ShiftList)
                    {
                        if (shift.AttendancePolicyList != null)
                        {
                            foreach (var policy in shift.AttendancePolicyList)
                            {
                                shift.Policies += Enum.GetName(typeof(HRM.Core.Enum.AttendanceVariable), policy.AttendanceVariableId) + ", ";
                            }
                            if (shift.Policies != null)
                                shift.Policies = shift.Policies.Substring(0, shift.Policies.LastIndexOf(", "));
                        }
                    }
                }
                else
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(VMShiftModel);
        }

        [HttpGet]
        [Authenticate]
        public ActionResult Assign(string Id)
        {
            VMUserModel VMUserModelDepartment = new VMUserModel();
            AssignShiftPrerequisiteData();
            try
            {
                if (Id != null && Id != "")
                {
                    IUserService objUserService = IoC.Resolve<IUserService>("UserService");

                    VMUserModel VMUserModel = new VMUserModel();
                    VMUserModel.User = objUserService.GetUserWithDepartment(int.Parse(Id));

                    VMUserModelDepartment.UserList = new List<User>();
                    VMUserModelDepartment.UserList.Add(VMUserModel.User);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(VMUserModelDepartment);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult Assign(VMUserModel model)
        {
            VMUserModel VMUserModel = new VMUserModel();
            try
            {
                IUserService objUserService = IoC.Resolve<IUserService>("UserService");
                IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
                if (model.DepartmentID != null)
                    VMUserModel.UserList = objUserService.GetAllUserWithDepartment(AuthBase.BranchId).Where(x => x.UserDepartment.DepartmentId == model.DepartmentID).ToList();
                else
                    VMUserModel.UserList = objUserService.GetAllUserWithDepartment(AuthBase.BranchId);

                if (model.UserName != null && model.UserName != "" && VMUserModel.UserList != null && VMUserModel.UserList.Count > 0)
                    VMUserModel.UserList = VMUserModel.UserList.Where(x => x.FirstName.ToLower().Contains(model.UserName.ToLower())).ToList();


                AssignShiftPrerequisiteData();
                if (VMUserModel.UserList == null || VMUserModel.UserList.Count == 0)
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(VMUserModel);
        }

        [HttpGet]
        [Authenticate]
        public ActionResult AssignList()
        {
            AssignShiftPrerequisiteData();
            VMUserModel model = new VMUserModel();
            try
            {
                IUserService objUserService = IoC.Resolve<IUserService>("UserService");
                IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");

                if (model.DepartmentID != null)
                    model.UserList = objUserService.GetAllUserWithDepartment(AuthBase.BranchId).Where(x => x.UserDepartment.DepartmentId == model.DepartmentID).ToList();
                else
                    model.UserList = objUserService.GetAllUserWithDepartment(AuthBase.BranchId);

                if (model.ShiftId != null && model.UserList.Count > 0 && model.UserList.Where(x => x.Shift.Id == model.ShiftId).ToList().Count > 0)
                    model.UserList = model.UserList.Where(x => x.Shift.Id == model.ShiftId).OrderBy(x => x.Shift).ToList();


                if (model.UserList.Count == 0)
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
            }
            return View(model);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult AssignList(VMUserModel model)
        {
            try
            {
                IUserService objUserService = IoC.Resolve<IUserService>("UserService");
                IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");

                if (model.DepartmentID != null)
                    model.UserList = objUserService.GetAllUserWithDepartment(AuthBase.BranchId).Where(x => x.UserDepartment.DepartmentId == model.DepartmentID).ToList();
                else
                    model.UserList = objUserService.GetAllUserWithDepartment(AuthBase.BranchId);

                if (model.ShiftId != null && model.UserList != null && model.UserList.Count > 0)
                    model.UserList = model.UserList.Where(x => x.Shift != null && x.Shift.Id == model.ShiftId).OrderBy(x => x.Shift).ToList();

                if (model.UserName != null && model.UserName != "" && model.UserList != null && model.UserList.Count > 0)
                    model.UserList = model.UserList.Where(x => x.FirstName.ToLower().Contains(model.UserName.ToLower())).ToList();

                if (model.UserList == null || model.UserList.Count == 0)
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            AssignShiftPrerequisiteData();
            return View(model);
        }

        [HttpGet]
        [Authenticate]
        public ActionResult AssignHistory(string Id)
        {
            VMShiftModel model = new VMShiftModel();
            try
            {
                if (Id != null && Id != "")
                {
                    IUserShiftService ObjUserShiftService = IoC.Resolve<IUserShiftService>("UserShiftService");
                    IShiftService ObjShiftService = IoC.Resolve<IShiftService>("ShiftService");
                    model.UserShiftList = ObjUserShiftService.GetUserShiftByUserId(int.Parse(Id));
                    if (model.UserShiftList != null && model.UserShiftList.Count > 0)
                    {
                        model.UserShiftList = model.UserShiftList.OrderBy(x => x.EffectiveDate).ToList();
                        foreach (var shift in model.UserShiftList)
                        {
                            shift.Shift = ObjShiftService.GetShift((int)shift.ShiftId);
                            //shift.Shift = ObjShiftService.GetShiftWithOffDaysHistory((int)shift.ShiftId, shift.RetiredDate == null ? DateTime.Now : (DateTime)shift.RetiredDate);
                            //if (shift.Shift.ShiftOffDayList != null)
                            //{
                            //    foreach (var offDay in shift.Shift.ShiftOffDayList)
                            //    {
                            //        shift.Shift.OffDays += Enum.GetName(typeof(System.DayOfWeek), offDay.OffDayOfWeek - 1) + ", ";
                            //    }
                            //    if (shift.Shift.OffDays != null)
                            //        shift.Shift.OffDays = shift.Shift.OffDays.Substring(0, shift.Shift.OffDays.LastIndexOf(", "));
                            //}
                        }
                    }
                    else
                        ModelState.AddModelError("", "No Records Found");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        [HttpGet]
        [Authenticate]
        public ActionResult OffDayHistory(string Id)
        {
            VMShiftModel model = new VMShiftModel();
            try
            {
                if (Id != null && Id != "")
                {
                    IShiftService ObjShiftService = IoC.Resolve<IShiftService>("ShiftService");
                    model.Shift = ObjShiftService.GetShiftWithOffDaysHistory(int.Parse(Id));
                    if (model.Shift != null && model.Shift.ShiftOffDayList != null && model.Shift.ShiftOffDayList.Count > 0)
                    {
                        //foreach (var offDay in model.Shift.ShiftOffDayList)
                        //{
                        //    model.Shift.OffDays += Enum.GetName(typeof(System.DayOfWeek), offDay.OffDayOfWeek - 1) + ", ";
                        //}
                        //if (model.Shift.OffDays != null)
                        //    model.Shift.OffDays = model.Shift.OffDays.Substring(0, model.Shift.OffDays.LastIndexOf(", "));
                    }
                    else
                        ModelState.AddModelError("", "No Records Found");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        [Authenticate]
        public JsonResult SaveUserShift(int ShiftId, string Users, string EffectiveDate)
        {
            try
            {
                var userids = Users.Split(',');
                foreach (var userid in userids)
                {
                    string expectedRetiredDate = string.Empty;

                    IUserShiftService ObjUserShiftService = IoC.Resolve<IUserShiftService>("UserShiftService");
                    List<UserShift> UserShiftList = ObjUserShiftService.GetUserShiftByUserId(int.Parse(userid));
                    
                    if (UserShiftList != null && UserShiftList.Count > 0)
                    {
                        if (UserShiftList.Where(x => x.EffectiveDate > Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", EffectiveDate))).Any())
                        {
                            UserShift _recentShift = ObjUserShiftService.GetUserShiftByUserId(int.Parse(userid)).Where(x => x.EffectiveDate > Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", EffectiveDate))).OrderBy(x => x.EffectiveDate).FirstOrDefault();
                            expectedRetiredDate = Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", _recentShift.EffectiveDate) + " 23:59:59").AddDays(-1).ToString();
                        }

                        UserShiftList = ObjUserShiftService.GetUserShiftByUserId(int.Parse(userid)).Where(x => x.EffectiveDate < Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", EffectiveDate)) && x.RetiredDate == null).ToList();
                        //UserShiftList = ObjUserShiftService.GetUserShiftByUserId(int.Parse(userid)).Where(x => x.RetiredDate == null).ToList();

                        foreach (var usershift in UserShiftList)
                        {
                            usershift.RetiredDate = Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", EffectiveDate) + " 23:59:59").AddDays(-1);
                            usershift.UpdateDate = DateTime.Now;
                            usershift.UpdateBy = AuthBase.UserId;
                            usershift.UserIp = Request.UserHostAddress;

                            ObjUserShiftService.UpdateUserShift(usershift);
                        }
                    }

                    if (expectedRetiredDate != string.Empty)
                    {
                        UserShift _userShift = new UserShift()
                        {
                            UserId = int.Parse(userid),
                            ShiftId = ShiftId,
                            EffectiveDate = Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", EffectiveDate)),
                            RetiredDate = Convert.ToDateTime(expectedRetiredDate),
                            CreationDate = DateTime.Now,
                            UpdateBy = AuthBase.UserId,
                            UserIp = Request.UserHostAddress
                        };
                        ObjUserShiftService.InsertUserShift(_userShift);
                    }
                    else
                    {
                        UserShift _userShift = new UserShift()
                        {
                            UserId = int.Parse(userid),
                            ShiftId = ShiftId,
                            EffectiveDate = Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", EffectiveDate)),
                            CreationDate = DateTime.Now,
                            UpdateBy = AuthBase.UserId,
                            UserIp = Request.UserHostAddress
                        };
                        ObjUserShiftService.InsertUserShift(_userShift);
                    }
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }

        }

        private void ShiftPrerequisiteData()
        {
            List<SelectListItem> HourList = new List<SelectListItem>();
            for (int i = 0; i <= 23; i++)
            {
                HourList.Add(new SelectListItem() { Text = i.ToString().Length == 1 ? "0" + i.ToString() : i.ToString(), Value = i.ToString() });
            }
            ViewBag.HourList = new SelectList(HourList, "Value", "Text");

            List<SelectListItem> MinuteList = new List<SelectListItem>();
            MinuteList.Add(new SelectListItem() { Text = "00", Value = "00" });
            MinuteList.Add(new SelectListItem() { Text = "15", Value = "15" });
            MinuteList.Add(new SelectListItem() { Text = "30", Value = "30" });
            MinuteList.Add(new SelectListItem() { Text = "45", Value = "45" });
            ViewBag.MinuteList = new SelectList(MinuteList, "Value", "Text");

            //NewHourList For Break
            List<SelectListItem> BreakHourList = new List<SelectListItem>();
            for (int i = 0; i <= 10; i++)
            {
                BreakHourList.Add(new SelectListItem() { Text = i.ToString().Length == 1 ? "0" + i.ToString() : i.ToString(), Value = i.ToString() });
            }
            ViewBag.BreakHourList = new SelectList(BreakHourList, "Value", "Text");

            List<SelectListItem> BreakMinuteList = new List<SelectListItem>();
            BreakMinuteList.Add(new SelectListItem() { Text = "00", Value = "0.00" });
            BreakMinuteList.Add(new SelectListItem() { Text = "15", Value = "0.25" });
            BreakMinuteList.Add(new SelectListItem() { Text = "30", Value = "0.50" });
            BreakMinuteList.Add(new SelectListItem() { Text = "45", Value = "0.75" });
            ViewBag.BreakMinuteList = new SelectList(BreakMinuteList, "Value", "Text");
        }

        private void VariablePrerequisiteData()
        {
            IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
            List<Shift> ShiftList = objShiftService.GetAllShift();
            if (ShiftList != null && ShiftList.Count > 0 && ShiftList.Where(x => x.IsActive == true) != null)
                ViewBag.Shift = new SelectList(ShiftList.Where(x => x.IsActive == true).ToList(), "Id", "Name");
        }

        private void AssignShiftPrerequisiteData()
        {
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
            List<Department> DepartmentList = objDepartmentService.GetAllDepartment(AuthBase.BranchId);
            if (DepartmentList != null && DepartmentList.Count > 0)
                ViewBag.Department = new SelectList(DepartmentList, "Id", "Name");
            else
            {
                List<SelectListItem> blank = new List<SelectListItem>();
                blank.Add(new SelectListItem() { Text = "Department", Value = "-1" });
                ViewBag.Department = new SelectList(blank, "Value", "Text", "");
            }

            IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
            List<Shift> ShiftList = objShiftService.GetAllShift(AuthBase.BranchId);
            if (ShiftList != null && ShiftList.Count > 0)
                ViewBag.Shift = new SelectList(ShiftList, "Id", "Name");
            else
            {
                List<SelectListItem> blank = new List<SelectListItem>();
                blank.Add(new SelectListItem() { Text = "Shift", Value = "-1" });
                ViewBag.Shift = new SelectList(blank, "Value", "Text", "");
            }
        }
    }
}
