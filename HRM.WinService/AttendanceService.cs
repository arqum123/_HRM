using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using HRM.Core.Entities;
using HRM.Core.Helper;
using HRM.WinServiceLibrary;
namespace HRM.WinService
{
    public partial class AttendanceService : ServiceBase
    {
        bool isStopped = true;
        int stopCounter = 0;
        string mainErrorFile = AppDomain.CurrentDomain.BaseDirectory + "\\ServiceLog.txt";
        Auth objAuth = null;
        SystemConfiguration objSystemConfiguration = null;
        public AttendanceService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();
            
            try
            {
                isStopped = false;
                objAuth = new Auth(mainErrorFile);
                objSystemConfiguration = new SystemConfiguration();
                tmrAttendance.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["ServiceIimePeriod"]);
                if (tmrAttendance.Interval <= 20000)
                {
                    System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Service time period must be greater than 20 seconds: Current TimePeriod: " + tmrAttendance.Interval.ToString() + Environment.NewLine);
                    objAuth = null;
                    objSystemConfiguration = null;
                    return;
                }
                
                foreach (Branch _branch in objSystemConfiguration.GetAllActiveBranch())
                {
                    if (!objAuth.Authenticate(_branch.Guid, _branch.Name))
                    {
                        System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Branch not configured - Name:" + _branch.Name + Environment.NewLine);
                        continue;
                    }
                    else
                        System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Branch Authenticated - Name:" + _branch.Name + Environment.NewLine);

                }
                tmrAttendance.Enabled = true;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Error on Start - Error:" + ex.Message + Environment.NewLine);
            }
        }

        protected override void OnStop()
        {
            tmrAttendance.Enabled = false;
            isStopped = true;
        }

        private void tmrAttendance_Elapsed(object sender, EventArgs e)
        {
            tmrAttendance.Enabled = false;
            try
            {
                AttendanceFromDevice objAttendanceFromDevice = new AttendanceFromDevice();
                System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Configuration: isStopped:" + isStopped.ToString()+" stopCounter:"+stopCounter.ToString() + Environment.NewLine);
                if (!objSystemConfiguration.GetConfigurationBoolValue(HRM.Core.Enum.Constant.ServiceStatus) && !isStopped && stopCounter < 10)
                {
                    System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Service is paused" + Environment.NewLine);
                    stopCounter++;
                    tmrAttendance.Enabled = true;
                    return;
                }
                else if (stopCounter >= 10)
                {
                    stopCounter = 0;
                    objSystemConfiguration.SetConfigurationBoolValue(HRM.Core.Enum.Constant.ServiceStatus,true);
                }
                AttendanceMarking objAttendanceMarking = null;
                Branch _branch = null;
                try { objAttendanceFromDevice.InsertPreAttendance(); }
                catch (Exception ex) { System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "- InsertPreAttendance - Error: " + ex.Message + Environment.NewLine); }
                foreach (Device _device in objSystemConfiguration.GetAllActiveDevices())
                {
                    System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Device:" + _device.DeviceId.Value.ToString() + Environment.NewLine);
                    objAttendanceMarking = new AttendanceMarking(_device.DeviceModalId.Value,_device.MachineId.Value, _device.DeviceId.Value.ToString(), _device.IpAddress, _device.PortNumber.Value, _device.Password, _device.ComPort.Value, _device.Baudrate.Value.ToString(), _device.ConnectionTypeId.Value);
                    objAttendanceMarking.mainLogFilePath = mainErrorFile;
                    _branch = objSystemConfiguration.GetBranchInfo(_device.BranchId.Value);
                     if (!objAuth.AuthenticateFromRegistery(_branch.Guid))
                     {
                         System.IO.File.AppendAllText(mainErrorFile,DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]")+"-"+"Branch not configured - Name:"+_branch.Name+Environment.NewLine);
                         objSystemConfiguration.UpdateDeviceStatus(_device.Id, "ERROR", "Configuration not Found");
                         continue;
                     }
                    List<String> lines = objAttendanceMarking.GetAttendanceDataFromDevice();
                    objSystemConfiguration.UpdateDeviceStatus(_device.Id, objAttendanceMarking.connectionStatus, objAttendanceMarking.connectionStatusDesc);
                    if (lines != null && lines.Count > 0)
                    {
                        objAttendanceFromDevice.ProcessAttendance(objAttendanceMarking.logFilePath, objAttendanceMarking.errorFilePath, lines);
                    }
                    objAttendanceMarking = null;
                    _branch = null;
                }
                objAttendanceFromDevice = null;
                //objSystemConfiguration = null;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "- Error: " + ex.Message+Environment.NewLine);
            }
            if(!isStopped)
                tmrAttendance.Enabled = true;
        }
    }
}
