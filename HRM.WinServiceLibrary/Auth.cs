using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using Microsoft.Win32;
using System.Security.Cryptography;
namespace HRM.WinServiceLibrary
{
    public class Auth
    {
        public string mainErrorFile;
        public string ProcessorID { get; set; }
        public Auth(string mainErrorFile)
        {
            this.mainErrorFile = mainErrorFile;
            this.ProcessorID = GetProcessorId();
        }
        public bool AuthenticateFromRegistery(string guid)
        {
            if (ProcessorID == string.Empty)
            {
                //Log Processor
                return false;
            }
            String[] RegisteredEntries = GetRegisteredEntries("Config");
            return Matched(ProcessorID + guid, RegisteredEntries);
        }
        public bool Authenticate(string guid, string name)
        {
            System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Authenticating..." + Environment.NewLine);
            if (ProcessorID == string.Empty)
            {
                //Log Processor
                return false;
            }
            String[] RegisteredEntries = GetRegisteredEntries("Config");
            if (!Matched(ProcessorID + guid, RegisteredEntries))
            {
                System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Branch configuration not found - Name:" + name + Environment.NewLine);
                System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Requesting branch configuration - Name:" + name + Environment.NewLine);

                //Send Request
                if (SendRequest(guid, ProcessorID, name))
                {
                    System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Configuration recieved for branch - Name:" + name + Environment.NewLine);
                    System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Branch Configured - Name:" + name + Environment.NewLine);
                    SetRegisteryEntries("Config", Encrypt(ProcessorID + guid));
                    return true;
                }
                else
                {
                    System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Configuration not recieved for branch - Name:" + name + Environment.NewLine);
                }
                return false;
            }
            else
                return true;
        }

        public string GetProcessorId()
        {
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();
            String ProcessorId = String.Empty;
            foreach (ManagementObject mo in moc)
            {
                ProcessorId = mo.Properties["processorID"].Value.ToString();
                break;
            }
            return ProcessorId;
        }
        private string[] GetRegisteredEntries(string key)
        {
            RegistryKey rk = Registry.LocalMachine;

            if (rk.OpenSubKey("Software\\DeesCorp\\Attendance") == null)
                rk.CreateSubKey("Software\\DeesCorp\\Attendance");
            rk = rk.OpenSubKey("Software\\DeesCorp\\Attendance", false);

            if (rk == null)
                return new string[]{""};
            if (rk.GetValue(key) == null)
                return new string[] { "" };
            return (string[])rk.GetValue(key);
        }
        private void SetRegisteryEntries(string key,string value)
        {
            RegistryKey rk = Registry.LocalMachine;

            if(rk.OpenSubKey("Software\\DeesCorp\\Attendance")==null)
                rk.CreateSubKey("Software\\DeesCorp\\Attendance");
            rk=rk.OpenSubKey("Software\\DeesCorp\\Attendance",true);

            List<string> registeredValues = GetRegisteredEntries("Config").ToList();
            if (registeredValues.Count == 0 || registeredValues[0] == string.Empty)
                registeredValues[0] = value;
            else
                registeredValues.Add(value);
            if(rk!=null)
                rk.SetValue(key, registeredValues.ToArray());
        }

        private bool Matched(string str, string[] encStrs)
        {
            string enc = Encrypt(str);
            foreach (string encStr in encStrs)
            {
                if (enc.Length == encStr.Length)
                {
                    int i = 0;
                    while ((i < enc.Length) && (enc[i] == encStr[i]))
                    {
                        i += 1;
                    }
                    if (i == enc.Length)
                        return true;
                }
            }
            return false;
        }
        private string Encrypt(string str)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(str);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private bool SendRequest(string guid, string processorID, string name)
        {
            net.deescorp.auth.serviceauth objServiceauth = new net.deescorp.auth.serviceauth();
            string result = "";
            try
            {
                result = objServiceauth.Authenticate(guid, processorID, name);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Error in registeration - branch: " + name + " - Error: "+ex.Message+ Environment.NewLine);
            }
            return (result.ToLower() == "success");
        }
    }
}
