using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HRM.WinServiceLibrary
{
    public class AttendanceMarking
    {
        int deviceModalId;
        int machineNumber;
        string deviceId;
        string ipAddress;
        int port;
        int password;
        int comPort;
        int braudRate;
        bool markRead;
        int connectionType;
        public zkemkeeper.CZKEMClass axCZKEM1 = null;
        List<GLogInfo> glogs_ = new List<GLogInfo>();
        List<GLogZKTiFace702Info> glogsZKTiFace702 = new List<GLogZKTiFace702Info>();
        List<String> lines = new List<String>();
        public string logFilePath { get; set; }
        public string errorFilePath { get; set; }
        public string mainLogFilePath { get; set; }
        bool isConnected = false;
        public string connectionStatus { get; set; }
        public string connectionStatusDesc { get; set; }
        public AttendanceMarking(int deviceModalId, int machineNumber, string deviceId, string ipAddress, int port, string password, int comPort, string braudRate, int connectionType)
        {
            this.deviceModalId = deviceModalId;
            this.machineNumber = machineNumber;
            this.deviceId = deviceId;
            this.ipAddress = ipAddress;
            this.port = port;
            Int32.TryParse(password, out this.password);
            //this.password = password;
            this.comPort = comPort;
            Int32.TryParse(braudRate, out this.braudRate);
            //this.braudRate = braudRate;
            this.connectionType = connectionType;
            GenerateLogFile();
            GenerateErrorLogFile();
        }
        public List<String> GetAttendanceDataFromDevice()
        {
            //Init
            if (Init())
            {
                //Connect to device
                if (Connect())
                {
                    connectionStatus = "CONNECTED";

                    isConnected = true;
                    //Read from device
                    switch (deviceModalId)
                    {
                        case 1://SB
                            GetSBNewRecord();
                            break;
                        case 2: //ZKT iFace 702
                            GetZKTiFace702Record();
                            break;
                    }
                }
                else
                {
                    connectionStatus = "ERROR";
                    connectionStatusDesc = "Level 1: Unable to connect";
                    WriteError("Unable to Connect to device.");
                }
            }
            else
            {
                connectionStatus = "ERROR";
                connectionStatusDesc = "Level 0: Unable to connect";
                //Error in init or log file creation
            }
            if (isConnected)
                connectionStatusDesc = "OK";
            isConnected = Disconnect();
            return lines;
        }
        private bool Connect()
        {
            try
            {
                switch (deviceModalId)
                {
                    case 1://SB
                        switch (connectionType)
                        {
                            case 1:
                                return sbxpc.SBXPCDLL.ConnectTcpip(machineNumber, ipAddress, port, password);
                            case 2:
                                return sbxpc.SBXPCDLL.ConnectSerial(machineNumber, comPort, braudRate);
                            case 3:
                                return sbxpc.SBXPCDLL.ConnectSerial(machineNumber, 0, 0);
                            default:
                                return false;
                        }
                    case 2: //ZKT iFace 702
                        return axCZKEM1.Connect_Net(ipAddress, port);
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }
        private bool Init()
        {
            switch (deviceModalId)
            {
                case 1://SB
                    markRead = true;
                    try
                    {
                        sbxpc.SBXPCDLL.DotNET();
                        sbxpc.SBXPCDLL._DisableTranseiveCallback();
                    }
                    catch (Exception ex)
                    {
                        System.IO.File.AppendAllText(mainLogFilePath, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Init Error: " + ex.Message + Environment.NewLine);
                        return false;
                    }
                    break;
                case 2: //ZKT iFace 702
                    try
                    {
                    axCZKEM1 = new zkemkeeper.CZKEMClass();
                    }
                    catch (Exception ex)
                    {
                        System.IO.File.AppendAllText(mainLogFilePath, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Init Error: " + ex.Message + Environment.NewLine);
                        return false;
                    }
                    break;
                default:
                    System.IO.File.AppendAllText(mainLogFilePath, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Init Error: Unknown Device Modal: "+ deviceModalId.ToString()  + Environment.NewLine);
                    return false;
            }
            return true;
        }
        private bool Disconnect()
        {
            if (isConnected)
            {
                switch (deviceModalId)
                {
                    case 1://SB
                        sbxpc.SBXPCDLL.Disconnect(machineNumber);
                        break;
                    case 2: //ZKT iFace 702
                        axCZKEM1.Disconnect();
                        break;
                }
            }
            return false;

        }
        private string GenerateLogFile()
        {
            DateTime dtCurrent = DateTime.Now;
            try
            {
                logFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + dtCurrent.ToString("yyyyMMdd");
                if (!Directory.Exists(logFilePath))
                    Directory.CreateDirectory(logFilePath);

                logFilePath += "\\" + dtCurrent.ToString("HH") + ".txt";
            }
            catch
            {
                return "";
            }
            return logFilePath;
        }
        private string GenerateErrorLogFile()
        {
            DateTime dtCurrent = DateTime.Now;
            try
            {
                errorFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + dtCurrent.ToString("yyyyMMdd");
                if (!Directory.Exists(errorFilePath))
                    Directory.CreateDirectory(errorFilePath);

                errorFilePath += "\\" + dtCurrent.ToString("HH") + "Error.txt";
            }
            catch
            {
                return "";
            }
            return errorFilePath;
        }
        private void GetSBNewRecord()
        {
            int vErrorCode = 0;
            Boolean vRet;
            try { vRet = sbxpc.SBXPCDLL.EnableDevice(machineNumber, 0); }
            catch { vRet = false; }

            if (!vRet) { WriteError("Unable to Disable device"); }

            try { vRet = sbxpc.SBXPCDLL.ReadGeneralLogData(machineNumber, markRead ? (byte)1 : (byte)0); }
            catch { vRet = false; }
            //vRet = sbxpc.SBXPCDLL.ReadAllGLogData(machineNumber);
            if (!vRet)
            {
                try { sbxpc.SBXPCDLL.GetLastError(machineNumber, out vErrorCode); }
                catch { vErrorCode = 11001; }
                WriteError("Unable to Read Super Log Data - ERROR CODE:" + vErrorCode);
            }
            else
            {
                int readError = 0;
                while (true && readError <= 3)
                {
                    GLogInfo gi = new GLogInfo();
                    try
                    {
                        /*vRet = sbxpc.SBXPCDLL.GetAllGLogData(machineNumber,
                                                     out gi.tmno,
                                                     out gi.seno,
                                                     out gi.smno,
                                                     out gi.vmode,
                                                     out gi.yr,
                                                     out gi.mon,
                                                     out gi.day,
                                                     out gi.hr,
                                                     out gi.min,
                                                     out gi.sec);
                       */
                        vRet = sbxpc.SBXPCDLL.GetGeneralLogData(machineNumber,
                                                     out gi.tmno,
                                                     out gi.seno,
                                                     out gi.smno,
                                                     out gi.vmode,
                                                     out gi.yr,
                                                     out gi.mon,
                                                     out gi.day,
                                                     out gi.hr,
                                                     out gi.min,
                                                     out gi.sec);
                        if (!vRet) break;
                        WriteLog(gi);
                        glogs_.Add(gi);
                    }
                    catch (Exception ex)
                    {
                        readError++;
                        WriteError("Unable to read data.#" + readError.ToString() + " Error Msg: " + ex.Message);
                    }
                }
            }
            try { vRet = sbxpc.SBXPCDLL.EnableDevice(machineNumber, 1); }
            catch { vRet = false; }
            if (!vRet)
            {
                WriteError("Unable to Enable device");
            }
        }
        private void GetZKTiFace702Record()
        {
            int vErrorCode = 0;
            
            Boolean vRet;
            try { vRet = axCZKEM1.DisableDeviceWithTimeOut(machineNumber, 30); /*axCZKEM1.EnableDevice(machineNumber, false);*/ }
            catch { vRet = false; }

            if (!vRet) { WriteError("Unable to Disable device"); }

            vRet = axCZKEM1.ReadGeneralLogData(machineNumber);
            if (!vRet)
            {
                try { axCZKEM1.GetLastError(ref vErrorCode); }
                catch { vErrorCode = 11001; }
                WriteError("Unable to Read Super Log Data - ERROR CODE:" + vErrorCode);
            }
            else
            {
                GLogZKTiFace702Info gi = new GLogZKTiFace702Info();
                try
                {
                    while (axCZKEM1.SSR_GetGeneralLogData(machineNumber, out gi.enrollNo, out gi.verifyMode,
                                out gi.inOutMode, out gi.year, out gi.month, out gi.day, out gi.hour, out gi.minute, out gi.second, ref gi.workCode))//get records from the memory
                    {
                        
                        WriteLog(gi);
                        glogsZKTiFace702.Add(gi);
                        gi = new GLogZKTiFace702Info();
                    }
                    axCZKEM1.ClearGLog(machineNumber);
                }
                catch (Exception ex)
                {
                    WriteError("Unable to read data." + " Error Msg: " + ex.Message);
                }
            }

            try { vRet = axCZKEM1.EnableDevice(machineNumber, true); }
            catch { vRet = false; }
            if (!vRet)
            {
                WriteError("Unable to Disable device");
            }
        }
        private void WriteLog(GLogInfo gi)
        {
            string logData;
            if (gi != null)
            {
                logData = gi.smno + "\t" +
                    gi.tmno + "\t" +
                     gi.seno + "\t" +
                     "-\t" + //name
                     "-\t" + //GMNO
                     gi.vmode + "\t" +
                     gi.verify_mode + "\t" +
                     "-\t" + //AntiPass
                     "-\t" + //ProxyWork
                     gi.yr + "-" +
                     gi.mon + "-" +
                     gi.day + " " +
                     gi.hr + ":" +
                     gi.min + ":" +
                     gi.sec;
                lines.Add(logData);
                File.AppendAllText(logFilePath, logData + Environment.NewLine);
            }
        }
        private void WriteLog(GLogZKTiFace702Info gi)
        {
            string logData;
            if (gi != null)
            {
                logData = "0" + "\t" +
                    "0" + "\t" +
                     gi.enrollNo + "\t" +
                     "-\t" + //name
                     "-\t" + //GMNO
                     gi.verifyMode + "\t" +
                     gi.inOutMode + "\t" +
                     "-\t" + //AntiPass
                     "-\t" + //ProxyWork
                     gi.year + "-" +
                     gi.month + "-" +
                     gi.day + " " +
                     gi.hour + ":" +
                     gi.minute + ":" +
                     gi.second + "\t" + 
                     machineNumber;
                lines.Add(logData);
                File.AppendAllText(logFilePath, logData + Environment.NewLine);
            }
        }
        private void WriteError(string msg)
        {
            File.AppendAllText(errorFilePath, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "_" + msg + Environment.NewLine);
        }
        public void ClearLogDataZKT()
        {
            int idwErrorCode = 0;
            axCZKEM1.EnableDevice(1, false);//disable the device
            if (axCZKEM1.ClearGLog(1))
            {
                axCZKEM1.RefreshData(1);//the data in the device should be refreshed
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
            }
            axCZKEM1.EnableDevice(1, true);//enable the device
        }
    }

    public class SLogInfo
    {
        public int tmno = 0;
        public int seno = 0, smno = 0;
        public int geno = 0, gmno = 0;
        public int mnpl = 0;
        public int fpno = 0;
        public int yr = 0, mon = 0, day = 0, hr = 0, min = 0, sec = 0;

        public int tmachine { get { return tmno; } }
        public int senroll { get { return seno; } }
        public int smachine { get { return smno; } }
        public int genroll { get { return geno; } }
        public int gmachine { get { return gmno; } }
        public string manipulation
        {
            get
            {
                switch (mnpl)
                {
                    case 1:
                    case 2:
                    case 3:
                        return Convert.ToString(mnpl) + "--" + "Enroll User";

                    case 4:
                        return Convert.ToString(mnpl) + "--" + "Enroll Manager";

                    case 5:
                        return Convert.ToString(mnpl) + "--" + "Delete Fp Data";

                    case 6:
                        return Convert.ToString(mnpl) + "--" + "Delete Password";

                    case 7:
                        return Convert.ToString(mnpl) + "--" + "Delete Card Data";

                    case 8:
                        return Convert.ToString(mnpl) + "--" + "Delete All LogData";

                    case 9:
                        return Convert.ToString(mnpl) + "--" + "Modify System Info";

                    case 10:
                        return Convert.ToString(mnpl) + "--" + "Modify System Time";

                    case 11:
                        return Convert.ToString(mnpl) + "--" + "Modify Log Setting";

                    case 12:
                        return Convert.ToString(mnpl) + "--" + "Modify Comm Setting";

                    case 13:
                        return Convert.ToString(mnpl) + "--" + "Modify Timezone Setting";

                    case 14:
                        return Convert.ToString(mnpl) + "--" + "Delete Face";

                    default:
                        return Convert.ToString(mnpl) + "--" + "Unknown";
                }
            }
        }

        public string finger { get { return (fpno < 10) ? Convert.ToString(fpno) : (fpno == 10) ? "Password" : (fpno == 14) ? "Face" : "Card"; } }
        public string logtime { get { return string.Format("{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}", yr, mon, day, hr, min, sec); } }
    }

    public class GLogInfo
    {
        public int tmno;
        public int smno, seno;
        public int vmode;
        public int yr, mon, day, hr, min, sec;

        public string photo { get { return (tmno == -1) ? "No Photo" : Convert.ToString(tmno); } }
        public int enroll { get { return seno; } }
        public int machine { get { return smno; } }

        public string verify_mode
        {
            get
            {
                string attend_status = "";
                switch ((vmode >> 8) & 0xFF)
                {
                    case 0: attend_status = "_DutyOn"; break;
                    case 1: attend_status = "_DutyOff"; break;
                    case 2: attend_status = "_OverOn"; break;
                    case 3: attend_status = "_OverOff"; break;
                    case 4: attend_status = "_GoIn"; break;
                    case 5: attend_status = "_GoOut"; break;
                }

                string antipass = "";
                switch ((vmode >> 16) & 0xFFFF)
                {
                    case 1: antipass = "(AP_In)"; break;
                    case 3: antipass = "(AP_Out)"; break;
                }


                int vm = vmode & 0xFF;
                string str = "--";
                switch (vm)
                {
                    case 1: str = "Fp"; break;
                    case 2: str = "Password"; break;
                    case 3: str = "Card"; break;
                    case 4: str = "FP+Card"; break;
                    case 5: str = "FP+Pwd"; break;
                    case 6: str = "Card+Pwd"; break;
                    case 7: str = "FP+Card+Pwd"; break;
                    case 10: str = "Hand Lock"; break;
                    case 11: str = "Prog Lock"; break;
                    case 12: str = "Prog Open"; break;
                    case 13: str = "Prog Close"; break;
                    case 14: str = "Auto Recover"; break;
                    case 20: str = "Lock Over"; break;
                    case 21: str = "Illegal Open"; break;
                    case 22: str = "Duress alarm"; break;
                    case 23: str = "Tamper detect"; break;
                    case 30: str = "FACE"; break;
                    case 31: str = "FACE+CARD"; break;
                    case 32: str = "FACE+PWD"; break;
                    case 33: str = "FACE+CARD+PWD"; break;
                    case 34: str = "FACE+FP"; break;
                    case 51: str = "Fp"; break;
                    case 52: str = "Password"; break;
                    case 53: str = "Card"; break;
                    case 101: str = "Fp"; break;
                    case 102: str = "Password"; break;
                    case 103: str = "Card"; break;
                    case 151: str = "Fp"; break;
                    case 152: str = "Password"; break;
                    case 153: str = "Card"; break;
                }

                if ((1 <= vm && vm <= 7) ||
                    (30 <= vm && vm <= 34) ||
                    (51 <= vm && vm <= 53) ||
                    (101 <= vm && vm <= 103) ||
                    (151 <= vm && vm <= 153))
                {
                    str = str + attend_status;
                }

                str += antipass;

                return str;
            }
        }

        public string logtime { get { return string.Format("{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}", yr, mon, day, hr, min, sec); } }
    }
    public class GLogZKTiFace702Info
    {
        public string enrollNo="0";
        public int verifyMode = 0, inOutMode = 0, year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, workCode = 0;
    }
}
