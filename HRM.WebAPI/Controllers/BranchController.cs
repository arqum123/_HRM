using HRM.Core;
using HRM.Core.Entities;
using HRM.Core.Helper;
using HRM.Core.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.WebAPI.Controllers
{
    public class BranchController : Controller
    {
        IBranchService iBranchService = IoC.Resolve<IBranchService>("BranchService");
        //
        // GET: /Branch/
        [HttpGet]
        [Authenticate]
        public ActionResult Index()
        {
            List<Branch> _modelBranchList = new List<Branch>();
            try 
            {
                _modelBranchList = iBranchService.GetAllBranch();
                if (_modelBranchList != null)
                {
                    _modelBranchList = _modelBranchList.Where(x => x.IsActive == true).ToList();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(_modelBranchList);
        }

        [HttpGet]
        [Authenticate]
        public ActionResult Create(string bid = null)
        {
            int branchID = 0;
            Branch _modelBranch = new Branch();
            try
            {
                if (int.TryParse(bid, out branchID))
                {
                    _modelBranch = iBranchService.GetBranch(branchID);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(_modelBranch);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult Create(Branch _modelBranch)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_modelBranch.Id != 0)
                    {
                        _modelBranch.UpdatedBy = AuthBase.UserId;
                        _modelBranch.UpdationDate = DateTime.Now;
                        _modelBranch.UserIp = Request.UserHostAddress;
                        iBranchService.UpdateBranch(_modelBranch);
                    }
                    else
                    {
                        _modelBranch.Guid = Guid.NewGuid().ToString().Replace("-","");
                        _modelBranch.CreatedDate = DateTime.Now;
                        _modelBranch.IsActive = true;
                        _modelBranch.UserIp = Request.UserHostAddress;
                        iBranchService.InsertBranch(_modelBranch);
                    }
                    return RedirectToAction("Index", "Branch");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ModelState.AddModelError("", "Unable to process your request. Please contact Administration");
            return View(_modelBranch);
        }

        [HttpPost]
        public JsonResult DeleteBranch(int id)
        {
            try
            {
                Dictionary<string, string> dictBranch = new Dictionary<string, string>();
                dictBranch.Add("IsActive", "false");
                dictBranch.Add("UpdationDate", DateTime.Now.ToString());
                dictBranch.Add("UpdatedBy", AuthBase.UserId.ToString());
                dictBranch.Add("UserIP", Request.UserHostAddress);

                iBranchService.UpdateBranchByKeyValue(dictBranch, id);
                return base.Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return base.Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
