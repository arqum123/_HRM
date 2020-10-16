using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HRM.Core.Entities;
using HRM.Core.Enum;
using HRM.Core.IService;
using HRM.Core.Model;

namespace HRM.Core.Helper
{
    public class SystemConfiguration
    {
        public bool GetConfigurationBoolValue(string key)
        {
            bool serviceStatus;
            IConfigurationService objConfigurationService = IoC.Resolve<HRM.Core.IService.IConfigurationService>("ConfigurationService"); ;
            List<Configuration> _configurationList = objConfigurationService.GetAllConfiguration();
            if (_configurationList != null && _configurationList.Count > 0)
            {
                _configurationList = _configurationList.Where(x => x.Name == key).ToList();
                if (_configurationList != null && _configurationList.Count > 0)
                {
                    serviceStatus = Convert.ToBoolean(Convert.ToInt32(_configurationList.FirstOrDefault().Value));
                }
                else
                    serviceStatus = false;
            }
            else
                serviceStatus = false;
            return serviceStatus;
        }
        public void SetConfigurationBoolValue(string key,bool value)
        {
            IConfigurationService objConfigurationService = IoC.Resolve<HRM.Core.IService.IConfigurationService>("ConfigurationService"); ;
            List<Configuration> _configurationList = objConfigurationService.GetAllConfiguration();
            Configuration _configuration = null;
            if (_configurationList != null && _configurationList.Count > 0)
            {
                _configurationList = _configurationList.Where(x => x.Name == key).ToList();
                if (_configurationList != null && _configurationList.Count > 0)
                {
                    _configuration = _configurationList.FirstOrDefault();
                }

                if (_configuration != null)
                {
                    _configuration.BoolValue = value;
                    objConfigurationService.UpdateConfiguration(_configuration);
                }
            }
        }
        public List<Device> GetAllActiveDevices()
        {
            IDeviceService objDeviceService = IoC.Resolve<HRM.Core.IService.IDeviceService>("DeviceService"); ;
            List<Device> _DeviceList = objDeviceService.GetAllDevice();
            if (_DeviceList != null && _DeviceList.Count > 0)
            {
                _DeviceList = _DeviceList.Where(x => x.IsActive == true).ToList();
            }
            return _DeviceList;
        }
        public void UpdateDeviceStatus(int deviceID, string status, string desc)
        {
            IDeviceService objDeviceService = IoC.Resolve<HRM.Core.IService.IDeviceService>("DeviceService"); ;
            Device _Device = objDeviceService.GetDevice(deviceID);
            _Device.Status=status;_Device.StatusDescription=desc;
            _Device = objDeviceService.UpdateDevice(_Device);
        }
        public Branch GetBranchInfo(int branchId)
        {
            IBranchService objBranchService = IoC.Resolve<HRM.Core.IService.IBranchService>("BranchService"); ;
            return objBranchService.GetBranch(branchId);
        }
        public List<Branch> GetAllActiveBranch()
        {
            IBranchService objBranchService = IoC.Resolve<HRM.Core.IService.IBranchService>("BranchService"); ;
            List<Branch> _branchList= objBranchService.GetAllBranch();
            if (_branchList != null)
                _branchList = _branchList.Where(x => x.IsActive == true).ToList();
            else _branchList = new List<Branch>();
            return _branchList;
        }
    }
}
