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
    [Authorize(Roles = "Admin")]
    public class HolidayController : Controller
    {
        //
        // GET: /Holiday/

        [HttpGet]
        [Authenticate]
        public ActionResult Index(string Id)
        {
            Holiday model = new Holiday();
            if (Id != null && Id != "")
            {
                IHolidayService objHolidayService = IoC.Resolve<IHolidayService>("HolidayService");
                model = objHolidayService.GetHoliday(int.Parse(Id));
            }
            return View(model);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult Index(Holiday model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id != 0)
                    {
                        #region Holiday Updation
                        model.UpdateDate = DateTime.Now;
                        model.UpdateBy = AuthBase.UserId;
                        model.UserIp = Request.UserHostAddress;

                        IHolidayService objHolidayService = IoC.Resolve<IHolidayService>("HolidayService");
                        Holiday _holiday = objHolidayService.UpdateHoliday(model);
                        #endregion
                    }
                    else
                    {
                        #region Holiday Insertion
                        IHolidayService objHolidayService = IoC.Resolve<IHolidayService>("HolidayService");

                        model.IsActive = true;
                        model.CreationDate = DateTime.Now;
                        model.UpdateBy = AuthBase.UserId;
                        model.UserIp = Request.UserHostAddress;
                        objHolidayService.InsertHoliday(model);
                        #endregion
                    }
                    return RedirectToAction("HolidayList", "Holiday");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            }
            ModelState.AddModelError("", "Unable to process your request. Please contact Administration");
            return View();
        }

        [HttpGet]
        [Authenticate]
        public ActionResult HolidayList()
        {
            VMHolidayModel model = new VMHolidayModel();
            try
            {
                IHolidayService objHolidayService = IoC.Resolve<IHolidayService>("HolidayService");
                List<Holiday> HolidayList = objHolidayService.GetAllHoliday();
                if (HolidayList != null && HolidayList.Count > 0)
                {
                    HolidayList = HolidayList.Where(x => x.IsActive == true).ToList();
                    if (HolidayList.Count > 0)
                    {
                        model.HolidayList = HolidayList;
                    }
                    else
                        ModelState.AddModelError("", "No Records Found");
                }
                else
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult DeleteHoliday(int id)
        {
            try
            {
                Dictionary<string, string> dictHoliday = new Dictionary<string, string>();
                dictHoliday.Add("IsActive", "false");
                dictHoliday.Add("UpdateDate", DateTime.Now.ToString());
                dictHoliday.Add("UpdateBy", AuthBase.UserId.ToString());
                dictHoliday.Add("UserIP", Request.UserHostAddress);

                IHolidayService ObjHolidayService = IoC.Resolve<IHolidayService>("HolidayService");
                ObjHolidayService.UpdateHolidayByKeyValue(dictHoliday, id);
                return base.Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return base.Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
