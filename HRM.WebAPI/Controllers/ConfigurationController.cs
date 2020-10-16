using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Core;
using HRM.Core.Entities;
using HRM.Core.Helper;
using HRM.Core.IService;

namespace HRM.WebAPI.Controllers
{
    public class ConfigurationController : Controller
    {
        //
        // GET: /Configuration/

        public ActionResult Index()
        {
            IConfigurationService objConfigurationService = IoC.Resolve<IConfigurationService>("ConfigurationService");
            List<Configuration> _configurationList = objConfigurationService.GetAllConfiguration();
            if (_configurationList != null && _configurationList.Count > 0)
                _configurationList = _configurationList.Where(x => x.IsActive == true).ToList();

            return View(_configurationList);
        }
        [HttpPost]
        public ActionResult Index(List<HRM.Core.Entities.Configuration> _configurationList)
        {
            IConfigurationService objConfigurationService = IoC.Resolve<IConfigurationService>("ConfigurationService");
            foreach (Configuration _configuration in _configurationList)
            {
                _configuration.UpdateDate = DateTime.Now;
                _configuration.UpdateBy = AuthBase.UserId;
                _configuration.UserIp = Request.UserHostAddress;
                objConfigurationService.UpdateConfiguration(_configuration);
            }

            ViewBag.Message = "Configuration updated successfully.";
            return View(_configurationList);
        }

        public ActionResult Device()
        {
            IDeviceService objDeviceService = IoC.Resolve<IDeviceService>("DeviceService");
            List<Device> _DeviceList = objDeviceService.GetAllDevice();
            if (_DeviceList != null && _DeviceList.Count > 0)
                _DeviceList = _DeviceList.Where(x => x.IsActive == true).ToList();
            IBranchService objBranchService = IoC.Resolve<IBranchService>("BranchService");
            IDeviceModalService objDeviceModalService = IoC.Resolve<IDeviceModalService>("DeviceModalService");
            foreach(Device _device in _DeviceList)
            {
                _device.BranchName = objBranchService.GetBranch(_device.BranchId.Value).Name;
                _device.DeviceModalName = objDeviceModalService.GetDeviceModal(_device.DeviceModalId.Value).DeviceModal;
            }
            return View(_DeviceList);
        }

        public ActionResult DeviceConfig(int id=0)
        {
            IDeviceService objDeviceService = IoC.Resolve<IDeviceService>("DeviceService");
            
            Device _Device =null;
            if (id == 0)
                _Device = new Device() { Id = id };
            else
                _Device = objDeviceService.GetDevice(id);

            BindDeviceConfigOG();
            return View(_Device);
        }
        [HttpPost]
        public ActionResult DeviceConfig(Device _Device)
        {
            BindDeviceConfigOG();
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Some fields has invalid data.";
                return View(_Device);
            }
            IDeviceService objDeviceService = IoC.Resolve<IDeviceService>("DeviceService");
            _Device.UserIp = Request.UserHostAddress;
            _Device.UpdateBy = AuthBase.UserId;
            _Device.MachineId = _Device.DeviceId;
            if (_Device.Id == 0)
            {
                _Device.CreationDate = DateTime.Now;
                _Device.IsActive = true;
                _Device.Status = "New";
                _Device.StatusDescription = "New Device Added";
                _Device=objDeviceService.InsertDevice(_Device);
            }
            else
            {
                
                _Device.UpdateDate = DateTime.Now;
                _Device.Status = "Recently Updated";
                _Device.StatusDescription = "Device Configuration recently updated";
                _Device = objDeviceService.UpdateDevice(_Device);
            }
            Response.Redirect("/Configuration/Device");
            return View(_Device);
        }
        private void BindDeviceConfigOG()
        {
            var DeviceConnectionType = from HRM.Core.Enum.DeviceConnectionType d in Enum.GetValues(typeof(HRM.Core.Enum.DeviceConnectionType))
                                 select new { ID = (int)d, Name = d.ToString() };
            ViewBag.DeviceConnectionTypeList = new SelectList(DeviceConnectionType, "ID", "Name", HRM.Core.Enum.DeviceConnectionType.Network);

            var ComPort = from HRM.Core.Enum.ComPort d in Enum.GetValues(typeof(HRM.Core.Enum.ComPort))
                                 select new { ID = (int)d, Name = d.ToString() };
            ViewBag.ComPortList = new SelectList(ComPort, "ID", "Name", HRM.Core.Enum.ComPort.NotSelected);

            var BraudRate = from HRM.Core.Enum.BraudRate d in Enum.GetValues(typeof(HRM.Core.Enum.BraudRate))
                                 select new { ID = (int)d, Name = d.ToString() };
            ViewBag.BraudRateList = new SelectList(BraudRate, "ID", "Name", HRM.Core.Enum.BraudRate.NotSelected);

            IBranchService objBranchService = IoC.Resolve<IBranchService>("BranchService");
            ViewBag.BranchList = new SelectList(objBranchService.GetAllBranch(), "ID", "Name");

            IDeviceModalService objDeviceModalService = IoC.Resolve<IDeviceModalService>("DeviceModalService");
            ViewBag.DeviceModalList = new SelectList(objDeviceModalService.GetAllDeviceModal(), "ID", "DeviceModal");
            

        }
    }
}
